using TFGinfo.Models;

namespace TFGinfo.Objects  
{
    public class ExternalTutorBase {
        public int? id { get; set; }
        public string name { get; set; }
        public string email { get; set; }

        public ExternalTutorBase () {}

        public ExternalTutorBase (ExternalTutorModel model) {
            name = model.name;
            id = model.id;
            email = model.email;
        }
    }
}