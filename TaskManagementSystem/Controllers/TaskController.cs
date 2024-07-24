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

        [HttpPost("Create Task")]
        [Authorize(Roles = "Manager , Admin")]
       public async Task<string> CreateTask(TaskModel t1)
       {
            return await _itaskRepository.CreateTask(t1);
       }
    }
}
