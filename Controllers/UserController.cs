using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

        
        // [HttpPost("Login")]
        // public IActionResult Login()
        // {
            
        // }

        // [HttpPost("AddUser")]
        // public bool AddClient()
        // {

        // }

        // [HttpPost("CreateRecruiter")]
        // public bool CreateRecruiter()
        // {
            
        // }

        
    }
}