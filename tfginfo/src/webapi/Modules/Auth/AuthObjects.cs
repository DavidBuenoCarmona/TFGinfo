
namespace TFGinfo.Objects  
{
    public class LoginCredentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ChangePasswordRequest
    {
        public string username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public enum UserRole
    {
        Admin = 1,
        Professor = 2,
        Student = 3,
    }
}