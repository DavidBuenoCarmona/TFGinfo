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

    public class WorkingGroupUser {
        public int working_group { get; set; }
        public int user { get; set; }
    }

    public class WorkingGroupProfessor {
        public WorkingGroupBase working_group { get; set; }
        public int professor { get; set; }
    }

    public class WorkingGroupMessage {
        public int working_group { get; set; }
        public int professor { get; set; }
        public string message { get; set; }
    }
}