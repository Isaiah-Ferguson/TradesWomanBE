using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradesWomanBE.Models;
using TradesWomanBE.Models.DTO;
using TradesWomanBE.Services;
using TradesWomanBE.Services.Context;

namespace TradesWomanBE.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserServices _userService;
        private readonly DataContext _context;

        public UserController(UserServices userService, DataContext context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO user)
        {
            var result = await _userService.LoginAsync(user);
            if (result == null)
            {
                return Unauthorized(new { Message = "Invalid username or password." });
            }
            return Ok(result);
        }

        [HttpPost("CreateAdmin")]
        public async Task<bool> CreateAdmin(AdminUser newAccount)
        {
            return await _userService.CreateAdmin(newAccount);
        }

        [HttpPost("ChangeAdminPassword")]
        [Authorize]
        public async Task<IActionResult> ChangeAdminPassword(CreateAccountDTO account)
        {
            var success = await _userService.ChangeAdminPasswordAsync(account);
            if (!success)
            {
                return BadRequest("Unable to change admin password. Please check the provided data.");
            }
            return Ok(true);
        }
        [HttpPost("ChangeRecruiterPassword")]
        [Authorize]
        public async Task<IActionResult> ChangeRecruiterPassword(CreateAccountDTO account)
        {
            var success = await _userService.ChangeRecruiterPasswordAsync(account);
            if (!success)
            {
                return BadRequest("Unable to change recruiter password. Please check the provided data.");
            }
            return Ok(true);
        }

        [HttpPost("AddRecruiter")]
        [Authorize]
        public async Task<IActionResult> AddRecruiter([FromBody] RecruiterModel newAccount)
        {
            newAccount.IsDeleted = false;
            var success = await _userService.AddRecruiterAsync(newAccount);
            if (!success)
            {
                return BadRequest("Unable to add recruiter. Please check the provided data.");
            }
            return Ok(true);
        }

        [HttpGet("GetAllRecruiters")]
        [Authorize]
        public async Task<IActionResult> GetAllRecruiters()
        {
            var recruiters = await _userService.GetAllRecruitersAsync();
            return Ok(recruiters);
        }

        [HttpGet("GetRecruiterByEmail/{email}")]
        [Authorize]
        public async Task<IActionResult> GetRecruiterbyEmail(string email)
        {
            var recruiters = await _userService.GetRecruiterByEmailAsync(email);
            return Ok(recruiters);
        }

        [HttpPut("EditRecruiter")]
        [Authorize]
        public async Task<IActionResult> UpdateRecruiter([FromBody] RecruiterModel recruiterinfo)
        {
            var success = await _userService.UpdateRecruiterAsync(recruiterinfo);
            if (!success)
            {
                return BadRequest("Unable to edit Recruiter. Please check the provided data.");
            }
            return Ok("Recruiter edited successfully.");
        }
        [HttpPost]
        [Route("Admin/ClearAdminUsers")]
        public IActionResult ClearAdminUsers()
        {
            _context.AdminUsers.RemoveRange(_context.AdminUsers);
            _context.SaveChanges();
            return Ok("All AdminUser records cleared.");
        }

    }
}