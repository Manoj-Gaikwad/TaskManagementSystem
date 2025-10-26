using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem;
using TaskManagementSystem.IRepository;
using TaskManagementSystem.Models;
using TaskManagementSystem.Repository;
using TaskManagementSystem.StatupExtentions;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.AddSerilogLogging(builder.Configuration);


// Services
builder.Services.AddDbContext<TaskManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Authentication and Identity
builder.Services.AddJwtAuthentication(builder.Configuration);


// Dependency Injection
builder.Services.AddApplicationServices();

builder.Services.AddHttpContextAccessor();

// CORS
builder.Services.AddCustomCorsPolicy();

// Controllers
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

//Swagger configuration
builder.Services.AddCustomSwagger();



var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManagementSystem v1"));
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSerilogRequestLogging();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed roles
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await CreateRoles(serviceProvider);
}

app.Run();

// Role creation method
async Task CreateRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Admin", "Manager", "Employee" };

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}
