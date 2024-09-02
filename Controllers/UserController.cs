using Microsoft.AspNetCore.Mvc;
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
            return _userService.AdminLogin(user);
        }

        [HttpPost("AddUser")]
        public bool CreateAdmin(CreateAccountDTO newAccount)
        {
            return _userService.AddUser(newAccount);
        }

        [HttpPost("ChangeUserPassword")]
        public bool ChangeUserPassword(CreateAccountDTO newAccount)
        {
            return _userService.AddUser(newAccount);
        }
    
        [HttpPost("AddRecruiter")]
        public bool AddRecruiter(CreateAccountDTO newAccount)
        {
            return _userService.AddUser(newAccount);
        }
    }
}