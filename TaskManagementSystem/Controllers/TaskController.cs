using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.IRepository;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _itaskRepository;

        public TaskController(ITaskRepository itaskRepository)
        {
            this._itaskRepository = itaskRepository;
        }
        [HttpGet("GetAllTasks")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<List<TaskModel>> GetAllTasks()
        {
            return await _itaskRepository.GetAllTasks();
        }

        [HttpPost("CreateTask")]
        [Authorize(Roles = "Manager , Admin")]
       public async Task<Response> CreateTask(TaskModel t1)
       {
            return await _itaskRepository.CreateTask(t1);
       }
        [HttpPost("UpdateTask")]
        [Authorize(Roles = "Manager , Admin")]
        public async Task<string> UpdateTask(TaskModel t1)
        {
            return await _itaskRepository.UpdateTask(t1);
        }
        [HttpGet("GetAllManagedEmployees")]
        public async Task<object> GetManagerAndEmployeesEmailsAsync()
        {
            return await _itaskRepository.GetManagerAndEmployeesEmailsAsync();
        }

        [HttpGet("GetTaskById")]
        public async Task<TaskModel> GetTaskById(int TaskId)
        {
            return await _itaskRepository.GetTaskById(TaskId);
        }
    }
}
