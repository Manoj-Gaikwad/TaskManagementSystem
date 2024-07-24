using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.IRepository
{
    public interface IAdminRepository
    {
        Task<List<Admin>> MonthWisePerformance();
    }
}
