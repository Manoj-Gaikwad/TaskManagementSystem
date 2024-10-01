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
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            this._adminRepository = adminRepository;
        }


        [HttpGet("MonthWisePerformance")]
        [Authorize(Roles = "Admin")]
        public async Task<List<Admin>> MonthWisePerformance()
        {
            return await _adminRepository.MonthWisePerformance();
        }
        [HttpPost("GetChartData")]
        [Authorize(Roles = "Admin")]
        public async Task<ChartData> GetChartData(ChartResponse chartResponse)
        {
            return await _adminRepository.GetChartData(chartResponse);
        }

        [HttpGet("GetMonthlyEmployeeTaskSummary")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<MonthlyEmployeeTaskSummary>>> GetMonthlyEmployeeTaskSummary(string email)
        {
            var summary = await _adminRepository.GetMonthlyEmployeeTaskSummary(email); // Pass email parameter
            return Ok(summary);
        }
        [HttpGet("GetAllEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<List<EmployeeWithManager>> GetAllEmployee()
        {
            return await _adminRepository.GetAllEmployee();
        }

        [HttpGet("GetAllManager")]
        [Authorize(Roles = "Admin")]
        public async Task<List<ApplicationUser>> GetAllManagers()
        {
            return await _adminRepository.GetAllManagers();
        }
     
        [HttpDelete("DeleteRecoredManager")]
        [Authorize(Roles = "Admin")]
        public async Task<Response> DeleteRecord(string email)
        {
           return await _adminRepository.DeleteRecord(email);
        }
    }
}

