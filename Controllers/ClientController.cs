using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradesWomanBE.Models;
using TradesWomanBE.Services;

namespace TradesWomanBE.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
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
                return BadRequest("Unable to add client. Please check the provided data. SSN might exist in Database");
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

        [HttpPut("DeleteClient")]
        public async Task<IActionResult> DeleteClient(ClientModel clientToDelete)
        {
            var success = await _clientServices.DeleteClientAsync(clientToDelete);
            if (!success)
            {
                return BadRequest("Unable to delete client. Please check the provided data.");
            }
            return Ok("Client Deleted successfully.");
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
        [HttpGet("GetClientById/{id}")]
        public async Task<IActionResult> GetClientByid(int id)
        {
            var clients = await _clientServices.GetClientByIdAsync(id);
            return Ok(clients);
        }

        [HttpGet("GetClientsByFirstNameAndLastname/{firstName}/{lastName}")]
        public async Task<IActionResult> GetClientsByFirstNameAndLastname(string firstName, string lastName)
        {
            var clients = await _clientServices.GetClientsByFirstNameAndLastnameAsync(firstName, lastName);
            return Ok(clients);
        }

        [HttpGet("GetClientSummary")]
        public async Task<IActionResult> GetClientSummaries()
        {
            var clients = await _clientServices.GetAllClientsOnLoadAsync();

            if (clients == null || !clients.Any())
            {
                return NotFound("No clients found");
            }

            return Ok(clients);
        }
        [HttpGet("GetClientSummaryByRecruiter/{firstName}/{lastName}")]
        public async Task<IActionResult> GetClientSummarieByRecruiter(string firstName, string lastName)
        {
            string recruiterName = firstName + " " + lastName;
            
            var clients = await _clientServices.GetClientsSummaryByRecruiterNameAsync(recruiterName);

            if (clients == null || !clients.Any())
            {
                return NotFound("No clients found");
            }

            return Ok(clients);
        }

    }
}
