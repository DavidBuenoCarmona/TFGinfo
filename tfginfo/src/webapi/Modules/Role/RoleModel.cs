namespace TFGinfo.Models
{
    public class RoleModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }

        public List<UserModel> Users { get; set; }

    }
}