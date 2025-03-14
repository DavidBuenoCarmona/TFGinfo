using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Models;
using TFGinfo.Objects;

namespace TFGinfo.Api
{
    public class UserManager : BaseManager
    {
        public UserManager(ApplicationDbContext context) : base(context) {}

        public int CreateUser(UserFlatDTO User)
        { 
            CheckNameIsNotRepeated(User.username);

            UserModel model = new UserModel {
                username = User.username,
                role = User.roleId,
            };
            context.user.Add(model);
            context.SaveChanges();

            return model.id;
        }

        public void DeleteUser(int id)
        {
            UserModel? model = context.user.FirstOrDefault(User => User.id == id);
            if (model == null) {
                throw new NotFoundException();
            }
            context.user.Remove(model);
            context.SaveChanges();
        }

        #region Private Methods
        private void CheckNameIsNotRepeated(string name)
        {
            if (context.user.Any(User => User.username.ToLower() == name.ToLower())) {
                throw new UnprocessableException("User name already exists");
            }
        }
        #endregion
    }
}
