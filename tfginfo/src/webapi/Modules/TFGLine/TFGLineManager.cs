using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Models;
using TFGinfo.Objects;

namespace TFGinfo.Api
{
    public class TFGLineManager : BaseManager
    {
        public TFGLineManager(ApplicationDbContext context) : base(context) { }

        public List<TFGLineDTO> GetAllTFGLines()
        {
            return context.tfg_line.Include(d => d.departmentModel).ToList().ConvertAll(model => new TFGLineDTO(model));
        }

        public TFGLineDTO CreateTFGLine(TFGLineFlatDTO TFGLine)
        {
            CheckNameIsNotRepeated(TFGLine);

            TFGLineModel model = new TFGLineModel
            {
                name = TFGLine.name,
                description = TFGLine.description,
                slots = TFGLine.slots,
                group = TFGLine.group ? 1 : 0,
                department = TFGLine.departmentId
            };
            context.tfg_line.Add(model);
            context.SaveChanges();

            var savedTFGLine = context.tfg_line
                .Where(d => d.id == model.id)
                .Include(d => d.departmentModel)
                .FirstOrDefault();

            return new TFGLineDTO(model);
        }

        public void DeleteTFGLine(int id)
        {
            TFGLineModel? model = context.tfg_line.FirstOrDefault(TFGLine => TFGLine.id == id);
            if (model == null)
            {
                throw new NotFoundException();
            }
            bool tfgExists = context.tfg.Any(t => t.tfg_line == id);
            if (tfgExists)
            {
                throw new UnprocessableException("CANNOT_DELETE_TFG_LINE_WITH_TFG");
            }
            List<TFGLineCareerModel>? tfgLineCareers = context.tfg_line_career.Where(t => t.tfg_line == id).ToList(); ;
            if (tfgLineCareers != null)
            {
                context.tfg_line_career.RemoveRange(tfgLineCareers);
            }
            List<TFGLineProfessorModel>? tfgLineProfessors = context.tfg_line_professor.Where(t => t.tfg_line == id).ToList();
            if (tfgLineProfessors != null)
            {
                context.tfg_line_professor.RemoveRange(tfgLineProfessors);
            }

            context.tfg_line.Remove(model);
            context.SaveChanges();
        }

        public TFGLineDTO UpdateTFGLine(TFGLineFlatDTO TFGLine)
        {
            TFGLineModel? model = context.tfg_line.Include(d => d.departmentModel).FirstOrDefault(d => d.id == TFGLine.id);
            if (model == null)
            {
                throw new NotFoundException();
            }

            CheckNameIsNotRepeated(TFGLine);

            model.name = TFGLine.name;
            model.description = TFGLine.description;
            model.slots = TFGLine.slots;
            model.group = TFGLine.group ? 1 : 0;
            model.department = TFGLine.departmentId;
            context.SaveChanges();

            return new TFGLineDTO(model);
        }

        public TFGLineDTO GetTFGLine(int id)
        {
            TFGLineModel? model = context.tfg_line.
                Include(d => d.departmentModel).ThenInclude(d => d.Universities).ThenInclude(u => u.universityModel).
                Include(d => d.Careers).
                    ThenInclude(c => c.careerModel).
                Include(p => p.Professors).
                    ThenInclude(p => p.professorModel).FirstOrDefault(TFGLine => TFGLine.id == id);
            if (model == null)
            {
                throw new NotFoundException();
            }
            return new TFGLineDTO(model);
        }

        public List<TFGLineDTO> GetByStudentId(int studentId)
        {
            var query = context.tfg_line.Include(d => d.departmentModel).ThenInclude(d => d.Universities).ThenInclude(u => u.universityModel).Include(d => d.TFGs).ThenInclude(d => d.Students).AsQueryable();
            return query.Where(tfg => tfg.TFGs.Any(t => t.Students.Any(s => s.student == studentId))).ToList().ConvertAll(model => new TFGLineDTO(model));
        }

        public List<TFGLineDTO> SearchTFGLines(List<Filter> filters)
        {
            var query = context.tfg_line.
                Include(d => d.departmentModel).ThenInclude(d => d.Universities).ThenInclude(u => u.universityModel).Include(t => t.Careers).ThenInclude(t => t.careerModel).ThenInclude(c => c.DoubleCareer)
                .AsQueryable();

            foreach (var filter in filters)
            {
                if (filter.value == null || filter.value == "") continue;
                if (filter.key == "university")
                {
                    query = query.Where(tfg => tfg.departmentModel.Universities.Any(u => u.university == int.Parse(filter.value)));
                }
                if (filter.key == "description")
                {
                    query = query.Where(tfg => tfg.description.ToLower().Contains(filter.value.ToLower()));
                }
                else if (filter.key == "name")
                {
                    query = query.Where(tfg => tfg.name.ToLower().Contains(filter.value.ToLower()));
                }
                else if (filter.key == "slots")
                {
                    query = query.Where(tfg => tfg.slots >= int.Parse(filter.value));
                }
                else if (filter.key == "career")
                {
                    var DoubleCareer = context.double_career.Where(c => c.career == int.Parse(filter.value)).FirstOrDefault();
                    if (DoubleCareer == null)
                    {
                        query = query.Where(tfg => tfg.Careers.Any(c => c.careerModel.id == int.Parse(filter.value)));
                    }
                    else
                    {
                        // Si es una carrera doble, buscamos tanto la carrera primaria como la secundaria
                        query = query.Where(tfg => tfg.Careers.Any(c => c.career == DoubleCareer.primary_career || c.career == DoubleCareer.secondary_career));
                    }
                }
                else if (filter.key == "department")
                {
                    query = query.Where(tfg => tfg.department == int.Parse(filter.value));
                }
                else if (filter.key == "departmentName")
                {
                    query = query.Where(tfg => tfg.departmentModel.name.ToLower().Contains(filter.value.ToLower()));
                }
                else if (filter.key == "generic")
                {
                    query = query.Where(tfg => tfg.name.ToLower().Contains(filter.value.ToLower()) || tfg.description.ToLower().Contains(filter.value.ToLower()) || tfg.departmentModel.name.ToLower().Contains(filter.value.ToLower()));
                }
            }

            return query.ToList().ConvertAll(model => new TFGLineDTO(model));
        }

        public void AddCareers(int id, List<int> careers)
        {
            var tfgLine = context.tfg_line.Include(d => d.departmentModel).ThenInclude(d => d.Universities).Include(t => t.Careers).FirstOrDefault(t => t.id == id);
            if (tfgLine == null)
            {
                throw new NotFoundException();
            }

            tfgLine.Careers.Clear();
            foreach (var careerId in careers)
            {
                var career = context.career.FirstOrDefault(c => c.id == careerId);
                if (career == null)
                {
                    throw new NotFoundException($"Career with id {careerId} not found");
                }
                if (!tfgLine.departmentModel.Universities.Any(u => u.university == career.university))
                {
                    throw new UnprocessableException($"Career with id {careerId} does not belong to the same university as TFGLine with id {id}");
                }
                if (tfgLine.Careers.Any(c => c.career == careerId))
                {
                    throw new UnprocessableException($"Career with id {careerId} already exists in TFGLine with id {id}");
                }
                // context.tfg_line_career.Add(new TFGLineCareerModel { career = careerId, tfg_line = id });
                tfgLine.Careers.Add(new TFGLineCareerModel { career = careerId, tfg_line = id });
            }
            context.SaveChanges();
        }

        public void AddProfessors(int id, List<int> professors)
        {
            var tfgLine = context.tfg_line.Include(d => d.departmentModel).Include(t => t.Professors).FirstOrDefault(t => t.id == id);
            if (tfgLine == null)
            {
                throw new NotFoundException();
            }

            tfgLine.Professors.Clear();
            foreach (var professorId in professors)
            {
                var professor = context.professor.FirstOrDefault(c => c.id == professorId);
                if (professor == null)
                {
                    throw new NotFoundException($"Professor with id {professorId} not found");
                }
                if (professor.department != tfgLine.departmentModel.id)
                {
                    throw new UnprocessableException($"Professor with id {professorId} does not belong to the same department as TFGLine with id {id}");
                }
                if (tfgLine.Professors.Any(c => c.professor == professorId))
                {
                    throw new UnprocessableException($"Professor with id {professorId} already exists in TFGLine with id {id}");
                }
                // context.tfg_line_professor.Add(new TFGLineProfessorModel { professor = professorId, tfg_line = id });
                tfgLine.Professors.Add(new TFGLineProfessorModel { professor = professorId, tfg_line = id });
            }
            context.SaveChanges();
        }

        public List<TFGLineDTO> GetTFGLinesByDepartment(int departmentId)
        {
            return context.tfg_line.Where(TFGLine => TFGLine.department == departmentId).Include(d => d.departmentModel).ToList().ConvertAll(model => new TFGLineDTO(model));
        }

        public List<TFGLineDTO> GetByProfessorId(int professorId)
        {
            var query = context.tfg_line.Include(d => d.departmentModel).ThenInclude(d => d.Universities).ThenInclude(u => u.universityModel).Include(d => d.Professors).ThenInclude(d => d.professorModel).AsQueryable();
            return query.Where(tfg => tfg.Professors.Any(t => t.professor == professorId)).ToList().ConvertAll(model => new TFGLineDTO(model));
        }

        public CSVOutput ImportTFGs(string base64)
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
                if (fields.Length < 5)
                {
                    output.errorItems.Add($"Line {i + 1}: Invalid number of fields. Expected at least 5.");
                    continue;
                }
                var title = fields[0].Trim();
                var departmentName = fields[1].Trim();
                var slots = fields[2].Trim();
                var group = string.IsNullOrWhiteSpace(fields[3].Trim()) ? 0 : int.Parse(fields[3].Trim());
                var description = fields[4].Trim();
                if (string.IsNullOrWhiteSpace(title))
                {
                    output.errorItems.Add($"Line {i + 1}: TFG title cannot be empty.");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(departmentName))
                {
                    output.errorItems.Add($"Line {i + 1}: Department name cannot be empty.");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(slots) || !int.TryParse(slots, out int parsedSlots) || parsedSlots <= 0)
                {
                    output.errorItems.Add($"Line {i + 1}: Invalid number of slots '{slots}'. Must be a positive integer.");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(description))
                {
                    output.errorItems.Add($"Line {i + 1}: Description cannot be empty.");
                    continue;
                }

                var department = context.department
                    .Where(d => d.name.ToLower() == departmentName.ToLower() || d.acronym.ToLower() == departmentName.ToLower())
                    .Select(d => d.id)
                    .FirstOrDefault();

                if (department == 0)
                {
                    output.errorItems.Add($"Line {i + 1}: Department '{departmentName}' does not exist.");
                    continue;
                }

                var tfgLine = new TFGLineFlatDTO
                {
                    name = title,
                    description = description,
                    departmentId = department,
                    slots = parsedSlots,
                    group = group == 1
                };

                try
                {
                    CreateTFGLine(tfgLine);
                    output.success++;
                }
                catch (Exception ex)
                {
                    output.errorItems.Add($"Line {i + 1}: {ex.Message}");
                }
            }

            return output;
        }


        #region Private Methods
        private void CheckNameIsNotRepeated(TFGLineFlatDTO TFGLine)
        {
            if (context.tfg_line.Any(t => t.id != TFGLine.id && t.name.ToLower() == TFGLine.name.ToLower()))
            {
                throw new UnprocessableException("TFG_LINE_ALREADY_EXISTS");
            }
        }
        #endregion
    }
}
