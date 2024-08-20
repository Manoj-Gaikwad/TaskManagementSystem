namespace TaskManagementSystem.Models
{
    public class UpdatePasswordModel
    {
        public string Email {  get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
