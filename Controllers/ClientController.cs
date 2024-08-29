using Microsoft.AspNetCore.Mvc;
using TradesWomanBE.Models;
using TradesWomanBE.Services;
using System.Threading.Tasks;

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

        // POST: /Client/AddClient
        [HttpPost("AddClient")]
        public async Task<IActionResult> AddClient(ClientModel newClient)
        {
            var success = await _clientServices.AddClientAsync(newClient);
            if (!success)
            {
                return BadRequest("Unable to add client. Please check the provided data.");
            }
            return Ok("Client added successfully.");
        }

        // PUT: /Client/EditClient
        [HttpPut("EditClient")]
        public async Task<IActionResult> EditClient(ClientModel clientToEdit)
        {
            var success = await _clientServices.EditClientAsync(clientToEdit);
            if (!success)
            {
                return BadRequest("Unable to edit client. Please check the provided data.");
            }
            return Ok("Client edited successfully.");
        }

        // GET: /Client/GetAllClients
        [HttpGet("GetAllClients")]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _clientServices.GetAllClientsAsync();
            return Ok(clients);
        }

    }
}
