using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.IRepository
{
    public interface ITaskRepository
    {
      Task<List<TaskModel>> GetAllTasks();
      Task<Response> CreateTask(TaskModel t1);
      Task<object> GetManagerAndEmployeesEmailsAsync();
    }
}
