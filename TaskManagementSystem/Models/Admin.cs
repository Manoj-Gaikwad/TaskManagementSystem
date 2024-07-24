namespace TaskManagementSystem.Models
{
    public class Admin
    { /*  public int AdminId {  get; set; }*/
        public int ManagerId { get; set; }
        public string ManagerName {  get; set; }
        public string EmpName { get; set; }
        public int Month {  get; set; }
        public int Year { get; set; }
        public int CompletedTaskCount {  get; set; }

    }
}
