using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class TempletFields
    {
        [Key]
       public int FId {  get; set; }
       public string FName { get; set; }
       public int TempId { get; set; }
        public string CreatedBy {  get; set; }
        public string CreatedDate {  get; set; }
    }
}
