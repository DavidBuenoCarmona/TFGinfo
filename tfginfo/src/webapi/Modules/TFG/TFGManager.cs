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
        public TFGManager(ApplicationDbContext context, EmailService? emailService = null, IConfiguration? configuration = null) : base(context) {
            this.emailService = emailService;
            this.configuration = configuration;
        }

        public List<TFGDTO> GetAllTFGs()
        {
            return context.tfg.Include(d => d.tfgLineModel).ToList().ConvertAll(model => new TFGDTO(model));
        }

        public TFGDTO CreateTFG(TFGFlatDTO TFG)
        { 

            TFGModel model = new TFGModel {
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
            if (model == null) {
                throw new NotFoundException();
            }
            context.tfg.Remove(model);
            context.SaveChanges();
        }

        public TFGDTO UpdateTFG(TFGFlatDTO TFG)
        {
            TFGModel? model = context.tfg.Include(d => d.tfgLineModel).FirstOrDefault(d => d.id == TFG.id);
            if (model == null) {
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
            if (model == null) {
                throw new NotFoundException();
            }
            return new TFGDTO(model);
        }

        public List<TFGDTO> SearchTFGs(List<Filter> filters)
        {
            var query = context.tfg.Include(d => d.tfgLineModel).ThenInclude(d => d.departmentModel).AsQueryable();

            foreach (var filter in filters)
            {
                if (filter.key == "university")
                {
                    query = query.Where(tfg => tfg.tfgLineModel.departmentModel.university == int.Parse(filter.value));
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
                accepted = false,
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
            await emailService.SendEmailAsync(professor.email, "Solicitud TFG", 
            $"Has recibido una solicitud de TFG de {request.studentEmail} para la línea {tfg.name}.\n\n" +
            $"Puedes ver la solicitud en el portal de gestión de TFGs: {configuration.GetSection("app:url").Value}.\n\n");

            context.tfg.Update(tfgModel);
            context.SaveChanges();

            WorkingGroupManager wgManager = new WorkingGroupManager(context);
            WorkingGroupBase wg = new WorkingGroupBase
            {
                name = tfg.name,
                description = tfg.description,
                isPrivate = true
            };
            List<int> students = new List<int> { studentId };
            List<int> professors = new List<int> { request.professorId, request.secondaryProfessorId ?? 0 };
            List<int> tfgs = new List<int> { tfgModel.id };
            wgManager.CreateWorkingGroup(wg, professors, students, tfgs);
        }

        #region Private Methods
        #endregion
    }
}
