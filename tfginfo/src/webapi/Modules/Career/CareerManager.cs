using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Models;
using TFGinfo.Objects;

namespace TFGinfo.Api
{
    public class CareerManager : BaseManager
    {
        public CareerManager(ApplicationDbContext context) : base(context) { }

        public List<CareerDTO> GetAllCareers()
        {
            return context.career.Include(d => d.universityModel).ToList().ConvertAll(model => new CareerDTO(model));
        }

        public CareerDTO CreateCareer(CareerFlatDTO Career)
        {
            CheckNameIsNotRepeated(Career);

            CareerModel model = new CareerModel
            {
                name = Career.name,
                university = Career.universityId
            };
            context.career.Add(model);
            context.SaveChanges();

            var savedCareer = context.career
                .Where(d => d.id == model.id)
                .Include(d => d.universityModel)
                .FirstOrDefault();

            return new CareerDTO(model);
        }

        public void DeleteCareer(int id)
        {
            CareerModel? model = context.career.FirstOrDefault(Career => Career.id == id);
            if (model == null)
            {
                throw new NotFoundException();
            }
            bool studentsExist = context.student.Any(s => s.career == id);
            if (studentsExist)
            {
                throw new UnprocessableException("Cannot delete career because there are students associated with it.");
            }
            bool tfgExists = context.tfg_line_career.Any(t => t.career == id);
            if (tfgExists)
            {
                throw new UnprocessableException("Cannot delete career because there are TFGs associated with it.");
            }

            context.career.Remove(model);
            context.SaveChanges();
        }

        public CareerDTO UpdateCareer(CareerFlatDTO Career)
        {
            CareerModel? model = context.career.Include(d => d.universityModel).FirstOrDefault(d => d.id == Career.id);
            if (model == null)
            {
                throw new NotFoundException();
            }

            CheckNameIsNotRepeated(Career);

            model.name = Career.name;
            model.university = Career.universityId;
            context.SaveChanges();

            return new CareerDTO(model);
        }

        public CareerDTO GetCareerById(int id)
        {
            CareerModel? model = context.career.Include(d => d.universityModel).FirstOrDefault(Career => Career.id == id);
            if (model == null)
            {
                throw new NotFoundException();
            }
            return new CareerDTO(model);
        }

        public List<CareerDTO> GetCareersByUniversity(int universityId)
        {
            return context.career.Where(Career => Career.university == universityId).Include(d => d.universityModel).ToList().ConvertAll(model => new CareerDTO(model));
        }

        public List<CareerDTO> SearchCareers(List<Filter> filters)
        {
            IQueryable<CareerModel> query = context.career.Include(d => d.universityModel);

            foreach (var filter in filters)
            {
                if (filter.key == "name")
                {
                    query = query.Where(c => c.name.ToLower().Contains(filter.value.ToLower()));
                }
                else if (filter.key == "university")
                {
                    query = query.Where(c => c.universityModel.name.ToLower().Contains(filter.value.ToLower()));
                }
                else if (filter.key == "universityId")
                {
                    query = query.Where(c => c.universityModel.id == int.Parse(filter.value));
                }
                else if (filter.key == "universities" && filter.value != "0")
                {
                    // Assuming 'universities' is a comma-separated list of university IDs
                    var universityIds = filter.value.Split(',').Select(int.Parse).ToList();
                    query = query.Where(c => universityIds.Contains(c.universityModel.id));
                }
                else if (filter.key == "generic")
                {
                    string searchValue = filter.value.ToLower();
                    query = query.Where(c => c.name.ToLower().Contains(searchValue) || (c.universityModel != null && c.universityModel.name.ToLower().Contains(searchValue)));
                }
            }

            return query.ToList().ConvertAll(model => new CareerDTO(model));
        }

        public CSVOutput ImportCareers(string base64)
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
                    output.errorItems.Add($"Line {i + 1}: Invalid number of fields. Expected at least 2.");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(fields[0].Trim()))
                {
                    output.errorItems.Add($"Line {i + 1}: Career name cannot be empty.");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(fields[1].Trim()))
                {
                    output.errorItems.Add($"Line {i + 1}: Center name cannot be empty.");
                    continue;
                }

                var university = context.university.Where(u => u.name.ToLower() == fields[1].Trim().ToLower()).Select(u => u.id).FirstOrDefault();
                if (university == 0)
                {
                    output.errorItems.Add($"Line {i + 1}: University '{fields[1].Trim()}' does not exist.");
                    continue;
                }

                var career = new CareerFlatDTO
                {
                    name = fields[0].Trim(),
                    universityId = university
                };

                try
                {
                    CreateCareer(career);
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
        private void CheckNameIsNotRepeated(CareerFlatDTO career)
        {
            if (context.career.Any(c => c.id != career.id && c.name.ToLower() == career.name.ToLower() && c.university == career.universityId))
            {
                throw new UnprocessableException("Career name already exists in this university.");
            }
        }
        #endregion
    }
}
