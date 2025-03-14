using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Models;
using TFGinfo.Objects;

namespace TFGinfo.Api
{
    public class StudentManager : BaseManager
    {
        public StudentManager(ApplicationDbContext context) : base(context) {}

        public List<StudentDTO> GetAllStudents()
        {
            return context.student.AsNoTracking().Include(d => d.careerModel).ToList().ConvertAll(model => new StudentDTO(model));
        }

        public StudentDTO CreateStudent(StudentFlatDTO Student)
        { 
            
            CheckEmailIsNotRepeated(Student.email);
            CheckDniIsNotRepeated(Student.dni);
            
            UserManager userManager = new UserManager(context);
            int userId = userManager.CreateUser(new UserFlatDTO {
                username = Student.email,
                roleId = (int)RoleTypes.Student
            });

            StudentModel model = new StudentModel {
                name = Student.name,
                dni = Student.dni,
                surname = Student.surname,
                email = Student.email,
                user = userId,
                phone = Student.phone,
                address = Student.address,
                career = Student.careerId,
                birthdate = Student.birthdate
            };
            context.student.Add(model);
            context.SaveChanges();

            var savedStudent = context.student
                .Where(d => d.id == model.id)
                .Include(d => d.careerModel)
                .FirstOrDefault() ?? model;

            return new StudentDTO(savedStudent);
        }

        public void DeleteStudent(int id)
        {
            StudentModel? model = context.student.FirstOrDefault(Student => Student.id == id);
            if (model == null) {
                throw new NotFoundException();
            }
            int userId = model.user;

            context.student.Remove(model);
            context.SaveChanges();

            UserManager userManager = new UserManager(context);
            userManager.DeleteUser(userId);
        }

        public StudentDTO UpdateStudent(StudentFlatDTO Student)
        {
            StudentModel? model = context.student.Include(d => d.careerModel).FirstOrDefault(d => d.id == Student.id);
            if (model == null) {
                throw new NotFoundException();
            }

            model.phone = Student.phone;
            model.address = Student.address;
            context.SaveChanges();

            return new StudentDTO(model);
        }

        public List<StudentDTO> GetAllByCareer(int careerId)
        {
            return context.student.AsNoTracking().Where(Student => Student.career == careerId).Include(d => d.careerModel).ToList().ConvertAll(model => new StudentDTO(model));
        }

        #region Private Methods
        private void CheckEmailIsNotRepeated(string email)
        {
            if (context.student.Any(Student => Student.email.ToLower() == email.ToLower())) {
                throw new UnprocessableException("Student email already exists");
            }
        }

        private void CheckDniIsNotRepeated(string dni)
        {
            if (context.student.Any(Student => Student.dni.ToLower() == dni.ToLower())) {
                throw new UnprocessableException("Student dni already exists");
            }
        }
        #endregion
    }
}
