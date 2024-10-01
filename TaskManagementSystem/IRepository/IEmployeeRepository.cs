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
        Task<object> GetManagerWiseEmployee();
        Task<List<TaskModel>> GetAllTasks();
        Task<Response> DeleteRecord(string email);
        Task<object> UploadDocument(IFormFile File, bool IsComplited, string Fileupload, int TaskId);
    }
}
