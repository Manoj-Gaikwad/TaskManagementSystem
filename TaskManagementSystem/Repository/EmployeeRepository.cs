﻿using Microsoft.AspNetCore.Http;
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
        public async Task<Response> DeleteRecord(string email)
        {
            var data = this._taskdbconnection.Users.SingleOrDefaultAsync(x => x.Email == email);
            Response r1 = new Response();
            if (data.Result != null)
            {
                _taskdbconnection.Remove(data.Result);
                await this._taskdbconnection.SaveChangesAsync();

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
