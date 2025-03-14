using TFGinfo.Models;

namespace TFGinfo.Objects  
{
    public class RoleBase {
        public int? id { get; set; }
        public string name { get; set; }
        public string code { get; set; }

        public RoleBase () {}

        public RoleBase (RoleModel model) {
            name = model.name;
            code = model.code;
            id = model.id;
        }
    }

    public enum RoleTypes {
        Admin = 1,
        Professor = 2,
        Student = 3
    }
}