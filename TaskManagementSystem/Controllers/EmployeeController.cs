using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.IRepository;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repository;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _iemployeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            this._iemployeeRepository = employeeRepository;
        }
        [HttpGet("GetAllEmployeeList")]
        public async Task<List<ApplicationUser>> GetAll()
        {
            return await _iemployeeRepository.GetAll();
        }
        [HttpGet("GetManagerWiseEmployee")]
        public async Task<List<ApplicationUser>> GetManagerWiseEmployee()
        {
            return await _iemployeeRepository.GetManagerWiseEmployee();
        }

        [HttpGet("GetAllTasks")]
        [Authorize(Roles = "Admin, Manager, Employee")]
        public async Task<List<TaskModel>> GetAllTasks()
        {
            return await _iemployeeRepository.GetAllTasks();
        }

        [HttpPost("UploadFile")]
        [Consumes("multipart/form-data")]
        public async Task<string> UploadDocument([FromForm] TaskComplition taskComplition)
        {
            return await _iemployeeRepository.UploadDocument(taskComplition);
        }

    }
}
