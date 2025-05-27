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
                                             u.address.ToLower().Contains(filter.value.ToLower()));
                }
            } 
            return query.ToList().ConvertAll(model => new UniversityBase(model)); 
        }

        #region Private Methods
        private void CheckNameIsNotRepeated(UniversityBase university)
        {
            if (context.university.Any(u => u.id != university.id && u.name.ToLower() == university.name.ToLower()))
            {
                throw new UnprocessableException("University name already exists");
            }
        }

        #endregion


    }
}