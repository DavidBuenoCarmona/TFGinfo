using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Models;
using TFGinfo.Objects;

namespace TFGinfo.Api
{
    public class TFGManager : BaseManager
    {
        public TFGManager(ApplicationDbContext context) : base(context) {}

        public List<TFGDTO> GetAllTFGs()
        {
            return context.tfg.Include(d => d.tfgLineModel).ToList().ConvertAll(model => new TFGDTO(model));
        }

        public TFGDTO CreateTFG(TFGFlatDTO TFG)
        { 

            TFGModel model = new TFGModel {
                startDate = TFG.startDate,
                tfgLine = TFG.tfgLineId
            };
            context.tfg.Add(model);
            context.SaveChanges();

            var savedTFG = context.tfg
                .Where(d => d.id == model.id)
                .Include(d => d.tfgLineModel)
                .FirstOrDefault() ?? model;

            return new TFGDTO(savedTFG);
        }

        public void DeleteTFG(int id)
        {
            TFGModel? model = context.tfg.FirstOrDefault(TFG => TFG.id == id);
            if (model == null) {
                throw new NotFoundException();
            }
            context.tfg.Remove(model);
            context.SaveChanges();
        }

        public TFGDTO UpdateTFG(TFGFlatDTO TFG)
        {
            TFGModel? model = context.tfg.Include(d => d.tfgLineModel).FirstOrDefault(d => d.id == TFG.id);
            if (model == null) {
                throw new NotFoundException();
            }

            model.startDate = TFG.startDate;
            context.SaveChanges();

            return new TFGDTO(model);
        }

        public List<TFGDTO> GetTFGsByTFGLine(int tfgLineId)
        {
            return context.tfg.Where(TFG => TFG.tfgLine == tfgLineId).Include(d => d.tfgLineModel).ToList().ConvertAll(model => new TFGDTO(model));
        }

        public TFGDTO GetTFGById(int id)
        {
            TFGModel? model = context.tfg.Include(d => d.tfgLineModel).FirstOrDefault(d => d.id == id);
            if (model == null) {
                throw new NotFoundException();
            }
            return new TFGDTO(model);
        }

        public List<TFGDTO> SearchTFGs(List<Filter> filters)
        {
            var query = context.tfg.Include(d => d.tfgLineModel).ThenInclude(d => d.departmentModel).AsQueryable();

            foreach (var filter in filters)
            {
                if (filter.key == "university")
                {
                    query = query.Where(tfg => tfg.tfgLineModel.departmentModel.university == int.Parse(filter.value));
                }
                else if (filter.key == "startDate")
                {
                    query = query.Where(tfg => tfg.startDate == DateTime.Parse(filter.value));
                }
            }

            return query.ToList().ConvertAll(model => new TFGDTO(model));
        }

        #region Private Methods
        #endregion
    }
}
