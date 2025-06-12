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
        private readonly EmailService? emailService;
        public UserManager(ApplicationDbContext context, EmailService? emailService = null) : base(context)
        {
            this.emailService = emailService;
        }

        public async Task<UserDTO> CreateUser(UserFlatDTO User)
        { 
            CheckNameIsNotRepeated(User);
            string authCode = GenerateTemporaryPassword();
            UserModel model = new UserModel {
                username = User.username,
                role = User.roleId,
                auth_code = authCode,
            };
            context.user.Add(model);
            context.SaveChanges();

            // Send email with auth code
            var body = $"Se te ha creado un usuario para TFGinfo.\n\n" +
                $"Se ha generado una contraseña temporal {authCode} para el usuario {User.username}.\n\n";
            await emailService.SendEmailAsync(User.username, "Nuevo usuario para TFGinfo", body);

            return new UserDTO(model);
        }
        
        public void ChangeEmail(int id, string newEmail)
        {
            UserModel? model = context.user.FirstOrDefault(u => u.id == id);
            if (model == null)
            {
                throw new NotFoundException();
            }
            if (context.user.Any(u => u.id != id && u.username.ToLower() == newEmail.ToLower()))
            {
                throw new UnprocessableException("User name already exists");
            }
            model.username = newEmail;
            context.SaveChanges();
        }

        public UserDTO GetUser(int id)
        {
            UserModel? model = context.user.AsNoTracking().FirstOrDefault(u => u.id == id);
            if (model == null)
            {
                throw new NotFoundException();
            }
            return new UserDTO(model);
        }

        public void DeleteUser(int id)
        {
            UserModel? model = context.user.FirstOrDefault(User => User.id == id);
            if (model == null)
            {
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
