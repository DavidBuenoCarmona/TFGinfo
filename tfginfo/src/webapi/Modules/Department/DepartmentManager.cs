using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Models;
using TFGinfo.Objects;

namespace TFGinfo.Api
{
    public class DepartmentManager : BaseManager
    {
        public DepartmentManager(ApplicationDbContext context) : base(context) {}

        public List<DepartmentDTO> GetAllDepartments()
        {
            return context.department.Include(d => d.universityModel).ToList().ConvertAll(model => new DepartmentDTO(model));
        }

        public DepartmentDTO CreateDepartment(DepartmentFlatDTO department)
        { 
            CheckNameIsNotRepeated(department.name);

            DepartmentModel model = new DepartmentModel {
                name = department.name,
                university = department.universityId
            };
            context.department.Add(model);
            context.SaveChanges();

            var savedDepartment = context.department
                .Where(d => d.id == model.id)
                .Include(d => d.universityModel)
                .FirstOrDefault();

            return new DepartmentDTO(model);
        }

        public void DeleteDepartment(int id)
        {
            DepartmentModel? model = context.department.FirstOrDefault(department => department.id == id);
            if (model == null) {
                throw new NotFoundException();
            }
            context.department.Remove(model);
            context.SaveChanges();
        }

        public DepartmentDTO UpdateDepartment(DepartmentFlatDTO department)
        {
            DepartmentModel? model = context.department.Include(d => d.universityModel).FirstOrDefault(d => d.id == department.id);
            if (model == null) {
                throw new NotFoundException();
            }

            CheckNameIsNotRepeated(department.name);

            model.name = department.name;
            model.university = department.universityId;
            context.SaveChanges();

            return new DepartmentDTO(model);
        }

        public List<DepartmentDTO> GetDepartmentsByUniversity(int universityId)
        {
            return context.department.Where(department => department.university == universityId).Include(d => d.universityModel).ToList().ConvertAll(model => new DepartmentDTO(model));
        }

        public DepartmentDTO GetDepartment(int id)
        {
            DepartmentModel? model = context.department.Include(d => d.universityModel).FirstOrDefault(department => department.id == id);
            if (model == null) {
                throw new NotFoundException();
            }
            return new DepartmentDTO(model);
        }

        #region Private Methods
        private void CheckNameIsNotRepeated(string name)
        {
            if (context.department.Any(department => department.name.ToLower() == name.ToLower())) {
                throw new UnprocessableException("Department name already exists");
            }
        }
        #endregion
    }
}
