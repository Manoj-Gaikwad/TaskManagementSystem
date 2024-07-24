using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class Manager
    {
        [Key]
        public int ManagerId { get; set; }
        public string ManagerName { get; set; }
        public string Email {  get; set; }
        public string Department {  get; set; }
    }
}
