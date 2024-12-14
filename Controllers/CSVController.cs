using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradesWomanBE.Services;
using System.IO;
using System.Threading.Tasks;

namespace TradesWomanBE.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CSVController : ControllerBase
    {
        private readonly CSVServices _csvServices;

        public CSVController(CSVServices csvServices)
        {
            _csvServices = csvServices;
        }

        [HttpGet("ExportClients")]
        public IActionResult ExportClients()
        {
            var csv = _csvServices.GetClientsAsCsv();
            var bytes = Encoding.UTF8.GetBytes(csv);
            var stream = new MemoryStream(bytes);

            return File(stream, "text/csv", "clients.csv");
        }

        [HttpGet("ExportClientsByDate/{startDate}/{endDate}")]
        public IActionResult ExportClientsByDate(string startDate, string endDate)
        {
            var csv = _csvServices.GetClientsAsCsvByDate(startDate, endDate);
            var bytes = Encoding.UTF8.GetBytes(csv);
            var stream = new MemoryStream(bytes);

            return File(stream, "text/csv", "clients.csv");
        }

        [HttpGet("ExportClientsByProgram/{program}")]
        public IActionResult ExportClientsByProgram(string program)
        {
            var csv = _csvServices.GetClientsAsCsvByProgram(program);
            var bytes = Encoding.UTF8.GetBytes(csv);
            var stream = new MemoryStream(bytes);

            return File(stream, "text/csv", "clients.csv");
        }

        [HttpGet("ExportClientsByProgram/{program}/{startDate}/{endDate}")]
        public IActionResult ExportClientsByProgramAndDate(string program, string startDate, string endDate)
        {
            var csv = _csvServices.GetClientsAsCsvByProgramAndDate(program, startDate, endDate);
            var bytes = Encoding.UTF8.GetBytes(csv);
            var stream = new MemoryStream(bytes);

            return File(stream, "text/csv", "clients.csv");
        }
        [HttpPost("ImportClientsFromCsv")]
    public async Task<IActionResult> ImportClientsFromCsv([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Please upload a valid CSV file.");
        }

        try
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0; // Reset the stream position for reading

                var clients = await _csvServices.ImportClientsFromCsvAsync(stream);

                // If you want to save clients to the database, do it here
                await _csvServices.SaveClientsToDatabaseAsync(clients);

                return Ok(new
                {
                    Message = "Clients imported successfully.",
                    ClientCount = clients.Count
                });
            }
        }
        catch (Exception ex)
        {
            // Log the error (not shown here)
            return StatusCode(500, $"Internal server error: {ex.Message} This is the File Name {file.FileName}, Size: {file.Length}");
        }
    }

    }
}