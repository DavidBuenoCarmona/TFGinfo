using TFGinfo.Models;

namespace TFGinfo.Objects  
{
    public class UniversityBase {
        public int? id { get; set; }
        public string name { get; set; }
        public string address { get; set; }

        public UniversityBase () {}

        public UniversityModel toModel () {
            return new UniversityModel {
                name = this.name,
                address = this.address
            };
        }

        public UniversityBase (UniversityModel model) {
            name = model.name;
            address = model.address;
            id = model.id;
        }
    }
}