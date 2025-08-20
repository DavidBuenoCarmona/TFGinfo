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
            return context.career.Include(d => d.universityModel).Include(d => d.DoubleCareer).ThenInclude(d => d.primaryCareerModel).Include(d => d.DoubleCareer).ThenInclude(d => d.secondaryCareerModel).ToList().ConvertAll(model => new CareerDTO(model));
        }

        public CareerDTO CreateCareer(CareerFlatDTO Career)
        {
            CheckNameIsNotRepeated(Career);

            CareerModel model = new CareerModel
            {
                name = Career.name,
                university = Career.universityId,
                double_career = Career.doubleCareer ? 1 : 0,
            };
            context.career.Add(model);

            if (Career.doubleCareer)
            {
                model.DoubleCareer = new DoubleCareerModel
                {
                    primary_career = Career.doubleCareers.Count > 0 ? Career.doubleCareers[0] : 0,
                    secondary_career = Career.doubleCareers.Count > 1 ? Career.doubleCareers[1] : 0
                };
            }
            context.career.Add(model);
            context.SaveChanges();

            var savedCareer = context.career.Include(d => d.universityModel).Include(d => d.DoubleCareer).ThenInclude(d => d.primaryCareerModel).Include(d => d.DoubleCareer).ThenInclude(d => d.secondaryCareerModel).FirstOrDefault(d => d.id == model.id);

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
            var doubleCareer = context.double_career.FirstOrDefault(dc => dc.career == id);
            if (doubleCareer != null)
            {
                context.double_career.Remove(doubleCareer);
            }

            context.career.Remove(model);
            context.SaveChanges();
        }

        public CareerDTO UpdateCareer(CareerFlatDTO Career)
        {
            CareerModel? model = context.career.Include(d => d.universityModel).Include(d => d.DoubleCareer).ThenInclude(d => d.primaryCareerModel).Include(d => d.DoubleCareer).ThenInclude(d => d.secondaryCareerModel).FirstOrDefault(d => d.id == Career.id);
            if (model == null)
            {
                throw new NotFoundException();
            }

            CheckNameIsNotRepeated(Career);

            model.name = Career.name;
            model.university = Career.universityId;
            model.double_career = Career.doubleCareer ? 1 : 0;
            if (model.DoubleCareer == null && Career.doubleCareer)
            {
                model.DoubleCareer = new DoubleCareerModel();
            }
            else if (!Career.doubleCareer)
            {
                model.DoubleCareer = null;
            }

            if (model.DoubleCareer != null)
            {
                model.DoubleCareer.primary_career = Career.doubleCareers.Count > 0 ? Career.doubleCareers[0] : 0;
                model.DoubleCareer.secondary_career = Career.doubleCareers.Count > 1 ? Career.doubleCareers[1] : 0;
            }

            model = context.career.Include(d => d.universityModel).Include(d => d.DoubleCareer).ThenInclude(d => d.primaryCareerModel).Include(d => d.DoubleCareer).ThenInclude(d => d.secondaryCareerModel).FirstOrDefault(d => d.id == Career.id);

            context.SaveChanges();

            return new CareerDTO(model!);
        }

        public CareerDTO GetCareerById(int id)
        {
            CareerModel? model = context.career.Include(d => d.universityModel).Include(d => d.DoubleCareer).ThenInclude(d => d.primaryCareerModel).Include(d => d.DoubleCareer).ThenInclude(d => d.secondaryCareerModel).FirstOrDefault(Career => Career.id == id);
            if (model == null)
            {
                throw new NotFoundException();
            }
            return new CareerDTO(model);
        }

        public List<CareerDTO> SearchCareers(List<Filter> filters)
        {
            IQueryable<CareerModel> query = context.career.Include(d => d.universityModel).Include(d => d.DoubleCareer).ThenInclude(d => d.primaryCareerModel).ThenInclude(c => c.universityModel).Include(d => d.DoubleCareer).ThenInclude(d => d.secondaryCareerModel).ThenInclude(c => c.universityModel);

            foreach (var filter in filters)
            {
                if (filter.key == "name")
                {
                    query = query.Where(c => c.name.ToLower().Contains(filter.value.ToLower()));
                }
                else if (filter.key == "university")
                {
                    query = query.Where(c => c.universityModel != null ? c.universityModel.name.ToLower().Contains(filter.value.ToLower()) :
                        (c.DoubleCareer.primaryCareerModel.universityModel!.name.ToLower().Contains(filter.value.ToLower()) || c.DoubleCareer.secondaryCareerModel.universityModel!.name.ToLower().Contains(filter.value.ToLower())));
                }
                else if (filter.key == "universityId")
                {
                    query = query.Where(c => c.universityModel != null ? c.universityModel.id == int.Parse(filter.value) :
                        (c.DoubleCareer.primaryCareerModel.university == int.Parse(filter.value) || c.DoubleCareer.secondaryCareerModel.university == int.Parse(filter.value)));
                }
                else if (filter.key == "universities" && filter.value != "0")
                {
                    // Assuming 'universities' is a comma-separated list of university IDs
                    var universityIds = filter.value.Split(',').Select(int.Parse).ToList();
                    query = query.Where(c => universityIds.Contains(c.universityModel != null ? c.universityModel.id : (c.DoubleCareer.primaryCareerModel.university ?? 0)) ||
                        universityIds.Contains(c.DoubleCareer.secondaryCareerModel != null ? c.DoubleCareer.secondaryCareerModel.university ?? 0 : 0));
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
                if (string.IsNullOrWhiteSpace(fields[1].Trim()) && string.IsNullOrWhiteSpace(fields[2].Trim()))
                {
                    output.errorItems.Add($"Line {i + 1}: Center or Careers must have value.");
                    continue;
                }

                var university = 0;
                if (!string.IsNullOrWhiteSpace(fields[1]))
                {
                    university = context.university.Where(u => u.name.ToLower() == fields[1].Trim().ToLower() || (u.acronym != null && u.acronym.ToLower() == fields[1].Trim().ToLower())).Select(u => u.id).FirstOrDefault();
                    if (university == 0)
                    {
                        output.errorItems.Add($"Line {i + 1}: University '{fields[1].Trim()}' does not exist.");
                        continue;
                    }
                }


                var doubleCareer = 0;
                var doubleCareers = new List<int>();
                if (!string.IsNullOrWhiteSpace(fields[2].Trim()))
                {
                    var careerNames = fields[2].Trim().Split(',').Select(c => c.Trim()).ToList();
                    if (careerNames.Count != 2)
                    {
                        output.errorItems.Add($"Line {i + 1}: Double career can only have 2 careers.");
                        continue;
                    }
                    doubleCareer = 1;
                    doubleCareers = context.career.Where(c => careerNames.Contains(c.name.Trim())).Select(c => c.id).ToList();
                    if (doubleCareers.Count != 2)
                    {
                        output.errorItems.Add($"Line {i + 1}: One or both careers '{string.Join(", ", careerNames)}' do not exist.");
                        continue;
                    }
                }


                var career = new CareerFlatDTO
                {
                    name = fields[0].Trim(),
                    universityId = university > 0 ? university : null,
                    doubleCareer = doubleCareer == 1,
                    doubleCareers = doubleCareers
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
