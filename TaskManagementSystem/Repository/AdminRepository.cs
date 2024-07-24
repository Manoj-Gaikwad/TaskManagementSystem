using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.IRepository;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Repository
{
    public class AdminRepository:IAdminRepository
    {
        private readonly TaskManagementDbContext _taskManagementDbContext;


        public AdminRepository(TaskManagementDbContext taskManagementDbContext)
        {
            this._taskManagementDbContext = taskManagementDbContext;
        }

        public async Task<List<Admin>>MonthWisePerformance()
        {
            var completedTasksCount = await _taskManagementDbContext.Tasks
         .Where(t => t.IsCompleted)
         .GroupBy(t => new { t.ManagerId, t.AssignedTo,t.CreatedBy, Month = t.ComplitionDate.Month, Year = t.ComplitionDate.Year })
         .Select(g => new Admin
         {
             ManagerId = g.Key.ManagerId,
             ManagerName=g.Key.CreatedBy,
             EmpName = g.Key.AssignedTo,  // Assuming AssignedTo is a string representing the employee's name
             Month = g.Key.Month,
             Year = g.Key.Year,
             CompletedTaskCount = g.Count()
         })
         .OrderBy(r => r.ManagerId)
         .ThenBy(r => r.Year)
         .ThenBy(r => r.Month)
         .ToListAsync();

            return completedTasksCount;
        }
    }
}
