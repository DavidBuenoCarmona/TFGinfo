using TFGinfo.Models;

namespace TFGinfo.Objects  
{
    public class WorkingGroupBase {
        public int? id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool isPrivate { get; set; }

        public WorkingGroupBase () {}

        public WorkingGroupBase (WorkingGroupModel model) {
            id = model.id;
            name = model.name;
            description = model.description;
            isPrivate = model.isPrivate == 1;
        }
    }
}