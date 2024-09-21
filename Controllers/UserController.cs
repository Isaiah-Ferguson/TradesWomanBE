using Microsoft.AspNetCore.Mvc;
using TradesWomanBE.Models;
using TradesWomanBE.Models.DTO;
using TradesWomanBE.Services;

namespace TradesWomanBE.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserServices _userService;

        public UserController(UserServices userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody]LoginDTO user)
        {
            return _userService.Login(user);
        }

        [HttpPost("CreateAdmin")]
        public bool CreateAdmin(CreateAccountDTO newAccount)
        {
            return _userService.CreateAdmin(newAccount);
        }

        [HttpPost("ChangeUserPassword")]
        public bool ChangeUserPassword(CreateAccountDTO account)
        {
            return _userService.ChangePassword(account);
        }
    
        [HttpPost("AddRecruiter")]
        public bool AddRecruiter(RecruiterModel newAccount)
        {
            newAccount.IsDeleted = false;
            return _userService.AddRecruiter(newAccount);
        }
        
        [HttpGet("GetAllRecruiters")]
        public async Task<IActionResult> GetAllRecruiters()
        {
            var recruiters = await _userService.GetAllRecruitersAsync();
            return Ok(recruiters);
        }

        [HttpGet("GetRecruiterByEmail/{email}")]
        public async Task<IActionResult> GetRecruiterbyEmail(string email)
        {
            var recruiters = await _userService.GetRecruiterByEmailAsync(email);
            return Ok(recruiters);
        }

        [HttpPut("EditRecruiter")]
        public async Task<IActionResult> UpdateRecruiter(RecruiterModel recruiterinfo)
        {
            var success = await _userService.UpdateRecruiterAsync(recruiterinfo);
                if (!success)
            {
                return BadRequest("Unable to edit Recruiter. Please check the provided data.");
            }
            return Ok("Recruiter edited successfully.");
        }
    }
}