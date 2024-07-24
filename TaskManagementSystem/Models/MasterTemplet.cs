
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class MasterTemplet
    {
        [Key]
        public int TempId {  get; set; }
        public string TempName {  get; set; }
        public string Title {  get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy {  get; set; }

    }
}
