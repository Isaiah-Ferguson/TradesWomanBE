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
        public IActionResult Login([FromBody] LoginDTO user)
        {
            return _userService.Login(user);
        }

        [HttpPost("CreateAdmin")]
        public async Task<bool> CreateAdmin(AdminUser newAccount)
        {
            return await _userService.CreateAdmin(newAccount);
        }

        [HttpPost("ChangeAdminPassword")]
        [Authorize]
        public bool ChangeAdminPassword(CreateAccountDTO account)
        {
            return _userService.ChangeAdminPassword(account);
        }
        [HttpPost("ChangeRecruiterPassword")]
        [Authorize]
        public bool ChangeRecruiterPassword(CreateAccountDTO account)
        {
            return _userService.ChangeRecruiterPassword(account);
        }

        [HttpPost("AddRecruiter")]
        [Authorize]
        public bool AddRecruiter([FromBody] RecruiterModel newAccount)
        {
            newAccount.IsDeleted = false;
            return _userService.AddRecruiter(newAccount);
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