using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.IRepository;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly TaskManagementDbContext _taskManagementDbContext;


        public AdminRepository(TaskManagementDbContext taskManagementDbContext)
        {
            this._taskManagementDbContext = taskManagementDbContext;
        }

        public async Task<List<Admin>> MonthWisePerformance()
        {
            var completedTasksCount = await _taskManagementDbContext.Tasks
         .Where(t => t.IsCompleted)
         .GroupBy(t => new { t.ManagerId, t.AssignedTo, t.CreatedBy, Month = t.ComplitionDate.Month, Year = t.ComplitionDate.Year })
         .Select(g => new Admin
         {
             ManagerId = g.Key.ManagerId,
             ManagerName = g.Key.CreatedBy,
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
        public async Task<ChartData> GetChartData(ChartResponse chartResponse)
        {
            // Count completed tasks for the specified month and year
            var completedCount = await _taskManagementDbContext.Tasks
                .Where(x => x.AssignedTo == chartResponse.email && x.IsCompleted &&
                            x.ComplitionDate.Month == chartResponse.month &&
                            x.ComplitionDate.Year == chartResponse.year)
                .CountAsync();

            // Count not completed tasks that are still active in the specified month and year
            var notCompletedCount = await _taskManagementDbContext.Tasks
                .Where(x => x.AssignedTo == chartResponse.email && !x.IsCompleted &&
                            x.CreationDate.Month == chartResponse.month &&
                            x.CreationDate.Year == chartResponse.year)
                .CountAsync();

            // Calculate the total count
            var totalCount = completedCount + notCompletedCount;

            // Get the month name for the specified month
            var monthName = new DateTime(chartResponse.year, chartResponse.month, 1).ToString("MMMM");

            // Prepare the response
            var response = new ChartData
            {
                CompletedCount = completedCount,
                NotCompletedCount = notCompletedCount,
                TotalCount = totalCount,
                CurrentMonth = monthName, // Returns the specified month name
            };

            return response;
        }



        public async Task<ActionResult<List<MonthlyEmployeeTaskSummary>>> GetMonthlyEmployeeTaskSummary(string email)
        {
            // Define the last five months
            var lastFiveMonths = Enumerable.Range(0, 5)
                .Select(i => DateTime.Now.AddMonths(-i))
                .Select(d => new { d.Month, d.Year })
                .ToList();

            // Fetch tasks for the last five months
            var tasks = await _taskManagementDbContext.Tasks
                .Where(t => t.AssignedTo == email
                            && t.CreationDate >= DateTime.Now.AddMonths(-5))
                .ToListAsync();

            // Prepare a dictionary to store counts of tasks by month and year
            var taskCounts = lastFiveMonths.ToDictionary(
                m => m,
                m => new
                {
                    CompletedCount = tasks
                        .Where(t => t.ComplitionDate.Month == m.Month && t.ComplitionDate.Year == m.Year && t.IsCompleted)
                        .Count(),
                    UncompletedCount = tasks
                        .Where(t => t.CreationDate.Month == m.Month && t.CreationDate.Year == m.Year && !t.IsCompleted)
                        .Count()
                });

            // Prepare the result list
            var taskSummary = lastFiveMonths
                .Select(m => new MonthlyEmployeeTaskSummary
                {
                    EmployeeName = email,
                    ManagerName = null, // Adjust as needed
                    ManagerId = null,   // Adjust as needed
                    Month = m.Month,
                    Year = m.Year,
                    CompletedTaskCount = taskCounts[m].CompletedCount,
                    UncompletedTaskCount = taskCounts[m].UncompletedCount
                })
                .OrderByDescending(r => r.Year)
                .ThenByDescending(r => r.Month)
                .ToList();

            return taskSummary;
        }




        public async Task<List<EmployeeWithManager>> GetAllEmployee()
        {
            // Get all users (both managers and employees)
            var allUsers = await this._taskManagementDbContext.Users.ToListAsync();

            // Get only employees
            var employees = await this._taskManagementDbContext.Users
                .Where(x => x.Role == "Employee").ToListAsync();

            // Create a list to store employees along with their manager's name
            var employeesWithManagers = employees.Select(employee =>
            {
                // Find the manager for each employee
                var manager = allUsers.FirstOrDefault(u => u.Id == employee.ManagerId);

                // Combine the manager's first and last name
                string managerName = manager != null ? $"{manager.FirstName} {manager.LastName}" : "No Manager";

                return new EmployeeWithManager
                {
                    Id = employee.Id,
                    Email = employee.Email,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    DOB = employee.DOB,
                    Address = employee.Address,
                    Department = employee.Department,
                    PhoneNumber = employee.PhoneNumber,
                    ManagerId = employee?.ManagerId,
                    ManagerName = managerName,
                    Role = employee.Role,
                    // Derived in the code, not from the database
                };
            }).ToList();

            return employeesWithManagers;
        }



        public async Task<List<ApplicationUser>> GetAllManagers()
        {
            return await this._taskManagementDbContext.Users.Where(x => x.Role == "Manager").ToListAsync();
        }

        public async Task<Response> DeleteRecord(string email)
        {
            var data = this._taskManagementDbContext.Users.SingleOrDefaultAsync(x => x.Email == email);
            Response r1 = new Response();
            if (data.Result != null)
            {
                _taskManagementDbContext.Remove(data.Result);
                await this._taskManagementDbContext.SaveChangesAsync();

                r1.StatusMessage = "Record Deleted Succfully";
                return r1;
            }
            else
            {
                r1.StatusMessage = "Error in the Record Deletion";
                return r1;
            }

        }

    }
}
