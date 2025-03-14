using TFGinfo.Models;

namespace TFGinfo.Objects  
{
    public class CareerBase {
        public int? id { get; set; }
        public string name { get; set; }

        public CareerBase () {}

        public CareerBase (CareerModel model) {
            name = model.name;
            id = model.id;
        }
    }

    public class CareerDTO : CareerBase {
        public UniversityBase university { get; set; }

        public CareerDTO () {}

        public CareerDTO (CareerModel model) : base(model) {
            if (model.universityModel != null) {
                university = new UniversityBase(model.universityModel);
            }
        }
    }

    public class CareerFlatDTO : CareerBase {
        public int universityId { get; set; }
        public CareerFlatDTO () {}
        public CareerFlatDTO (CareerModel model) : base(model) {
            universityId = model.university;
        }
    }
}