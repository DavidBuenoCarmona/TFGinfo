using TFGinfo.Models;

namespace TFGinfo.Objects  
{
    public class DepartmentBase {
        public int? id { get; set; }
        public string name { get; set; }

        public DepartmentBase () {}

        public DepartmentBase (DepartmentModel model) {
            name = model.name;
            id = model.id;
        }
    }

    public class DepartmentDTO : DepartmentBase {
        public UniversityBase university { get; set; }

        public DepartmentDTO () {}

        public DepartmentDTO (DepartmentModel model) : base(model) {
            if (model.universityModel != null) {
                university = new UniversityBase(model.universityModel);
            }
        }
    }

    public class DepartmentFlatDTO : DepartmentBase {
        public int universityId { get; set; }
        public DepartmentFlatDTO () {}
        public DepartmentFlatDTO (DepartmentModel model) : base(model) {
            universityId = model.university;
        }
    }
}