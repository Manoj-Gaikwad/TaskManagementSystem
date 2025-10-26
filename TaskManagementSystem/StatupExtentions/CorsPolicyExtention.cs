using Microsoft.Extensions.DependencyInjection;

namespace TaskManagementSystem.StatupExtentions
{
    public static class CorsPolicyExtention
    {
      public static IServiceCollection AddCustomCorsPolicy(this IServiceCollection services)
      {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyHeader()
                          .AllowAnyOrigin()
                          .AllowAnyMethod();


                });
            });

            return services;
      }
    }
}
