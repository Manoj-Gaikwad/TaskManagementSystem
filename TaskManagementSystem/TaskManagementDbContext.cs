using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TaskManagementSystem
{
    public class TaskManagementDbContext : IdentityDbContext<ApplicationUser>
    {
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options)
       : base(options)
        {
        }

        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<MasterTemplet> MasterTemplet{get;set; }
        public DbSet<TempletFields> TempletFields { get; set; }
        public DbSet<TempletValue> TempletValues { get; set; }
        public DbSet<Manager> Manager { get; set; }
        public DbSet<Roles> Roles { get; set; }
    }
}
