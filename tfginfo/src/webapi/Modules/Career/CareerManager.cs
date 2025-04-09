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
        public CareerManager(ApplicationDbContext context) : base(context) {}

        public List<CareerDTO> GetAllCareers()
        {
            return context.career.Include(d => d.universityModel).ToList().ConvertAll(model => new CareerDTO(model));
        }

        public CareerDTO CreateCareer(CareerFlatDTO Career)
        { 
            CheckNameIsNotRepeated(Career);

            CareerModel model = new CareerModel {
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
            if (model == null) {
                throw new NotFoundException();
            }
            context.career.Remove(model);
            context.SaveChanges();
        }

        public CareerDTO UpdateCareer(CareerFlatDTO Career)
        {
            CareerModel? model = context.career.Include(d => d.universityModel).FirstOrDefault(d => d.id == Career.id);
            if (model == null) {
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
            if (model == null) {
                throw new NotFoundException();
            }
            return new CareerDTO(model);
        }

        public List<CareerDTO> GetCareersByUniversity(int universityId)
        {
            return context.career.Where(Career => Career.university == universityId).Include(d => d.universityModel).ToList().ConvertAll(model => new CareerDTO(model));
        }

        #region Private Methods
        private void CheckNameIsNotRepeated(CareerFlatDTO career)
        {
            if (context.career.Any(c => c.id != career.id && c.name.ToLower() == career.name.ToLower())) {
                throw new UnprocessableException("Career name already exists");
            }
        }
        #endregion
    }
}
