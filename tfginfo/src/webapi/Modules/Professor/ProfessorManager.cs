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
    public class ProfessorManager : BaseManager
    {
        private readonly EmailService? emailService;
        public ProfessorManager(ApplicationDbContext context, EmailService? emailService = null) : base(context)
        {
            this.emailService = emailService;
        }

        public List<ProfessorDTO> GetAllProfessors()
        {
            return context.professor.AsNoTracking().Include(d => d.departmentModel).ToList().ConvertAll(model => new ProfessorDTO(model));
        }

        public async Task<NewProfessorDTO> CreateProfessor(ProfessorFlatDTO Professor)
        {
            CheckEmailIsNotRepeated(Professor);

            UserManager userManager = new UserManager(context, emailService);
            UserDTO user = await userManager.CreateUser(new UserFlatDTO
            {
                username = Professor.email,
                roleId = (int)RoleTypes.Professor
            });

            ProfessorModel model = new ProfessorModel
            {
                name = Professor.name,
                department = Professor.departmentId,
                surname = Professor.surname,
                email = Professor.email,
                user = user.id.Value,
                department_boss = Professor.department_boss ? 1 : 0,

            };
            context.professor.Add(model);
            context.SaveChanges();

            var savedProfessor = context.professor
                .Where(d => d.id == model.id)
                .Include(d => d.departmentModel)
                .FirstOrDefault();

            NewProfessorDTO newProfessorDTO = new NewProfessorDTO(new(savedProfessor), user.auth_code);

            return newProfessorDTO;
        }

        public void DeleteProfessor(int id)
        {
            ProfessorModel? model = context.professor.FirstOrDefault(Professor => Professor.id == id);
            if (model == null)
            {
                throw new NotFoundException();
            }
            int userId = model.user;

            bool groupsExist = context.working_group_professor.Any(g => g.professor == id);
            if (groupsExist)
            {
                throw new UnprocessableException("CANNOT_DELETE_PROFESSOR_WITH_GROUPS");
            }

            bool tfgsExist = context.tfg_professor.Any(t => t.professor == id);
            if (tfgsExist)
            {
                throw new UnprocessableException("CANNOT_DELETE_PROFESSOR_WITH_TFG");
            }

            bool tfgLinesExist = context.tfg_line_professor.Any(t => t.professor == id);
            if (tfgLinesExist)
            {
                throw new UnprocessableException("CANNOT_DELETE_PROFESSOR_WITH_TFG_LINES");
            }

            context.professor.Remove(model);
            context.SaveChanges();

            UserManager userManager = new UserManager(context);
            userManager.DeleteUser(userId);
        }

        public ProfessorDTO UpdateProfessor(ProfessorFlatDTO Professor)
        {
            ProfessorModel? model = context.professor.Include(d => d.departmentModel).FirstOrDefault(d => d.id == Professor.id);
            if (model == null)
            {
                throw new NotFoundException();
            }

            CheckEmailIsNotRepeated(Professor);

            model.department = Professor.departmentId;
            model.name = Professor.name;
            model.surname = Professor.surname;
            model.email = Professor.email;
            model.department_boss = Professor.department_boss ? 1 : 0;
            context.SaveChanges();

            UserManager userManager = new UserManager(context);
            UserDTO user = userManager.GetUser(model.user);
            if (user.username != Professor.email)
            {
                user.username = Professor.email;
                userManager.ChangeEmail(user.id.Value, user.username);
            }

            return new ProfessorDTO(model);
        }

        public List<ProfessorDTO> GetAllByDepartment(int departmentId)
        {
            return context.professor.AsNoTracking().Where(Professor => Professor.department == departmentId).Include(d => d.departmentModel).ToList().ConvertAll(model => new ProfessorDTO(model));
        }

        public ProfessorDTO GetProfessorById(int id)
        {
            ProfessorModel? model = context.professor.Include(d => d.departmentModel).FirstOrDefault(d => d.id == id);
            if (model == null)
            {
                throw new NotFoundException();
            }
            return new ProfessorDTO(model);
        }

        public List<ProfessorDTO> SearchProfessors(List<Filter> filters)
        {
            IQueryable<ProfessorModel> query = context.professor.AsNoTracking().Include(d => d.departmentModel).ThenInclude(d => d.Universities).ThenInclude(u => u.universityModel);

            foreach (var filter in filters)
            {
                if (filter.key == "name")
                {
                    query = query.Where(p => p.name.ToLower().Contains(filter.value.ToLower()));
                }
                else if (filter.key == "surname")
                {
                    query = query.Where(p => p.surname.ToLower().Contains(filter.value.ToLower()));
                }
                else if (filter.key == "email")
                {
                    query = query.Where(p => p.email.ToLower().Contains(filter.value.ToLower()));
                }
                else if (filter.key == "department")
                {
                    query = query.Where(p => p.departmentModel.name.ToLower().Contains(filter.value.ToLower()));
                }
                else if (filter.key == "university")
                {
                    query = query.Where(p => p.departmentModel.Universities.Any(u => u.university == int.Parse(filter.value)));
                }
                else if (filter.key == "universityId" && filter.value != "0")
                {
                    query = query.Where(p => p.departmentModel.Universities.Any(u => u.university == int.Parse(filter.value)));
                }
                else if (filter.key == "universities" && filter.value != "0")
                {
                    // Assuming 'universities' is a comma-separated list of university IDs
                    var universityIds = filter.value.Split(',').Select(int.Parse).ToList();
                    query = query.Where(p => p.departmentModel.Universities.Any(u => universityIds.Contains(u.university)));
                }
                else if (filter.key == "generic")
                {
                    string lowerValue = filter.value.ToLower();
                    query = query.Where(p => p.name.ToLower().Contains(lowerValue) ||
                                             p.surname.ToLower().Contains(lowerValue) ||
                                             p.email.ToLower().Contains(lowerValue) ||
                                             p.departmentModel.name.ToLower().Contains(lowerValue));
                }
            }

            return query.ToList().ConvertAll(model => new ProfessorDTO(model));
        }

        public async Task<CSVOutput> ImportProfessors(string base64)
        {
            var output = new CSVOutput();

            // Decodifica el base64 a texto
            var bytes = Convert.FromBase64String(base64);
            var csvContent = System.Text.Encoding.UTF8.GetString(bytes);

            // Separa líneas y elimina vacías
            var lines = csvContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Opcional: Si la primera línea es cabecera, sáltala
            // var startIndex = lines[0].StartsWith("Nombre") ? 1 : 0;

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                // Divide por ';'
                var fields = line.Split(';');
                if (fields.Length < 4)
                {
                    output.errorItems.Add($"Error in line {i + 1}: Not enough fields.");
                    continue;
                }
                var name = fields[0].Trim();
                var surname = fields[1].Trim();
                var email = fields[2].Trim();
                var departmentName = fields[3].Trim();
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(departmentName))
                {
                    output.errorItems.Add($"Error in line {i + 1}: Missing required fields.");
                    continue;
                }
                var department = context.department.Where(d => d.name.ToLower() == departmentName.ToLower() || d.acronym.ToLower() == departmentName.ToLower()).Select(d => d.id).FirstOrDefault();
                if (department == 0)
                {
                    output.errorItems.Add($"Error in line {i + 1}: Department '{departmentName}' not found.");
                    continue;
                }
                var professor = new ProfessorFlatDTO
                {
                    name = name,
                    surname = surname,
                    email = email,
                    departmentId = department,
                    department_boss = false // Default value, can be adjusted later
                };

                try
                {
                    await CreateProfessor(professor);
                    output.success++;
                }
                catch (Exception ex)
                {
                    output.errorItems.Add($"Line {i + 1}: {ex.Message}");
                    continue;
                }

            }
            return output;
        }

        #region Private Methods
        private void CheckEmailIsNotRepeated(ProfessorFlatDTO professor)
        {
            if (context.professor.Any(p => p.id != professor.id && p.email.ToLower() == professor.email.ToLower()))
            {
                throw new UnprocessableException("PROFESSOR_EMAIL_ALREADY_EXISTS");
            }
        }
        #endregion
    }
}
