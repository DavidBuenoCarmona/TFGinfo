using TFGinfo.Models;

namespace TFGinfo.Objects  
{
    public class UserBase {
        public int? id { get; set; }
        public string username { get; set; }
        public string? password { get; set; }
        public string? auth_code { get; set; }

        public UserBase () {}

        public UserBase (UserModel model) {
            id = model.id;
            username = model.username;
            password = model.password;
            auth_code = model.auth_code;
        }
    }

    public class UserDTO : UserBase {
        public RoleBase role { get; set; }
        public string? token { get; set; }

        public UserDTO () {}

        public UserDTO (UserModel model) : base(model) {
            if (model.roleModel != null) {
                role = new RoleBase(model.roleModel);
            }
        }
    }

    public class UserFlatDTO : UserBase {
        public int roleId { get; set; }
        public UserFlatDTO () {}
        public UserFlatDTO (UserModel model) : base(model) {
            roleId = model.role;
        }
    }

    public class AppUserDTO {
        public int id { get; set; }
        public string? token { get; set; }
        public RoleModel role { get; set; }
        public string username { get; set; }
        public int? department { get; set; }
        public int? career { get; set; }
        public List<int> universitiesId { get; set; }
        public AppUserDTO(UserModel model)
        {
            id = model.id;
            role = model.roleModel;
            username = model.username;
            universitiesId = new List<int>();
        }
    }

    public class NewUserDTO {
        public AppUserDTO user { get; set; }
        public bool firstLogin { get; set; }
    }
}