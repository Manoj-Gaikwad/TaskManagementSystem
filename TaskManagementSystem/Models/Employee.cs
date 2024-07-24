using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class Employee
    {
        [Key]
        public int EmpId { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleId { get; set; } // Employee, Manager, Admin
        public string ManagerId { get; set; }
    }
}
