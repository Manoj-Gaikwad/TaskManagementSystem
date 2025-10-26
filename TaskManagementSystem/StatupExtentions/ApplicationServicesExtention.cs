using Microsoft.Extensions.DependencyInjection;
using TaskManagementSystem.IRepository;
using TaskManagementSystem.Repository;

namespace TaskManagementSystem.StatupExtentions
{
    public static class ApplicationServicesExtention
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services
                .AddTransient<ITaskRepository, TaskRepository>()
                .AddTransient<IEmployeeRepository, EmployeeRepository>()
                .AddTransient<IAdminRepository, AdminRepository>()
                .AddSingleton<ISessionData, SessionData>()
                .AddScoped<EmailService>();
                

        }
    }
}
