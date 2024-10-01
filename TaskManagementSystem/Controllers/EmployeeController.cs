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
        public async Task<object> GetManagerWiseEmployee()
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
        public async Task<object> UploadDocument([FromForm] IFormFile File, [FromForm] bool IsComplited, [FromForm] string Fileupload, [FromForm] int TaskId)
        {
            var data=await _iemployeeRepository.UploadDocument(File, IsComplited, Fileupload,TaskId);
            return Ok(data);
        }
        [HttpDelete("DeleteRecordEmployee")]
        public async Task<Response> DeleteRecord(string email)
        {
            return await _iemployeeRepository.DeleteRecord(email);
        }
    }
}
