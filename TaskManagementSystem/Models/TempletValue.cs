using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class TempletValue
    {
        [Key]
        public int TVId {  get; set; }
        public int FId {  get; set; }
        public string TFValue { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }




    }
}
