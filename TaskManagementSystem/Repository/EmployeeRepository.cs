using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Hosting;
using TaskManagementSystem.IRepository;
using TaskManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementSystem.Repository
{
    public class EmployeeRepository:IEmployeeRepository
    {
        private readonly TaskManagementDbContext _taskdbconnection;
        private readonly IWebHostEnvironment _environment;
        private  string _uploadPath;
        private readonly ISessionData _sessionData;

        public EmployeeRepository(TaskManagementDbContext taskdbconnection, IWebHostEnvironment environment, ISessionData sessionData)
        {
            this._taskdbconnection = taskdbconnection;
            _environment = environment;
            this._sessionData = sessionData;
        }

        public async Task<List<ApplicationUser>> GetAll(){
            return await this._taskdbconnection.Users.ToListAsync();
        }

        public async Task<object>GetManagerWiseEmployee()
        {
            
           var manager = await this._taskdbconnection.Users.FirstOrDefaultAsync(x=>x.Email== this._sessionData.UserEmail);

            var employee = await this._taskdbconnection.Users.Where(x => x.ManagerId == manager.Id && x.Email != manager.Email).ToListAsync();

            return new {
                manager = manager,
                data = employee
            };
            
           
        }
        public async Task<List<TaskModel>> GetAllTasks()
        {
            //var result = from mt in _taskdbconnection.MasterTemplet
            //             join tf in _taskdbconnection.TempletFields on mt.TempId equals tf.TempId
            //             join tv in _taskdbconnection.TempletValues on tf.FId equals tv.FId
            //             where mt.TempName == "EmployeeInfo"
            //             select new
            //             {
            //                 Template = mt,
            //                 Field = tf,
            //                 Value = tv
            //             };


            //var completedTasksCount = _taskdbconnection.Tasks
            //    .Where(t => t.IsCompleted)
            //    .GroupBy(t => new { t.ManagerId, t.AssignedTo, Month = t.ComplitionDate.Month, Year = t.ComplitionDate.Year })
            //    .Select(g => new
            //    {
            //        ManagerId = g.Key.ManagerId,
            //        Employee = g.Key.AssignedTo,
            //        Month = g.Key.Month,
            //        Year = g.Key.Year,
            //        CompletedTaskCount = g.Count()
            //    })
            //    .OrderBy(r => r.ManagerId)
            //    .ThenBy(r => r.Year)
            //    .ThenBy(r => r.Month)
            //    .ToList();
            var data = await _taskdbconnection.Tasks.Where(x => x.AssignedTo == _sessionData.UserEmail).ToListAsync();
            return data;
        }
      
        public async Task<object> UploadDocument(IFormFile File, bool IsComplited, string Fileupload, int TaskId)
        {
            _uploadPath = @"D:\TaskManagementSystem\TaskManagementSystem\TaskManagementSystem\Documents";
            if (File == null || File.Length == 0)
                return "No file uploaded";

            // Combine the upload path with the file name
            var filePath = Path.Combine(_uploadPath,File.FileName);

            // Copy the file to the specified path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await File.CopyToAsync(stream);
            }

            TaskModel T1 = await _taskdbconnection.Tasks.SingleOrDefaultAsync(x=>x.TaskId ==TaskId);
            if(T1!=null)
            {
               T1.IsCompleted = IsComplited;
                T1.Uplodedfile = Fileupload;
                T1.ComplitionDate = DateTime.Now;
                await _taskdbconnection.SaveChangesAsync();
            }

            return $"File uploaded successfully";

        }

     

    }
}
