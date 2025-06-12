using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using TFGinfo.Common;
using TFGinfo.Data;
using TFGinfo.Models;
using TFGinfo.Objects;

namespace TFGinfo.Api
{
    public class StudentManager : BaseManager
    {
        private readonly EmailService? emailService;
        public StudentManager(ApplicationDbContext context, EmailService? emailService = null) : base(context) {
            this.emailService = emailService;
        }

        public List<StudentDTO> GetAllStudents()
        {
            return context.student.AsNoTracking().Include(d => d.careerModel).ThenInclude(c => c.universityModel).ToList().ConvertAll(model => new StudentDTO(model));
        }
        
        public List<StudentDTO> SearchStudents(List<Filter> filters)
        {
            IQueryable<StudentModel> query = context.student.AsNoTracking().Include(d => d.careerModel).ThenInclude(c => c.universityModel);

            foreach (var filter in filters)
            {
                if (filter.key == "name")
                {
                    query = query.Where(s => s.name.ToLower().Contains(filter.value.ToLower()));
                }
                else if (filter.key == "surname")
                {
                    query = query.Where(s => s.surname.ToLower().Contains(filter.value.ToLower()));
                }
                else if (filter.key == "email")
                {
                    query = query.Where(s => s.email.ToLower().Contains(filter.value.ToLower()));
                }
                else if (filter.key == "career")
                {
                    query = query.Where(s => s.careerModel.name.ToLower().Contains(filter.value.ToLower()));
                }
                else if (filter.key == "university")
                {
                    query = query.Where(s => s.careerModel.universityModel.name.ToLower().Contains(filter.value.ToLower()));
                }
                else if (filter.key == "generic")
                {
                    string searchValue = filter.value.ToLower();
                    query = query.Where(s => s.name.ToLower().Contains(searchValue) ||
                                             s.surname.ToLower().Contains(searchValue) ||
                                             s.email.ToLower().Contains(searchValue) ||
                                             s.careerModel.name.ToLower().Contains(searchValue) ||
                                             s.careerModel.universityModel.name.ToLower().Contains(searchValue));
                }
            }

            return query.ToList().ConvertAll(model => new StudentDTO(model));
        }

        public async Task<NewStudentDTO> CreateStudent(StudentFlatDTO Student)
        {

            CheckEmailIsNotRepeated(Student);
            CheckDniIsNotRepeated(Student);

            UserManager userManager = new UserManager(context, emailService!);
            UserDTO user = await userManager.CreateUser(new UserFlatDTO
            {
                username = Student.email,
                roleId = (int)RoleTypes.Student
            });

            StudentModel model = new StudentModel
            {
                name = Student.name,
                dni = Student.dni,
                surname = Student.surname,
                email = Student.email,
                user = user.id.Value,
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

            var newStudentDTO = new NewStudentDTO(new(savedStudent), user.auth_code);
            return newStudentDTO;
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

            CheckEmailIsNotRepeated(Student);
            CheckDniIsNotRepeated(Student);

            model.name = Student.name;
            model.dni = Student.dni;
            model.surname = Student.surname;
            model.email = Student.email;
            model.phone = Student.phone;
            model.address = Student.address;
            model.birthdate = Student.birthdate;
            context.SaveChanges();

            UserManager userManager = new UserManager(context);
            UserDTO user = userManager.GetUser(model.user);
            if (user.username != Student.email)
            {
                user.username = Student.email;
                userManager.ChangeEmail(user.id.Value, user.username);
            }

            return new StudentDTO(model);
        }

        public StudentDTO UpdateOptionalData(int id, StudentOptionalDataDTO optionalData)
        {
            StudentModel? model = context.student.Include(d => d.careerModel).FirstOrDefault(Student => Student.id == id);
            if (model == null) {
                throw new NotFoundException();
            }

            model.phone = optionalData.phone;
            model.address = optionalData.address;
            model.birthdate = optionalData.birthdate;
            context.SaveChanges();

            return new StudentDTO(model);
        }

        public List<StudentDTO> GetAllByCareer(int careerId)
        {
            return context.student.AsNoTracking().Where(Student => Student.career == careerId).Include(d => d.careerModel).ToList().ConvertAll(model => new StudentDTO(model));
        }

        public StudentDTO GetById(int id)
        {
            StudentModel? model = context.student.Include(d => d.careerModel).ThenInclude(c => c.universityModel).FirstOrDefault(Student => Student.id == id);
            if (model == null) {
                throw new NotFoundException();
            }
            return new StudentDTO(model);
        }

        #region Private Methods
        private void CheckEmailIsNotRepeated(StudentFlatDTO student)
        {
            if (context.student.Any(s => s.id != student.id && s.email.ToLower() == student.email.ToLower())) {
                throw new UnprocessableException("Student email already exists");
            }
        }

        private void CheckDniIsNotRepeated(StudentFlatDTO student)
        {
            if (context.student.Any(s => s.id != student.id && s.dni.ToLower() == student.dni.ToLower())) {
                throw new UnprocessableException("Student dni already exists");
            }
        }
        #endregion
    }
}
