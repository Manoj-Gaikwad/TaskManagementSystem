using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Models;
using TaskManagementSystem.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Microsoft.Extensions.Logging;



namespace TaskManagementSystem.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagementDbContext _taskdbconnection;
        private readonly  ISessionData _sessionData;
        private readonly ILogger<TaskRepository> _logger;
        public TaskRepository(TaskManagementDbContext taskdbconnection, ISessionData sessionData, ILogger<TaskRepository> logger)
        {
            this._taskdbconnection = taskdbconnection;
            this._sessionData = sessionData;
            this._logger=logger;
        }
        public async Task<List<TaskModel>> GetAllTasks()
        {
            var data= await _taskdbconnection.Tasks.Where(x => x.CreatedBy == _sessionData.UserEmail).ToListAsync();
            return data;
        }
        public async Task<TaskModel>GetTaskById(int Id)
        {
            var data=await _taskdbconnection.Tasks.FirstOrDefaultAsync(x=>x.TaskId == Id);
            return data;
        }
        public async Task<Response> CreateTask(TaskModel t1)
            {
            TaskModel task = new TaskModel()
            {
                //TaskId=t1.TaskId,
                Title = t1.Title,
                Description = t1.Description,
                AssignedTo = t1.AssignedTo,
                IsCompleted = t1.IsCompleted,
                CreationDate = t1.CreationDate,
                Uplodedfile = t1.Uplodedfile,
                ComplitionDate=t1.ComplitionDate,
                CreatedBy = t1.CreatedBy,
                ManagerId = t1.ManagerId,

            };
            if (task.Uplodedfile == null)
            {
                task.Uplodedfile = "No";
            }
            await _taskdbconnection.AddAsync(task);
            await _taskdbconnection.SaveChangesAsync();
            _logger.LogInformation($"Task Name:{t1.Title}, CreatedBy:{t1.CreatedBy} and Assignedto:{t1.AssignedTo}");

            Response r1= new Response();
            r1.Output = "Task Added Successfully";
            r1.StatusMessage = "success";
            return r1;
        }

        public async Task<string> UpdateTask(TaskModel t1)
        {
            var data = await this._taskdbconnection.Tasks.FirstOrDefaultAsync(x => x.TaskId == t1.TaskId);

            if (data != null)
            {
                data.Title = t1.Title;
                data.Description = t1.Description;
                data.AssignedTo = t1.AssignedTo;

                this._taskdbconnection.SaveChangesAsync();
                return "Task Updated Successfully";
            }
            else
            {
                return "Error In Task Update";
            }
        }

        public async Task<object> GetManagerAndEmployeesEmailsAsync()
        {
            // Get the manager's email from the session data
            var managerEmail = this._sessionData.UserEmail;

            // Retrieve the manager's user
            var manager = await this._taskdbconnection.Users
                .FirstOrDefaultAsync(x => x.Email == managerEmail);

            if (manager == null)
            {
                // Handle the case where the manager is not found
                return new { manager = (object)null, employees = new List<string>() };
            }

            // Retrieve all employees managed by the identified manager
            var employees = await this._taskdbconnection.Users
              .Where(x => x.ManagerId == manager.Id && x.Email != managerEmail &&x.Role=="Employee") // Exclude manager's email
              .Select(x => x.Email) // Select only the email of employees
              .ToListAsync();

            // Create the response object with manager details and employee emails
            var result = new
            {
                manager = new
                {
                    name = $"{manager.FirstName} {manager.LastName}",
                    email = manager.Email,
                    ManagerId= manager.Id,
                },
                employees = employees
            };

            return result;
        }


    }
}
