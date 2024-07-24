using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class TaskComplition
    {
      
       public  IFormFile File { get; set; }
       public bool IsComplited {  get; set; }
       public string Fileupload { get; set; }
       public int TaskId {  get; set; }
      
    }
}
