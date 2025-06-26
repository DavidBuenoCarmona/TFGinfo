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
        public AuthManager(ApplicationDbContext context, IConfiguration configuration) : base(context)
        {
            _configuration = configuration;
        }

        public NewUserDTO Login(LoginCredentials credentials)
        {
            NewUserDTO newUser = new NewUserDTO();
            newUser.firstLogin = false;
            UserModel? model = context.user.Include(r => r.roleModel).FirstOrDefault(user => user.username == credentials.Username);
            if (model == null)
            {
                throw new UnprocessableException("Invalid username or password");
            }
            if (model.password == null && model.auth_code == credentials.Password)
            {
                newUser.firstLogin = true;
            }
            else if (model.password != HashPassword(credentials.Password))
            {
                throw new UnprocessableException("Invalid username or password");
            }
            newUser.user = new AppUserDTO(model);
            if (newUser.user.role.id == (int)UserRole.Student)
            {
                var student = context.student.Include(s => s.careerModel).FirstOrDefault(s => s.user == newUser.user.id);
                if (student != null)
                {
                    newUser.user.career = student.career;
                    newUser.user.id = student.id;
                    newUser.user.universitiesId.Add(student.careerModel?.university ?? 0);
                }
            }
            else if (newUser.user.role.id == (int)UserRole.Professor)
            {
                var teacher = context.professor.Include(p => p.departmentModel).ThenInclude(d => d.Universities).FirstOrDefault(t => t.user == newUser.user.id);
                if (teacher != null)
                {
                    newUser.user.department = teacher.department;
                    newUser.user.id = teacher.id;
                    newUser.user.universitiesId.AddRange(teacher.departmentModel?.Universities.Select(u => u.university) ?? Enumerable.Empty<int>());
                }
            }
            var token = GenerateJwtToken(newUser.user);
            newUser.user.token = token;
            return newUser;
        }

        public bool ChangePassword(ChangePasswordRequest request)
        {

            UserModel? model = context.user.FirstOrDefault(user => user.username == request.username);
            if (model == null)
            {
                throw new UnprocessableException("Invalid username or password");
            }
            if (model.password == null && model.auth_code != null)
            {
                if (model.auth_code != request.OldPassword)
                {
                    throw new UnprocessableException("Invalid username or password");
                }
                model.password = HashPassword(request.NewPassword);
                model.auth_code = null;
                context.SaveChanges();
                return true;
            }
            else if (model.password != HashPassword(request.OldPassword))
            {
                throw new UnprocessableException("Invalid username or password");
            }
            model.password = HashPassword(request.NewPassword);
            context.SaveChanges();
            return true;
        }

        public void CreateAdmin(LoginCredentials credentials)
        {
            if (context.user.Any(u => u.username.ToLower() == credentials.Username.ToLower()))
            {
                throw new UnprocessableException("User name already exists");
            }

            UserModel model = new UserModel
            {
                username = credentials.Username,
                role = 1,
                password = HashPassword(credentials.Password),
            };
            context.user.Add(model);
            context.SaveChanges();
        }

        public AppUserDTO CheckToken(string token)
        {
            var jwtEncryptKey = _configuration["Jwt:EncryptKey"] ?? throw new InvalidOperationException("JWT Encrypt Key is not configured.");
            string decryptedJwt = Jose.JWT.Decode(token, Encoding.UTF8.GetBytes(jwtEncryptKey));

            if (string.IsNullOrEmpty(decryptedJwt) || !IsTokenValid(decryptedJwt, _configuration))
            {
                throw new UnprocessableException("Invalid or expired token");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(decryptedJwt);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId");
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role");
            var usernameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

            if (userIdClaim == null || roleClaim == null || usernameClaim == null)
            {
                throw new UnprocessableException("Invalid token: user ID claim not found");
            }

            int userId = int.Parse(userIdClaim.Value);
            int roleId = int.Parse(roleClaim.Value);
            string username = usernameClaim.Value;
            UserModel? model = context.user.Include(r => r.roleModel).FirstOrDefault(u => u.username == username);

            if (model == null)
            {
                throw new UnprocessableException("User not found");
            }

            AppUserDTO userDto = new AppUserDTO(model);
            userDto.role = context.role.FirstOrDefault(r => r.id == roleId) ?? throw new UnprocessableException("Role not found");
            if (userDto.role.id == (int)UserRole.Student)
            {
                var student = context.student.Include(s => s.careerModel).FirstOrDefault(s => s.user == userDto.id);
                if (student != null)
                {
                    userDto.career = student.career;
                    userDto.id = student.id;
                    userDto.universitiesId.Add(student.careerModel?.university ?? 0);
                }
            }
            else if (userDto.role.id == (int)UserRole.Professor)
            {
                var teacher = context.professor.Include(p => p.departmentModel).ThenInclude(d => d.Universities).FirstOrDefault(t => t.user == userDto.id);
                if (teacher != null)
                {
                    userDto.department = teacher.department;
                    userDto.id = teacher.id;
                    userDto.universitiesId.AddRange(teacher.departmentModel?.Universities.Select(u => u.university) ?? Enumerable.Empty<int>());
                }
            }

            return userDto;
        }

        public AppUserDTO ValidateRoles(string token, List<int> roles)
        {
            AppUserDTO user = null;
            try
            {
                user = CheckToken(token);
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException("Invalid token");
            }

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid token");
            }
            if (roles.Any() && !roles.Contains(user.role.id))
            {
                throw new UnauthorizedAccessException("User does not have the required role to access this resource.");
            }
            return user;
        }

        #region "Private methods"

        private string GenerateJwtToken(AppUserDTO user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.username),
                new Claim("userId", user.id.ToString() ?? throw new InvalidOperationException("User ID cannot be null")),
                new Claim("role", user.role.id.ToString() ?? throw new InvalidOperationException("Role ID cannot be null")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var jwtEncryptKey = _configuration["Jwt:EncryptKey"] ?? throw new InvalidOperationException("JWT Encrypt Key is not configured.");
            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            // 1. Genera el JWT firmado como string
            var jwtString = new JwtSecurityTokenHandler().WriteToken(token);

            // 2. Cifra el JWT usando jose-jwt (JWE)
            var encryptedJwt = Jose.JWT.Encode(jwtString, Encoding.UTF8.GetBytes(jwtEncryptKey), Jose.JweAlgorithm.DIR, Jose.JweEncryption.A128GCM);

            return encryptedJwt;
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
