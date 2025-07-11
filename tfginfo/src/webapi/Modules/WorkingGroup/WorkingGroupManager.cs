using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Models;
using TFGinfo.Objects;

namespace TFGinfo.Api
{
    public class WorkingGroupManager : BaseManager
    {
        public EmailService? emailService { get; set; }
        public WorkingGroupManager(ApplicationDbContext context, EmailService? emailService = null) : base(context)
        {
            this.emailService = emailService;
        }

        public List<WorkingGroupBase> GetAllWorkingGroups()
        {
            return context.working_group.ToList().ConvertAll(model => new WorkingGroupBase(model));
        }

        public WorkingGroupBase CreateWorkingGroup(WorkingGroupBase WorkingGroup, List<int>? professors = null, List<int>? students = null, List<int>? TFGs = null)
        {

            WorkingGroupModel model = new WorkingGroupModel
            {
                name = WorkingGroup.name,
                description = WorkingGroup.description,
                isPrivate = WorkingGroup.isPrivate ? 1 : 0
            };
            context.working_group.Add(model);
            context.SaveChanges();

            model.Professors = new List<WorkingGroupProfessorModel>();
            if (professors != null)
            {
                foreach (int professorId in professors)
                {
                    WorkingGroupProfessorModel workingGroupProfessor = new WorkingGroupProfessorModel
                    {
                        working_group = model.id,
                        professor = professorId
                    };
                    model.Professors.Add(workingGroupProfessor);
                }
            }
            model.Students = new List<WorkingGroupStudentModel>();
            if (students != null)
            {
                foreach (int studentId in students)
                {
                    WorkingGroupStudentModel workingGroupStudent = new WorkingGroupStudentModel
                    {
                        working_group = model.id,
                        student = studentId
                    };
                    model.Students.Add(workingGroupStudent);
                }
            }
            model.TFGs = new List<WorkingGroupTFGModel>();
            if (TFGs != null)
            {
                foreach (int TFGId in TFGs)
                {
                    WorkingGroupTFGModel workingGroupTFG = new WorkingGroupTFGModel
                    {
                        working_group = model.id,
                        tfg = TFGId
                    };
                    model.TFGs.Add(workingGroupTFG);
                }
            }
            context.Update(model);
            context.SaveChanges();

            return new WorkingGroupBase(model);
        }

        public void AddStudentToWorkingGroup(int id, int studentId)
        {
            WorkingGroupModel? model = context.working_group.FirstOrDefault(WorkingGroup => WorkingGroup.id == id);
            if (model == null)
            {
                throw new NotFoundException();
            }
            WorkingGroupStudentModel workingGroupStudent = new WorkingGroupStudentModel
            {
                working_group = model.id,
                student = studentId
            };
            context.working_group_student.Add(workingGroupStudent);
            context.SaveChanges();
        }

        public StudentDTO AddStudentToWorkingGroupByEmail(int id, string email)
        {
            WorkingGroupModel? model = context.working_group.FirstOrDefault(WorkingGroup => WorkingGroup.id == id);
            if (model == null)
            {
                throw new NotFoundException("Working group not found");
            }
            StudentModel? student = context.student.FirstOrDefault(Student => Student.email == email);
            if (student == null)
            {
                throw new NotFoundException("Student not found");
            }
            WorkingGroupStudentModel? existingWorkingGroupStudent = context.working_group_student.FirstOrDefault(WorkingGroup => WorkingGroup.working_group == model.id && WorkingGroup.student == student.id);
            if (existingWorkingGroupStudent != null)
            {
                throw new UnprocessableException("Student already in working group");
            }
            WorkingGroupStudentModel workingGroupStudent = new WorkingGroupStudentModel
            {
                working_group = model.id,
                student = student.id
            };
            context.working_group_student.Add(workingGroupStudent);
            context.SaveChanges();
            return new StudentDTO(student);
        }

        public void RemoveStudentFromWorkingGroup(int id, int studentId)
        {
            WorkingGroupModel? model = context.working_group.FirstOrDefault(WorkingGroup => WorkingGroup.id == id);
            if (model == null)
            {
                throw new NotFoundException();
            }
            WorkingGroupStudentModel? workingGroupStudent = context.working_group_student.FirstOrDefault(WorkingGroup => WorkingGroup.working_group == model.id && WorkingGroup.student == studentId);
            if (workingGroupStudent == null)
            {
                throw new NotFoundException();
            }
            context.working_group_student.Remove(workingGroupStudent);
            context.SaveChanges();
        }

        public void AddProfessorToWorkingGroup(int id, int professorId)
        {
            WorkingGroupModel? model = context.working_group.FirstOrDefault(WorkingGroup => WorkingGroup.id == id);
            if (model == null)
            {
                throw new NotFoundException("Working group not found");
            }
            ProfessorModel? professor = context.professor.FirstOrDefault(Professor => Professor.id == professorId);
            if (professor == null)
            {
                throw new NotFoundException("Professor not found");
            }
            WorkingGroupProfessorModel workingGroupProfessor = new WorkingGroupProfessorModel
            {
                working_group = model.id,
                professor = professorId
            };
            context.working_group_professor.Add(workingGroupProfessor);
            context.SaveChanges();
        }

        public void RemoveProfessorFromWorkingGroup(int id, int professorId)
        {
            WorkingGroupModel? model = context.working_group.FirstOrDefault(WorkingGroup => WorkingGroup.id == id);
            if (model == null)
            {
                throw new NotFoundException();
            }
            List<WorkingGroupProfessorModel> professors = context.working_group_professor.Where(WorkingGroup => WorkingGroup.working_group == model.id).ToList();
            WorkingGroupProfessorModel? workingGroupProfessor = professors.FirstOrDefault(WorkingGroup => WorkingGroup.professor == professorId);
            if (workingGroupProfessor == null)
            {
                throw new NotFoundException();
            }
            if (professors.Count <= 1)
            {
                throw new UnprocessableException("Cannot remove last professor from working group");
            }
            context.working_group_professor.Remove(workingGroupProfessor);
            context.SaveChanges();
        }

        public void DeleteWorkingGroup(int id)
        {
            WorkingGroupModel? model = context.working_group.FirstOrDefault(WorkingGroup => WorkingGroup.id == id);
            if (model == null)
            {
                throw new NotFoundException();
            }
            List<WorkingGroupStudentModel> students = context.working_group_student.Where(WorkingGroup => WorkingGroup.working_group == id).ToList();
            foreach (WorkingGroupStudentModel student in students)
            {
                context.working_group_student.Remove(student);
            }
            List<WorkingGroupProfessorModel> professors = context.working_group_professor.Where(WorkingGroup => WorkingGroup.working_group == id).ToList();
            foreach (WorkingGroupProfessorModel professor in professors)
            {
                context.working_group_professor.Remove(professor);
            }
            List<WorkingGroupTFGModel> TFGs = context.working_group_tfg.Where(WorkingGroup => WorkingGroup.working_group == id).ToList();
            foreach (WorkingGroupTFGModel TFG in TFGs)
            {
                context.working_group_tfg.Remove(TFG);
            }
            context.working_group.Remove(model);
            context.SaveChanges();
        }

        public WorkingGroupBase UpdateWorkingGroup(WorkingGroupBase WorkingGroup)
        {
            WorkingGroupModel? model = context.working_group.FirstOrDefault(d => d.id == WorkingGroup.id);
            if (model == null)
            {
                throw new NotFoundException();
            }
            model.description = WorkingGroup.description;
            model.name = WorkingGroup.name;
            model.isPrivate = WorkingGroup.isPrivate ? 1 : 0;
            context.SaveChanges();

            return new WorkingGroupBase(model);
        }

        public WorkingGroupBase GetWorkingGroup(int id)
        {
            WorkingGroupModel? model = context.working_group.FirstOrDefault(WorkingGroup => WorkingGroup.id == id);
            if (model == null)
            {
                throw new NotFoundException();
            }
            return new WorkingGroupBase(model);
        }

        public List<ProfessorDTO> GetProfessorsByWorkingGroup(int id)
        {
            List<ProfessorModel> professors = context.working_group_professor.Where(WorkingGroup => WorkingGroup.working_group == id).Select(p => p.professorModel).ToList();
            return professors.ConvertAll(model => new ProfessorDTO(model));
        }

        public List<StudentDTO> GetStudentsByWorkingGroup(int id)
        {
            List<StudentModel> students = context.working_group_student.Where(WorkingGroup => WorkingGroup.working_group == id).Select(p => p.studentModel).ToList();
            return students.ConvertAll(model => new StudentDTO(model));
        }

        public List<TFGDTO> GetTFGsByWorkingGroup(int id)
        {
            List<TFGModel> TFGs = context.working_group_tfg.Where(WorkingGroup => WorkingGroup.id == id).Select(p => p.tfgModel).ToList();
            return TFGs.ConvertAll(model => new TFGDTO(model));
        }

        public List<WorkingGroupBase> GetWorkingGroupsByProfessor(int id)
        {
            List<WorkingGroupModel> workingGroups = context.working_group_professor.Where(WorkingGroup => WorkingGroup.professor == id).Select(p => p.workingGroupModel).ToList();
            return workingGroups.ConvertAll(model => new WorkingGroupBase(model));
        }

        public List<WorkingGroupBase> GetWorkingGroupsByStudent(int id)
        {
            List<WorkingGroupModel> workingGroups = context.working_group_student.Where(WorkingGroup => WorkingGroup.student == id).Select(p => p.workingGroupModel).ToList();
            return workingGroups.ConvertAll(model => new WorkingGroupBase(model));
        }

        public async Task SendMessage(int id, int professorId, string message)
        {
            WorkingGroupModel? model = context.working_group.FirstOrDefault(WorkingGroup => WorkingGroup.id == id);
            if (model == null)
            {
                throw new NotFoundException("Working group not found");
            }
            ProfessorModel? professor = context.professor.FirstOrDefault(Professor => Professor.id == professorId);
            if (professor == null)
            {
                throw new NotFoundException("Professor not found");
            }
            if (emailService == null)
            {
                throw new NotFoundException("Email service not found");
            }
            string subject = $"Notificación del grupo {model.name}";
            string body = $"El profesor {professor.name} {professor.surname} ({professor.email}) ha enviado el siguiente mensaje al grupo de trabajo {model.name}:\n\n{message}\n\n";

            List<StudentModel> student = context.working_group_student.Where(WorkingGroup => WorkingGroup.working_group == id).Select(p => p.studentModel).ToList();
            foreach (StudentModel studentModel in student)
            {
                await emailService.SendEmailAsync(studentModel.email, subject, body);
            }

            List<ProfessorModel> professors = context.working_group_professor.Where(WorkingGroup => WorkingGroup.working_group == id && WorkingGroup.professorModel.id != professorId).Select(p => p.professorModel).ToList();
            foreach (ProfessorModel professorModel in professors)
            {
                await emailService.SendEmailAsync(professorModel.email, subject, body);
            }
        }
        
        public List<WorkingGroupBase> SearchWorkingGroups(List<Filter> filters)
        {
            IQueryable<WorkingGroupModel> query = context.working_group.Include(wg => wg.Professors).ThenInclude(wg => wg.professorModel).ThenInclude(p => p.departmentModel).ThenInclude(d => d.Universities).AsQueryable();

            foreach (var filter in filters)
            {
                if (filter.key == "name")
                {
                    query = query.Where(wg => wg.name.Contains(filter.value));
                }
                else if (filter.key == "description")
                {
                    query = query.Where(wg => wg.description.Contains(filter.value));
                }
                else if (filter.key == "isPrivate" && filter.value != "-1")
                {
                    query = query.Where(wg => wg.isPrivate == int.Parse(filter.value));
                }
                else if (filter.key == "university")
                {
                    query = query.Where(wg => wg.Professors.First().professorModel.departmentModel.Universities.Any(u => u.university == int.Parse(filter.value)));
                }
                else if (filter.key == "universities")
                {
                    var universities = filter.value.Split(',').Select(int.Parse).ToList();
                    query = query.Where(wg => wg.Professors.First().professorModel.departmentModel.Universities.Any(u => universities.Contains(u.university)));
                }
                else if (filter.key == "department")
                {
                    query = query.Where(wg => wg.Professors.First().professorModel.department == int.Parse(filter.value));
                }
                else if (filter.key == "professor")
                {
                    query = query.Where(wg => wg.Professors.Any(p => p.professor == int.Parse(filter.value)));
                }
                else if (filter.key == "student")
                {
                    query = query.Where(wg => wg.Students.Any(s => s.student == int.Parse(filter.value)));
                }
                else if (filter.key == "tfg")
                {
                    query = query.Where(wg => wg.TFGs.Any(t => t.tfg == int.Parse(filter.value)));
                }
                else if (filter.key == "generic")
                {
                    query = query.Where(wg => wg.name.Contains(filter.value) || wg.description.Contains(filter.value));
                }
            }

            return query.ToList().ConvertAll(model => new WorkingGroupBase(model));
        }


        #region Private Methods
        #endregion
    }
}
