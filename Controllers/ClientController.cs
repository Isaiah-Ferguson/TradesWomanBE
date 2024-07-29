using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TradesWomanBE.Services;
using TradesWomanBE.Services.Interfaces;

namespace TradesWomanBE.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : ControllerBase
    {


        private readonly ClientServices _clientServices;

        public ClientController(ClientServices clientServices)
        {
            _clientServices = clientServices;
        }

        [HttpPost("AddClient")]
        public bool AddClient(ClientServices newClient)
        {
            return _data.AddClient(newClient);
        }
    }
}