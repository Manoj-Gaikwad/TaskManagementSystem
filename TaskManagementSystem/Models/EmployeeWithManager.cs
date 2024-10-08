﻿using System;

namespace TaskManagementSystem.Models
{
    public class EmployeeWithManager
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DOB { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public string Role { get; set; }
        public string ManagerId { get; set; }
        public string ManagerName { get; set; }
    }
}
