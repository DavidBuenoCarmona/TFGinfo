using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Models;
using TFGinfo.Objects;

namespace TFGinfo.Api
{
    public class UniversityManager : BaseManager
    {
        public UniversityManager(ApplicationDbContext context) : base(context) {}
        public List<UniversityBase> GetAllUniversities()
        {
            return context.university.ToList().ConvertAll(model => new UniversityBase(model));
        }

        public UniversityBase CreateUniversity(UniversityBase university)
        { 
            CheckNameIsNotRepeated(university);

            UniversityModel model = new UniversityModel
            {
                name = university.name,
                address = university.address,
                acronym = university.acronym ?? string.Empty,
            };
            context.university.Add(model);
            context.SaveChanges();

            return new UniversityBase(model);
        }

        public void DeleteUniversity(int id)
        {
            UniversityModel? model = context.university.FirstOrDefault(university => university.id == id);
            if (model == null) {
                throw new NotFoundException();
            }

            // Check departments and careers
            if (context.department.Any(department => department.Universities.Any(u => u.id == id)))
            {
                throw new UnprocessableException("Cannot delete university with existing departments.");
            }
            if (context.career.Any(career => career.university == id))
            {
                throw new UnprocessableException("Cannot delete university with existing careers.");
            }

            context.university.Remove(model);
            context.SaveChanges();
        }

        public UniversityBase UpdateUniversity(UniversityBase university)
        {
            UniversityModel? model = context.university.FirstOrDefault(u => u.id == university.id);
            if (model == null) {
                throw new NotFoundException();
            }

            CheckNameIsNotRepeated(university);

            model.name = university.name;
            model.address = university.address;
            model.acronym = university.acronym ?? string.Empty;
            context.university.Update(model);
            context.SaveChanges();

            return new UniversityBase(model);
        }

        public UniversityBase GetUniversity(int id)
        {
            UniversityModel? model = context.university.FirstOrDefault(u => u.id == id);
            if (model == null) {
                throw new NotFoundException();
            }
            return new UniversityBase(model);
        }

        public List<UniversityBase> SearchUniversities(List<Filter> filters)
        {
            IQueryable<UniversityModel> query = context.university;
            foreach (var filter in filters)
            {
                if (filter.key == "name")
                {
                    query = query.Where(u => u.name.ToLower().Contains(filter.value.ToLower()));
                }
                else if (filter.key == "address")
                {
                    query = query.Where(u => u.address.ToLower().Contains(filter.value.ToLower()));
                }
                else if (filter.key == "generic")
                {
                    query = query.Where(u => u.name.ToLower().Contains(filter.value.ToLower()) || 
                                             u.address.ToLower().Contains(filter.value.ToLower()) ||
                                             u.acronym.ToLower().Contains(filter.value.ToLower()));
                } else if (filter.key == "acronym")
                {
                    query = query.Where(u => u.acronym.ToLower().Contains(filter.value.ToLower()));
                }
            } 
            return query.ToList().ConvertAll(model => new UniversityBase(model)); 
        }

        public CSVOutput ImportUniversities(string base64)
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
                if (fields.Length < 2)
                {
                    output.errorItems.Add($"Line {i + 1}: Invalid format, expected at least 2 fields.");
                    continue;
                }
                string name = fields[0].Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    output.errorItems.Add($"Line {i + 1}: Name cannot be empty.");
                    continue;
                }
                string acronym = fields[1].Trim();
                string address = fields.Length > 2 ? fields[2].Trim() : "";
                if (string.IsNullOrWhiteSpace(address))
                {
                    output.errorItems.Add($"Line {i + 1}: Address cannot be empty.");
                    continue;
                }
                var university = new UniversityBase
                {
                    name = name,
                    address = address,
                    acronym = acronym
                };

                try
                {
                    CreateUniversity(university);
                    output.success++;
                }
                catch (Exception e)
                {
                    output.errorItems.Add($"Line {i + 1}: {e.Message}");
                    continue;
                }
            }
            
            return output;
        }

        #region Private Methods
        private void CheckNameIsNotRepeated(UniversityBase university)
        {
            if (context.university.Any(u => u.id != university.id && u.name.ToLower() == university.name.ToLower()))
            {
                throw new UnprocessableException("University name already exists");
            }
            if (context.university.Any(u => u.id != university.id && u.acronym.ToLower() == university.acronym.ToLower()))
            {
                throw new UnprocessableException("University acronym already exists");
            }
        }

        #endregion


    }
}