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
        public TFGLineManager(ApplicationDbContext context) : base(context) {}

        public List<TFGLineDTO> GetAllTFGLines()
        {
            return context.tfg_line.Include(d => d.departmentModel).ToList().ConvertAll(model => new TFGLineDTO(model));
        }

        public TFGLineDTO CreateTFGLine(TFGLineFlatDTO TFGLine)
        { 
            CheckNameIsNotRepeated(TFGLine);

            TFGLineModel model = new TFGLineModel {
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
            if (model == null) {
                throw new NotFoundException();
            }
            bool tfgExists = context.tfg.Any(t => t.tfg_line == id);
            if (tfgExists) {
                throw new UnprocessableException("Cannot delete TFGLine because there are TFGs associated with it.");
            }
            List<TFGLineCareerModel>? tfgLineCareers = context.tfg_line_career.Where(t => t.tfg_line == id).ToList(); ;
            if (tfgLineCareers != null) {
                context.tfg_line_career.RemoveRange(tfgLineCareers);
            }
            List<TFGLineProfessorModel>? tfgLineProfessors = context.tfg_line_professor.Where(t => t.tfg_line == id).ToList();
            if (tfgLineProfessors != null) {
                context.tfg_line_professor.RemoveRange(tfgLineProfessors);
            }
            
            context.tfg_line.Remove(model);
            context.SaveChanges();
        }

        public TFGLineDTO UpdateTFGLine(TFGLineFlatDTO TFGLine)
        {
            TFGLineModel? model = context.tfg_line.Include(d => d.departmentModel).FirstOrDefault(d => d.id == TFGLine.id);
            if (model == null) {
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
                Include(d => d.departmentModel).
                Include(d => d.Careers).
                    ThenInclude(c => c.careerModel).
                Include(p => p.Professors).
                    ThenInclude(p => p.professorModel).FirstOrDefault(TFGLine => TFGLine.id == id);
            if (model == null) {
                throw new NotFoundException();
            }
            return new TFGLineDTO(model);
        }

        public List<TFGLineDTO> GetByStudentId(int studentId)
        {
            var query = context.tfg_line.Include(d => d.departmentModel).Include(d => d.TFGs).ThenInclude(d => d.Students).AsQueryable();
            return query.Where(tfg => tfg.TFGs.Any(t => t.Students.Any(s => s.student == studentId))).ToList().ConvertAll(model => new TFGLineDTO(model));
        }

        public List<TFGLineDTO> SearchTFGLines(List<Filter> filters)
        {
            var query = context.tfg_line.Include(d => d.departmentModel).Include(t => t.Careers).ThenInclude(t => t.careerModel).AsQueryable();

            foreach (var filter in filters)
            {
                if (filter.value == null || filter.value == "") continue;
                if (filter.key == "university")
                {
                    query = query.Where(tfg => tfg.departmentModel.university == int.Parse(filter.value));
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
                    query = query.Where(tfg => tfg.Careers.Any(c => c.careerModel.id == int.Parse(filter.value)));
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
            var tfgLine = context.tfg_line.Include(d => d.departmentModel).Include(t => t.Careers).FirstOrDefault(t => t.id == id);
            if (tfgLine == null) {
                throw new NotFoundException();
            }

            tfgLine.Careers.Clear();
            foreach (var careerId in careers) {
                var career = context.career.FirstOrDefault(c => c.id == careerId);
                if (career == null) {
                    throw new NotFoundException($"Career with id {careerId} not found");
                }
                if (career.university != tfgLine.departmentModel.university) {
                    throw new UnprocessableException($"Career with id {careerId} does not belong to the same university as TFGLine with id {id}");
                }
                if (tfgLine.Careers.Any(c => c.career == careerId)) {
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
            if (tfgLine == null) {
                throw new NotFoundException();
            }

            tfgLine.Professors.Clear();
            foreach (var professorId in professors) {
                var professor = context.professor.FirstOrDefault(c => c.id == professorId);
                if (professor == null) {
                    throw new NotFoundException($"Professor with id {professorId} not found");
                }
                if (professor.department != tfgLine.departmentModel.id) {
                    throw new UnprocessableException($"Professor with id {professorId} does not belong to the same department as TFGLine with id {id}");
                }
                if (tfgLine.Professors.Any(c => c.professor == professorId)) {
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
            var query = context.tfg_line.Include(d => d.departmentModel).Include(d => d.Professors).ThenInclude(d => d.professorModel).AsQueryable();
            return query.Where(tfg => tfg.Professors.Any(t => t.professor == professorId)).ToList().ConvertAll(model => new TFGLineDTO(model));
        }

        #region Private Methods
        private void CheckNameIsNotRepeated(TFGLineFlatDTO TFGLine)
        {
            if (context.tfg_line.Any(t => t.id != TFGLine.id && t.name.ToLower() == TFGLine.name.ToLower())) {
                throw new UnprocessableException("TFGLine name already exists");
            }
        }
        #endregion
    }
}
