using System.Collections.Generic;
using System.Reflection.Metadata;
using System;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class TaskModel
    {
        [Key]
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AssignedTo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsCompleted { get; set; }
        public string Uplodedfile { get; set; }
        public DateTime ComplitionDate { get; set; }
        public int ManagerId {  get; set; } 



    }
}
