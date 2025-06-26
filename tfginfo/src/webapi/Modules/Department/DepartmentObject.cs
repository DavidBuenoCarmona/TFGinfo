using TFGinfo.Models;

namespace TFGinfo.Objects  
{
    public class DepartmentBase {
        public int? id { get; set; }
        public string name { get; set; }
        public string acronym { get; set; }

        public DepartmentBase() { }

        public DepartmentBase(DepartmentModel model)
        {
            name = model.name;
            id = model.id;
            acronym = model.acronym;
        }
    }

    public class DepartmentDTO : DepartmentBase {
        public List<UniversityBase> universities { get; set; }

        public DepartmentDTO () {}

        public DepartmentDTO (DepartmentModel model) : base(model) {
            if (model.Universities != null) {
                universities = model.Universities
                    .Select(u => new UniversityBase(u.universityModel))
                    .ToList();
            }
        }
    }

    public class DepartmentFlatDTO : DepartmentBase {
        public List<int> universitiesId { get; set; }
        public DepartmentFlatDTO () {}
        public DepartmentFlatDTO (DepartmentModel model) : base(model) {
            if (model.Universities != null) {
                universitiesId = model.Universities
                    .Select(u => u.university)
                    .ToList();
            }
        }
    }
}