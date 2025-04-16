using System.Collections.Generic;
using System.Security.Cryptography;
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

        public UserDTO CreateUser(UserFlatDTO User)
        { 
            CheckNameIsNotRepeated(User);

            UserModel model = new UserModel {
                username = User.username,
                role = User.roleId,
                auth_code = GenerateTemporaryPassword(),
            };
            context.user.Add(model);
            context.SaveChanges();

            return new(model);
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
        private void CheckNameIsNotRepeated(UserFlatDTO user)
        {
            if (context.user.Any(u => u.id != user.id && u.username.ToLower() == user.username.ToLower())) {
                throw new UnprocessableException("User name already exists");
            }
        }

        private static string GenerateTemporaryPassword(int length = 12)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
            char[] password = new char[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                for (int i = 0; i < password.Length; i++)
                {
                    password[i] = validChars[randomBytes[i] % validChars.Length];
                }
            }
            return new string(password);
        }
        #endregion
    }
}
