using System.Collections.Generic;
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
        public ProfessorManager(ApplicationDbContext context) : base(context) {}

        public List<ProfessorDTO> GetAllProfessors()
        {
            return context.professor.AsNoTracking().Include(d => d.departmentModel).ToList().ConvertAll(model => new ProfessorDTO(model));
        }

        public ProfessorDTO CreateProfessor(ProfessorFlatDTO Professor)
        { 
            CheckEmailIsNotRepeated(Professor);

            UserManager userManager = new UserManager(context);
            int userId = userManager.CreateUser(new UserFlatDTO {
                username = Professor.email,
                roleId = (int)RoleTypes.Professor
            });

            ProfessorModel model = new ProfessorModel {
                name = Professor.name,
                department = Professor.departmentId,
                surname = Professor.surname,
                email = Professor.email,
                user = userId,
                department_boss = Professor.department_boss ? 1 : 0,

            };
            context.professor.Add(model);
            context.SaveChanges();

            var savedProfessor = context.professor
                .Where(d => d.id == model.id)
                .Include(d => d.departmentModel)
                .FirstOrDefault();

            return new ProfessorDTO(model);
        }

        public void DeleteProfessor(int id)
        {
            ProfessorModel? model = context.professor.FirstOrDefault(Professor => Professor.id == id);
            if (model == null) {
                throw new NotFoundException();
            }
            int userId = model.user;

            context.professor.Remove(model);
            context.SaveChanges();

            UserManager userManager = new UserManager(context);
            userManager.DeleteUser(userId);
        }

        public ProfessorDTO UpdateProfessor(ProfessorFlatDTO Professor)
        {
            ProfessorModel? model = context.professor.Include(d => d.departmentModel).FirstOrDefault(d => d.id == Professor.id);
            if (model == null) {
                throw new NotFoundException();
            }

            CheckEmailIsNotRepeated(Professor);

            model.department = Professor.departmentId;
            model.department_boss = Professor.department_boss ? 1 : 0;
            context.SaveChanges();

            return new ProfessorDTO(model);
        }

        public List<ProfessorDTO> GetAllByDepartment(int departmentId)
        {
            return context.professor.AsNoTracking().Where(Professor => Professor.department == departmentId).Include(d => d.departmentModel).ToList().ConvertAll(model => new ProfessorDTO(model));
        }

        public ProfessorDTO GetProfessorById(int id)
        {
            ProfessorModel? model = context.professor.Include(d => d.departmentModel).FirstOrDefault(d => d.id == id);
            if (model == null) {
                throw new NotFoundException();
            }
            return new ProfessorDTO(model);
        }

        #region Private Methods
        private void CheckEmailIsNotRepeated(ProfessorFlatDTO professor)
        {
            if (context.professor.Any(p => p.id != professor.id && p.email.ToLower() == professor.email.ToLower())) {
                throw new UnprocessableException("Professor email already exists");
            }
        }
        #endregion
    }
}
