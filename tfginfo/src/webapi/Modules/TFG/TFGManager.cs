using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Models;
using TFGinfo.Objects;

namespace TFGinfo.Api
{
    public class TFGManager : BaseManager
    {
        private readonly EmailService? emailService;
        private readonly IConfiguration? configuration;
        public TFGManager(ApplicationDbContext context, EmailService? emailService = null, IConfiguration? configuration = null) : base(context)
        {
            this.emailService = emailService;
            this.configuration = configuration;
        }

        public List<TFGDTO> GetAllTFGs()
        {
            return context.tfg.Include(d => d.tfgLineModel).ToList().ConvertAll(model => new TFGDTO(model));
        }

        public TFGDTO CreateTFG(TFGFlatDTO TFG)
        {

            TFGModel model = new TFGModel
            {
                startDate = TFG.startDate.Value,
                tfg_line = TFG.tfgLineId
            };
            context.tfg.Add(model);
            context.SaveChanges();

            var savedTFG = context.tfg
                .Where(d => d.id == model.id)
                .Include(d => d.tfgLineModel)
                .FirstOrDefault() ?? model;

            return new TFGDTO(savedTFG);
        }

        public void DeleteTFG(int id)
        {
            TFGModel? model = context.tfg.FirstOrDefault(TFG => TFG.id == id);
            if (model == null)
            {
                throw new NotFoundException();
            }
            context.tfg.Remove(model);
            context.SaveChanges();
        }

        public TFGDTO UpdateTFG(TFGFlatDTO TFG)
        {
            TFGModel? model = context.tfg.Include(d => d.tfgLineModel).FirstOrDefault(d => d.id == TFG.id);
            if (model == null)
            {
                throw new NotFoundException();
            }

            model.startDate = TFG.startDate.Value;
            context.SaveChanges();

            return new TFGDTO(model);
        }

        public List<TFGDTO> GetTFGsByTFGLine(int tfgLineId)
        {
            return context.tfg.Where(TFG => TFG.tfg_line == tfgLineId).Include(d => d.tfgLineModel).ToList().ConvertAll(model => new TFGDTO(model));
        }

        public TFGDTO GetTFGById(int id)
        {
            TFGModel? model = context.tfg.Include(d => d.tfgLineModel).FirstOrDefault(d => d.id == id);
            if (model == null)
            {
                throw new NotFoundException();
            }
            return new TFGDTO(model);
        }

        public List<TFGDTO> SearchTFGs(List<Filter> filters)
        {
            var query = context.tfg.Include(d => d.tfgLineModel).ThenInclude(d => d.departmentModel).ThenInclude(d => d.Universities).AsQueryable();

            foreach (var filter in filters)
            {
                if (filter.key == "university" && filter.value != "0")
                {
                    query = query.Where(tfg => tfg.tfgLineModel.departmentModel.Universities.Any(u => u.university == int.Parse(filter.value)));
                }
                else if (filter.key == "universities" && filter.value != "0")
                {
                    // Assuming 'universities' is a comma-separated list of university IDs
                    var universityIds = filter.value.Split(',').Select(int.Parse).ToList();
                    query = query.Where(tfg => tfg.tfgLineModel.departmentModel.Universities.Any(u => universityIds.Contains(u.university)));
                }
                else if (filter.key == "startDate")
                {
                    query = query.Where(tfg => tfg.startDate == DateTime.Parse(filter.value));
                }
            }

            return query.ToList().ConvertAll(model => new TFGDTO(model));
        }

        public async Task RequestTFG(TFGRequest request)
        {
            // Check if the TFGLine exists
            var tfg = context.tfg_line.FirstOrDefault(t => t.id == request.tfg.tfgLineId);
            if (tfg == null)
            {
                throw new NotFoundException("TFG Line not found");
            }
            // Check if the professor exists
            var professor = context.professor.FirstOrDefault(p => p.id == request.professorId);
            if (professor == null)
            {
                throw new NotFoundException("Professor not found");
            }
            // Check if the secondary professor exists
            if (request.secondaryProfessorId.HasValue)
            {
                var secondaryProfessor = context.professor.FirstOrDefault(p => p.id == request.secondaryProfessorId.Value);
                if (secondaryProfessor == null)
                {
                    throw new NotFoundException("Secondary professor not found");
                }
            }
            // Check if the student exists
            int studentId = context.student.FirstOrDefault(s => s.email == request.studentEmail)?.id ?? 0;
            if (studentId == 0)
            {
                throw new NotFoundException("Student not found");
            }
            // Check if the the student has already requested a TFG with the same line and professor
            bool existingTfg = context.tfg.Include(t => t.Students).Include(t => t.Professors).Any(t =>
                t.Students.Any(s => s.student == studentId) &&
                t.Professors.Any(p => p.professor == request.professorId) &&
                t.tfg_line == request.tfg.tfgLineId);

            if (existingTfg)
            {
                throw new UnprocessableException("TFG already requested by this student for this line and professor");
            }


            TFGModel tfgModel = new TFGModel
            {
                startDate = DateTime.Now,
                external_tutor_name = request.tfg.external_tutor_name,
                external_tutor_email = request.tfg.external_tutor_email,
                status = 0,
                tfg_line = request.tfg.tfgLineId
            };
            context.tfg.Add(tfgModel);
            context.SaveChanges();

            tfgModel.Students ??= new List<TFGStudentModel>();
            tfgModel.Professors ??= new List<TFGProfessorModel>();

            tfgModel.Students.Add(new TFGStudentModel { student = studentId, tfg = tfgModel.id });
            tfgModel.Professors.Add(new TFGProfessorModel { professor = request.professorId, tfg = tfgModel.id });
            if (request.secondaryProfessorId.HasValue)
            {
                tfgModel.Professors.Add(new TFGProfessorModel { professor = request.secondaryProfessorId.Value, tfg = tfgModel.id });
            }

            if (emailService == null)
            {
                throw new UnprocessableException("Email service not available");
            }
            if (configuration == null)
            {
                throw new UnprocessableException("Configuration not available");
            }
            // Send email to the professor
            var body = $"Has recibido una solicitud de TFG de {request.studentEmail} para la línea {tfg.name}.\n\n" +
                $"Puedes ver la solicitud en el portal de gestión de TFGs: {configuration.GetSection("app:url").Value}.\n\n";
            await emailService.SendEmailAsync(professor.email, "Solicitud TFG", body);

            context.tfg.Update(tfgModel);
            context.SaveChanges();
        }

        public List<TFGPendingRequestDTO> GetTFGsByProfessor(int id)
        {
            List<TFGModel> tfgs = context.tfg_professor
                .Include(d => d.tfgModel).ThenInclude(d => d.tfgLineModel)
                .Include(d => d.tfgModel.Students).ThenInclude(d => d.studentModel)
                .Where(d => d.professor == id)
                .Select(d => d.tfgModel)
                .ToList();
            List<TFGPendingRequestDTO> tfgsDTO = tfgs.ConvertAll(model => new TFGPendingRequestDTO
            {
                tfgId = model.id,
                studentName = model.Students.FirstOrDefault()?.studentModel.name + " " + model.Students.FirstOrDefault()?.studentModel.surname,
                tfgName = model.tfgLineModel.name,
                tfgStatus = model.status
            });

            return tfgsDTO;
        }

        public async Task AcceptTFG(int tfgId)
        {
            Console.WriteLine($"Accepting TFG with ID: {tfgId}");
            TFGModel? model = context.tfg
                .Include(d => d.tfgLineModel)
                .Include(d => d.Professors).ThenInclude(d => d.professorModel)
                .Include(d => d.Students).ThenInclude(d => d.studentModel)
                .Where(d => d.id == tfgId)
                .FirstOrDefault();
            if (model == null)
            {
                throw new NotFoundException("TFG not found");
            }
            model.status = 1;
            context.SaveChanges();


            WorkingGroupManager wgManager = new WorkingGroupManager(context);
            WorkingGroupBase wg = new WorkingGroupBase
            {
                name = model.tfgLineModel.name,
                description = model.tfgLineModel.description,
                isPrivate = true
            };
            List<int> students = model.Students.Select(s => s.student).ToList();
            List<int> professors = model.Professors.Select(p => p.professor).ToList();
            List<int> tfgs = new List<int> { tfgId };
            wgManager.CreateWorkingGroup(wg, professors, students, tfgs);

            if (emailService == null)
            {
                throw new UnprocessableException("Email service not available");
            }
            if (configuration == null)
            {
                throw new UnprocessableException("Configuration not available");
            }
            // Send email to the student
            var body = $"Tu solicitud de TFG ha sido aceptada por el profesor {model.Professors.FirstOrDefault()?.professorModel.name}.\n\n" +
                $"Puedes ver la solicitud en el portal de gestión de TFGs: {configuration.GetSection("app:url").Value}.\n\n";
            await emailService.SendEmailAsync(model.Students.FirstOrDefault()?.studentModel.email, "Solicitud TFG aceptada", body);
        }

        public async Task RejectTFG(int tfgId)
        {
            TFGModel? model = context.tfg
                .Include(d => d.tfgLineModel)
                .Include(d => d.Students).ThenInclude(d => d.studentModel)
                .Include(d => d.Professors).ThenInclude(d => d.professorModel)
                .Where(d => d.id == tfgId).FirstOrDefault();
            if (model == null)
            {
                throw new NotFoundException("TFG not found");
            }
            var studentEmail = model.Students.FirstOrDefault()?.studentModel.email;
            context.tfg.Remove(model);
            context.tfg_professor.RemoveRange(context.tfg_professor.Where(d => d.tfg == tfgId));
            context.tfg_student.RemoveRange(context.tfg_student.Where(d => d.tfg == tfgId));

            context.SaveChanges();

            if (emailService == null)
            {
                throw new UnprocessableException("Email service not available");
            }
            if (configuration == null)
            {
                throw new UnprocessableException("Configuration not available");
            }
            // Send email to the student
            var body = $"Tu solicitud de TFG ha sido rechazada por el profesor {model.Professors.FirstOrDefault()?.professorModel.name}.\n\n" +
                $"Puedes ver la solicitud en el portal de gestión de TFGs: {configuration.GetSection("app:url").Value}.\n\n";
            await emailService.SendEmailAsync(studentEmail, "Solicitud TFG rechazada", body);
        }

        public async Task ChangeStatus(int tfgId)
        {
            TFGModel? model = context.tfg
                .Include(d => d.tfgLineModel)
                .Include(d => d.Professors).ThenInclude(d => d.professorModel)
                .Include(d => d.Students).ThenInclude(d => d.studentModel)
                .Where(d => d.id == tfgId)
                .FirstOrDefault();

            if (model == null)
            {
                throw new NotFoundException("TFG not found");
            }
            if (model.status == (int)TFGStatus.Pending || model.status == (int)TFGStatus.Finished)
            {
                throw new UnprocessableException("TFG status can't be changed from this status");
            }
            model.status = model.status + 1; // Increment the status;
            context.SaveChanges();

            if (emailService == null)
            {
                throw new UnprocessableException("Email service not available");
            }
            if (configuration == null)
            {
                throw new UnprocessableException("Configuration not available");
            }
            var body = string.Empty;
            var subject = string.Empty;
            switch (model.status)
            {
                case (int)TFGStatus.PreliminaryAproved:
                    body = $"Tu anteproyecto para tu TFG ha sido aprobado.\n\n" +
                        $"Puedes ver la solicitud en el portal de gestión de TFGs: {configuration.GetSection("app:url").Value}.\n\n";
                    subject = "Anteproyecto TFG aprobado";
                    break;
                case (int)TFGStatus.Finished:
                    body = $"Tu TFG ha sido presentado ante tribunal y finalizado.\n\n" +
                        $"Puedes ver la solicitud en el portal de gestión de TFGs: {configuration.GetSection("app:url").Value}.\n\n";
                    break;
                default:
                    throw new UnprocessableException("Invalid TFG status");
            }
            // Send email to the student
            await emailService.SendEmailAsync(model.Students.FirstOrDefault()?.studentModel.email, subject, body);
        }

        #region Private Methods
        #endregion
    }
}
