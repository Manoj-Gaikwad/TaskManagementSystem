namespace TaskManagementSystem.Models
{
    public class MonthlyEmployeeTaskSummary
    {
        public string EmployeeName { get; set; }
        public string ManagerName { get; set; }
        public string ManagerId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int CompletedTaskCount { get; set; }
        public int UncompletedTaskCount { get; set; }
    }
}
