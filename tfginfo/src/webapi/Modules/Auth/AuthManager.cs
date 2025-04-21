using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Models;
using TFGinfo.Objects;

namespace TFGinfo.Api
{
    public class AuthManager : BaseManager
    
    {
        private readonly IConfiguration _configuration;
        public AuthManager(ApplicationDbContext context, IConfiguration configuration) : base(context) {
            _configuration = configuration;
        }

        public NewUserDTO Login(LoginCredentials credentials)
        {
            NewUserDTO newUser = new NewUserDTO();
            newUser.firstLogin = false;
            UserModel? model = context.user.Include(r => r.roleModel).FirstOrDefault(user => user.username == credentials.Username);
            if (model == null) {
                throw new UnprocessableException("Invalid username or password");
            }
            if (model.password == null && model.auth_code == credentials.Password) {
                newUser.firstLogin = true;
            } else if (model.password != HashPassword(credentials.Password)) {
                throw new UnprocessableException("Invalid username or password");
            }
            newUser.user = new AppUserDTO(model);
            var token = GenerateJwtToken(newUser.user);
            newUser.user.token = token;
            return newUser;
        }

        public bool ChangePassword(ChangePasswordRequest request)
        {

            UserModel? model = context.user.FirstOrDefault(user => user.username == request.username);
            if (model == null) {
                throw new UnprocessableException("Invalid username or password");
            }
            if (model.password == null && model.auth_code != null) {
                if (model.auth_code != request.OldPassword) {
                    throw new UnprocessableException("Invalid username or password");
                }
                model.password = HashPassword(request.NewPassword);
                model.auth_code = null;
                context.SaveChanges();
                return true;
            } else if (model.password != HashPassword(request.OldPassword)) {
                throw new UnprocessableException("Invalid username or password");
            }
            model.password = HashPassword(request.NewPassword);
            context.SaveChanges();
            return true;
        }

        #region "Private methods"

        private string GenerateJwtToken(AppUserDTO user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.username),
                new Claim("userId", user.id.ToString() ?? throw new InvalidOperationException("User ID cannot be null")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static bool IsTokenValid(string token, IConfiguration configuration)
    {
        var jwtKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
        var jwtIssuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured.");
        var jwtAudience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,
                ValidateAudience = true,
                ValidAudience = jwtAudience,
                ValidateLifetime = true, // Verifica que el token no haya expirado
                ClockSkew = TimeSpan.Zero // Opcional: elimina la tolerancia de tiempo por defecto (5 minutos)
            }, out SecurityToken validatedToken);

            return true; // El token es válido
        }
        catch
        {
            return false; // El token no es válido
        }
    }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash); // Devuelve el hash en formato Base64
            }
        }
    
            #endregion
    

    }
}
