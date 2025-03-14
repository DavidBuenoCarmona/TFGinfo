using TFGinfo.Models;

namespace TFGinfo.Objects  
{
    public class StudentBase {
        public int? id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string dni { get; set; }
        public string? phone { get; set; }
        public string? address { get; set; }
        public DateTime? birthdate { get; set; }

        public StudentBase () {}

        public StudentBase (StudentModel model) {
            name = model.name;
            id = model.id;
            surname = model.surname;
            email = model.email;
            dni = model.dni;
            phone = model.phone;
            address = model.address;
            birthdate = model.birthdate;
        }
    }

    public class StudentDTO : StudentBase {
        public CareerBase career { get; set; }

        public StudentDTO () {}

        public StudentDTO (StudentModel model) : base(model) {
            if (model.careerModel != null) {
                career = new CareerBase(model.careerModel);
            }
        }
    }

    public class StudentFlatDTO : StudentBase {
        public int careerId { get; set; }
        public StudentFlatDTO () {}
        public StudentFlatDTO (StudentModel model) : base(model) {
            careerId = model.career;
        }
    }
}