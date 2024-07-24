using System.Collections.Generic;

namespace TaskManagementSystem.Models
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }


    public class LoginResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Roles { get; set; }
    }
}
