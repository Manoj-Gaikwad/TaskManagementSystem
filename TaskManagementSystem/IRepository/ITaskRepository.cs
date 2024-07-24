using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.IRepository
{
    public interface ITaskRepository
    {
      Task<List<TaskModel>> GetAllTasks();
      Task<string> CreateTask(TaskModel t1);
    }
}
