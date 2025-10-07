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
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Logging;

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
        private readonly EmailService _emailService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, ISessionData sessionData, TaskManagementDbContext dbContext, EmailService emailService, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _sessionData = sessionData;
            _dbContext = dbContext;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (model.Role == "Employee")
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    DOB = model.DOB,
                    Address = model.Address,
                    Department = model.Department,
                    Role = model.Role,
                    ManagerId = model.ManagerId,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"Employee {model.FirstName} Added by ManagerID {model.ManagerId}");
                    string subject = "Your Account Details";
                    string body = $"<p>Dear {model.FirstName},</p><p>Your account has been successfully created. Here are your login details:</p>" +
                                  $"<p><b>Username:</b> {model.Email}<br><b>Password:</b> {model.Password}</p>";

                    await _emailService.SendEmailAsync(model.Email, subject, body);
                    // Assign role to user
                    var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
                    if (!roleResult.Succeeded)
                    {
                        return BadRequest(roleResult.Errors);
                    }
                    return Ok(new { Result = "User created successfully" });
                }

            }
            else
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    DOB = model.DOB,
                    Address = model.Address,
                    Department = model.Department,
                    Role = model.Role,
                    ManagerId = null,

                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign role to user
                    var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
                    string subject = "Your Account Details";
                    string body = $"<p>Dear {model.FirstName},</p><p>Your account has been successfully created. Here are your login details:</p>" +
                                  $"<p><b>Username:</b> {model.Email}<br><b>Password:</b> {model.Password}</p>";

                    await _emailService.SendEmailAsync(model.Email, subject, body);
                    if (!roleResult.Succeeded)
                    {
                        return BadRequest(roleResult.Errors);
                    }
                    return Ok(new { Result = "User created successfully" });
                }
            }
            return Ok(new { Result = "Error" });
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
            else
            {
                _logger.LogError($"Invalid password for user: {model.Email}");
                return Unauthorized();
               
            }
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
                expires: DateTime.Now.AddMinutes(90),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpGet("GetLoginUserDetails")]
        public async Task<ApplicationUser> GetLoginUserDetails(string Email)
        {
            var data = await this._dbContext.Users.FirstOrDefaultAsync(x => x.Email == Email);
            return data;
        }
        [HttpPost("UpdateUserInfo")]
        public async Task<List<ApplicationUser>> UpdateUserProfile(ApplicationUser applicationUser)
        {
            ApplicationUser applicationUser1 = await this._dbContext.Users.FirstOrDefaultAsync(x => x.Email == applicationUser.Email);

            if (applicationUser1 != null)
            {
                applicationUser1.Email = applicationUser.Email;
                applicationUser1.FirstName = applicationUser.FirstName;
                applicationUser1.LastName = applicationUser.LastName;
                applicationUser1.DOB = applicationUser.DOB;
                applicationUser1.Department = applicationUser.Department;
                applicationUser1.PhoneNumber = applicationUser.PhoneNumber;
                applicationUser1.Address = applicationUser.Address;
                await this._dbContext.SaveChangesAsync();
                var updatedUsers = await this._dbContext.Users.Where(x => x.Email == applicationUser.Email).ToListAsync();
                return updatedUsers;
            }
            else
            {
                return null;
            }

        }
        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Password updated successfully" });
            }

            return BadRequest(result.Errors);
        }

    }


}
