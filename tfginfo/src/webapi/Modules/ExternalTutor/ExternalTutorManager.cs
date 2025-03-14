using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Models;
using TFGinfo.Objects;

namespace TFGinfo.Api
{
    public class ExternalTutorManager : BaseManager
    {
        public ExternalTutorManager(ApplicationDbContext context) : base(context) {}
        public List<ExternalTutorBase> GetAllExternalTutors()
        {
            return context.external_tutor.ToList().ConvertAll(model => new ExternalTutorBase(model));
        }

        public ExternalTutorBase CreateExternalTutor(ExternalTutorBase ExternalTutor)
        { 
            CheckEmailIsNotRepeated(ExternalTutor);

            ExternalTutorModel model = new ExternalTutorModel {
                name = ExternalTutor.name,
                email = ExternalTutor.email
            };
            context.external_tutor.Add(model);
            context.SaveChanges();

            return new ExternalTutorBase(model);
        }

        public void DeleteExternalTutor(int id)
        {
            ExternalTutorModel? model = context.external_tutor.FirstOrDefault(ExternalTutor => ExternalTutor.id == id);
            if (model == null) {
                throw new NotFoundException();
            }
            context.external_tutor.Remove(model);
            context.SaveChanges();
        }

        public ExternalTutorBase UpdateExternalTutor(ExternalTutorBase ExternalTutor)
        {
            ExternalTutorModel? model = context.external_tutor.FirstOrDefault(u => u.id == ExternalTutor.id);
            if (model == null) {
                throw new NotFoundException();
            }

            CheckEmailIsNotRepeated(ExternalTutor);

            model.name = ExternalTutor.name;
            model.email = ExternalTutor.email;
            context.SaveChanges();

            return new ExternalTutorBase(model);
        }

        #region Private Methods
        private void CheckEmailIsNotRepeated(ExternalTutorBase tutor)
        {
            if (context.external_tutor.Any(e => e.id != tutor.id && e.email.ToLower() == tutor.email.ToLower())) {
                throw new UnprocessableException("ExternalTutor email already exists");
            }
        }
        #endregion


    }
}