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
        public DepartmentManager(ApplicationDbContext context) : base(context) { }

        public List<DepartmentDTO> GetAllDepartments()
        {
            return context.department.Include(d => d.Universities).ThenInclude(u => u.universityModel).ToList().ConvertAll(model => new DepartmentDTO(model));
        }

        public DepartmentDTO CreateDepartment(DepartmentFlatDTO department)
        {
            CheckNameIsNotRepeated(department);

            DepartmentModel model = new DepartmentModel
            {
                name = department.name,
                acronym = department.acronym,
            };
            if (department.universitiesId != null && department.universitiesId.Count > 0)
            {
                context.department.Add(model);
                context.SaveChanges();

                context.university_department.AddRange(
                    department.universitiesId.Select(id => new UniversityDepartmentModel { university = id, department = model.id })
                );
                context.SaveChanges();
            }
            else
            {
                throw new UnprocessableException("At least one university must be associated with the department.");
            }


            model = context.department
            .Include(d => d.Universities)
                .ThenInclude(ud => ud.universityModel)
                .FirstOrDefault(d => d.id == model.id)!;

            return new DepartmentDTO(model);
        }

        public void DeleteDepartment(int id)
        {
            DepartmentModel? model = context.department.FirstOrDefault(department => department.id == id);
            if (model == null)
            {
                throw new NotFoundException();
            }
            context.university_department.RemoveRange(context.university_department.Where(ud => ud.department == id));
            context.department.Remove(model);
            context.SaveChanges();
        }

        public DepartmentDTO UpdateDepartment(DepartmentFlatDTO department)
        {
            DepartmentModel? model = context.department.Include(d => d.Universities).ThenInclude(d => d.universityModel).FirstOrDefault(d => d.id == department.id);
            if (model == null)
            {
                throw new NotFoundException();
            }

            CheckNameIsNotRepeated(department);

            model.name = department.name;
            model.acronym = department.acronym;
            model.Universities.Clear();
            if (department.universitiesId != null && department.universitiesId.Count > 0)
            {
                model.Universities = department.universitiesId.Select(id => new UniversityDepartmentModel { university = id, department = department.id!.Value }).ToList();
            }
            else
            {
                throw new UnprocessableException("At least one university must be associated with the department.");
            }


            context.Update(model);
            context.SaveChanges();

            model = context.department
            .Include(d => d.Universities)
                .ThenInclude(ud => ud.universityModel)
                .FirstOrDefault(d => d.id == model.id);

            return new DepartmentDTO(model!);
        }


        public DepartmentDTO GetDepartment(int id)
        {
            DepartmentModel? model = context.department.Include(d => d.Universities).ThenInclude(u => u.universityModel).FirstOrDefault(department => department.id == id);
            if (model == null)
            {
                throw new NotFoundException();
            }
            return new DepartmentDTO(model);
        }

        public List<DepartmentDTO> SearchDepartments(List<Filter> filters)
        {
            IQueryable<DepartmentModel> query = context.department.Include(d => d.Universities).ThenInclude(u => u.universityModel);

            foreach (var filter in filters)
            {
                if (filter.key == "name")
                {
                    query = query.Where(c => c.name.ToLower().Contains(filter.value.ToLower()));
                }
                else if (filter.key == "acronym")
                {
                    query = query.Where(c => c.acronym.ToLower().Contains(filter.value.ToLower()));
                }
                else if (filter.key == "university")
                {
                    query = query.Where(c => c.Universities.Any(u => u.universityModel.name.ToLower().Contains(filter.value.ToLower())));
                }
                else if (filter.key == "universityId")
                {
                    query = query.Where(c => c.Universities.Any(u => u.university == int.Parse(filter.value)));
                }
                else if (filter.key == "universities" && filter.value != "0")
                {
                    // Assuming 'universities' is a comma-separated list of university IDs
                    var universityIds = filter.value.Split(',').Select(int.Parse).ToList();
                    query = query.Where(c => c.Universities.Any(u => universityIds.Contains(u.university)));
                }
                else if (filter.key == "generic")
                {
                    string searchValue = filter.value.ToLower();
                    query = query.Where(c => c.name.ToLower().Contains(searchValue) ||
                                             c.acronym.ToLower().Contains(searchValue) ||
                                             c.Universities.Any(u => u.universityModel.name.ToLower().Contains(searchValue)));
                }
            }

            return query.ToList().ConvertAll(model => new DepartmentDTO(model));
        }

        #region Private Methods
        private void CheckNameIsNotRepeated(DepartmentFlatDTO department)
        {
            if (context.department.Any(d => d.name.ToLower() == department.name.ToLower() && d.id != department.id))
            {
                throw new UnprocessableException("Department name already exists");
            }
        }
        #endregion
    }
}
