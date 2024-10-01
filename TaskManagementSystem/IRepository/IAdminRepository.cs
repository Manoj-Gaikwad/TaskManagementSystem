using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repository;

namespace TaskManagementSystem.IRepository
{
    public interface IAdminRepository
    {
        Task<List<Admin>> MonthWisePerformance();
        Task<ChartData> GetChartData(ChartResponse chartResponse);
        Task<ActionResult<List<MonthlyEmployeeTaskSummary>>> GetMonthlyEmployeeTaskSummary(string email); // Updated signature
        Task<List<EmployeeWithManager>> GetAllEmployee();
        Task<List<ApplicationUser>> GetAllManagers();
        Task<Response> DeleteRecord(string email);
    }
}
