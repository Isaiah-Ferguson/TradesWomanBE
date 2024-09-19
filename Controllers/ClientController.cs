using Microsoft.AspNetCore.Mvc;
using TradesWomanBE.Models;
using TradesWomanBE.Services;
using System.Text;

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
        public async Task<IActionResult> AddClient(ClientModel newClient)
        {
            newClient.IsDeleted = false;
            var success = await _clientServices.AddClientAsync(newClient);
            if (!success)
            {
                return BadRequest("Unable to add client. Please check the provided data.");
            }
            return Ok("Client added successfully.");
        }

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

        [HttpGet("GetAllClients")]
        public async Task<IActionResult> GetAllClients()
        {
            var clients = await _clientServices.GetAllClientsAsync();
            return Ok(clients);
        }

        [HttpGet("GetLast30Clients")]
        public async Task<IActionResult> GetLast30Clients()
        {
            var clients = await _clientServices.GetLast30ClientsAsync();
            return Ok(clients);
        }

        [HttpGet("GetClientByEmail/{email}")]
        public async Task<IActionResult> GetClientByEmail(string email)
        {
            var clients = await _clientServices.GetClientByEmailAsync(email);
            return Ok(clients);
        }

        [HttpGet("GetClientsByFirstNameAndLastname/{firstName}/{lastName}")]
        public async Task<IActionResult> GetClientsByFirstNameAndLastname(string firstName, string lastName)
        {
            var clients = await _clientServices.GetClientsByFirstNameAndLastnameAsync(firstName, lastName);
            return Ok(clients);
        }

        [HttpGet("ExportClients")]
        public IActionResult ExportClients()
        {
            var csv = _clientServices.GetClientsAsCsv();
            var bytes = Encoding.UTF8.GetBytes(csv);
            var stream = new MemoryStream(bytes);

            return File(stream, "text/csv", "clients.csv");
        }
    }
}
