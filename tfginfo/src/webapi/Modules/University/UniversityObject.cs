using TFGinfo.Models;

namespace TFGinfo.Objects  
{
    public class UniversityBase {
        public int? id { get; set; }
        public string name { get; set; }
        public string? acronym { get; set; }
        public string address { get; set; }

        public UniversityBase () {}

        public UniversityBase (UniversityModel model) {
            name = model.name;
            address = model.address;
            id = model.id;
            acronym = model.acronym;
        }
    }
}