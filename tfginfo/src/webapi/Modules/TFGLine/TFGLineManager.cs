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
            TFGLineModel? model = context.tfg_line.Include(d => d.departmentModel).FirstOrDefault(TFGLine => TFGLine.id == id);
            if (model == null) {
                throw new NotFoundException();
            }
            return new TFGLineDTO(model);
        }

        public List<TFGLineDTO> GetTFGLinesByDepartment(int departmentId)
        {
            return context.tfg_line.Where(TFGLine => TFGLine.department == departmentId).Include(d => d.departmentModel).ToList().ConvertAll(model => new TFGLineDTO(model));
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
