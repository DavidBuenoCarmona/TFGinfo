using TFGinfo.Models;

namespace TFGinfo.Objects  
{
    public class ProfessorBase {
        public int? id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public bool department_boss { get; set; }

        public ProfessorBase () {}

        public ProfessorBase (ProfessorModel model) {
            name = model.name;
            id = model.id;
            surname = model.surname;
            email = model.email;
            department_boss = model.department_boss == 1;
        }
    }

    public class ProfessorDTO : ProfessorBase {
        public DepartmentBase department { get; set; }

        public ProfessorDTO () {}

        public ProfessorDTO (ProfessorModel model) : base(model) {
            if (model.departmentModel != null) {
                department = new DepartmentBase(model.departmentModel);
            }
        }
    }

    public class ProfessorFlatDTO : ProfessorBase {
        public int departmentId { get; set; }
        public ProfessorFlatDTO () {}
        public ProfessorFlatDTO (ProfessorModel model) : base(model) {
            departmentId = model.department;
        }
    }

    public class NewProfessorDTO {
        public ProfessorDTO professor { get; set; }
        public string auth_code { get; set; }
        public NewProfessorDTO (ProfessorDTO professor, string auth_code) {
            this.professor = professor;
            this.auth_code = auth_code;
        }
    }
}