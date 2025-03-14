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

            UniversityModel model = new UniversityModel {
                name = university.name,
                address = university.address
            };
            context.university.Add(model);
            context.SaveChanges();

            return new UniversityBase(model);
        }

        public void DeleteUniversity(int id)
        {
            // TODO: Check if the university has any related data (Departments, Degrees, etc)
            UniversityModel? model = context.university.FirstOrDefault(university => university.id == id);
            if (model == null) {
                throw new NotFoundException();
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
            context.SaveChanges();

            return new UniversityBase(model);
        }

        #region Private Methods
        private void CheckNameIsNotRepeated(UniversityBase university)
        {
            if (context.university.Any(u => u.id != university.id && u.name.ToLower() == university.name.ToLower())) {
                throw new UnprocessableException("University name already exists");
            }
        }

        #endregion


    }
}