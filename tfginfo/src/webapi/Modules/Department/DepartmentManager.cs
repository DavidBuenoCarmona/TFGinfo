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
                throw new UnprocessableException("AT_LEAST_ONE_UNIVERSITY_REQUIRED");
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
            bool professorExists = context.professor.Any(professor => professor.department == id);
            if (professorExists)
            {
                throw new UnprocessableException("CANNOT_DELETE_DEPARTMENT_WITH_PROFESSORS");
            }
            bool tfgLineExists = context.tfg_line.Any(tfgLine => tfgLine.department == id);
            if (tfgLineExists)
            {
                throw new UnprocessableException("CANNOT_DELETE_DEPARTMENT_WITH_TFG");
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
                throw new UnprocessableException("AT_LEAST_ONE_UNIVERSITY_REQUIRED");
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

        public CSVOutput ImportDepartments(string base64)
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

                if (fields.Length < 3)
                {
                    output.errorItems.Add($"Line {i + 1}: Invalid format, expected at least 2 fields.");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(fields[0]))
                {
                    output.errorItems.Add($"Line {i + 1}: Name cannot be empty.");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(fields[2]))
                {
                    output.errorItems.Add($"Line {i + 1}: Center list cannot be empty.");
                    continue;
                }

                var acronym = string.IsNullOrWhiteSpace(fields[1]) ? "" : fields[1].Trim();
                var universityNames = fields[2].Split(',').Select(u => u.Trim().ToLower()).ToList();
                var universityIds = context.university
                    .Where(u => universityNames.Contains(u.name.ToLower() ) || (u.acronym != null && universityNames.Contains(u.acronym.ToLower())))
                    .Select(u => u.id)
                    .ToList();
                if (universityIds.Count == 0)
                {
                    output.errorItems.Add($"Line {i + 1}: No valid universities found for '{fields[2]}'.");
                    continue;
                }

                foreach (var universityName in universityNames)
                {
                    var university = context.university.FirstOrDefault(u => u.name.ToLower() == universityName || (u.acronym != null && u.acronym.ToLower() == universityName));
                    if (university == null)
                    {
                        output.errorItems.Add($"Line {i + 1}: University '{universityName}' does not exist. This relation will be ignored.");
                    }
                }

                var department = new DepartmentFlatDTO
                {
                    name = fields[0].Trim(),
                    acronym = acronym,
                    universitiesId = universityIds
                };

                try
                {
                    CreateDepartment(department);
                    output.success++;
                }
                catch (Exception e)
                {
                    output.errorItems.Add($"Line {i + 1}: {e.Message}");
                }

            }

            return output;
        }

        #region Private Methods
        private void CheckNameIsNotRepeated(DepartmentFlatDTO department)
        {
            if (context.department.Any(d => d.name.ToLower() == department.name.ToLower() && d.id != department.id))
            {
                throw new UnprocessableException("DEPARTMENT_NAME_ALREADY_EXISTS");
            }
        }
        #endregion
    }
}
