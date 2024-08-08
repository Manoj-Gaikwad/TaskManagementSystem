using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.IRepository
{
    public interface IEmployeeRepository
    {
        Task<List<ApplicationUser>> GetAll();
        Task<List<ApplicationUser>> GetManagerWiseEmployee();
        Task<List<TaskModel>> GetAllTasks();
        Task<string> UploadDocument(TaskComplition taskComplition);
    }
}
