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


namespace TaskManagementSystem.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagementDbContext _taskdbconnection;
        private readonly  ISessionData _sessionData;

        public TaskRepository(TaskManagementDbContext taskdbconnection, ISessionData sessionData)
        {
            this._taskdbconnection = taskdbconnection;
            this._sessionData = sessionData;
        }
        public async Task<List<TaskModel>> GetAllTasks()
        {
            var data= await _taskdbconnection.Tasks.Where(x => x.CreatedBy == _sessionData.UserEmail).ToListAsync();
            return data;
        }
        public async Task<string> CreateTask(TaskModel t1)
        {
            TaskModel task = new TaskModel()
            {
                TaskId=t1.TaskId,
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
            await _taskdbconnection.AddAsync(task);
            await _taskdbconnection.SaveChangesAsync();
            return "Task Added Successfully";
        }
    }
}
