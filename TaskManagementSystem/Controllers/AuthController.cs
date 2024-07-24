using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using TaskManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;
using TaskManagementSystem.Repository;
using TaskManagementSystem.IRepository;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ISessionData _sessionData;
        private readonly TaskManagementDbContext _dbContext;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, ISessionData sessionData, TaskManagementDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _sessionData = sessionData;
            _dbContext = dbContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (model.ManagerId==null)
            {
                model.ManagerId = 0;
            }
            var user = new ApplicationUser
            {
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber=model.PhoneNumber,
                DOB = model.DOB,
                Address = model.Address,
                Department = model.Department,
                Role=model.Role,
                ManagerId = model.ManagerId,

            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Assign role to user
                var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
                if (!roleResult.Succeeded)
                {
                    return BadRequest(roleResult.Errors);
                }

                if (model.Role.Equals("Manager", StringComparison.OrdinalIgnoreCase))
                {
                    var manager = new Manager
                    {
                        ManagerId = model.ManagerId,
                        ManagerName = $"{model.FirstName} {model.LastName}",
                        Email = model.Email,
                        Department = model.Department // You might want to get this from the model or some other source
                    };
                    // Save the manager to the database
                    _dbContext.Manager.Add(manager);
                    await _dbContext.SaveChangesAsync();
                }
                return Ok(new { Result = "User created successfully" });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                this._sessionData.UserEmail = user.Email;
                Console.WriteLine($"UserEmail set to: {_sessionData.UserEmail}");
                var token = await GenerateJwtToken(user);
                var roles = await _userManager.GetRolesAsync(user);
                var rolesString = string.Join(", ", roles);
                var logres = new LoginResponse
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Token = token,
                    Roles = rolesString


                };
                return Ok(logres);
            }

            return Unauthorized();
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    };

            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
