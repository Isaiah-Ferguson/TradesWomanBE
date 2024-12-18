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


        [HttpPost("ImportFromCsv")]
        public async Task<IActionResult> ImportFromCsv([FromForm] IFormFile file, [FromQuery] string entityType)
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
                    stream.Position = 0;

                    switch (entityType.ToLower())
                    {
                        case "clients":
                            var clients = await _csvServices.ImportClientsFromCsvAsync(stream);
                            await _csvServices.SaveClientsToDatabaseAsync(clients);
                            return Ok(new { Message = "Programs imported successfully.", ClientCount = clients.Count });

                        case "programs":
                            await _csvServices.ImportProgramsFromCsvAsync(stream);
                            return Ok(new { Message = "Programs imported successfully." });

                        case "stipends":
                            await _csvServices.ImportStipendsFromCsvAsync(stream);
                            return Ok(new { Message = "Stipends imported successfully." });

                        case "meetings":
                            await _csvServices.ImportMeetingsFromCsvAsync(stream);
                            return Ok(new { Message = "Meetings imported successfully." });

                        case "meetingnotes":
                            await _csvServices.ImportMeetingsNotesFromCsvAsync(stream);
                            return Ok(new { Message = "Meeting Notes imported successfully." });

                        default:
                            return BadRequest("Invalid entity type specified. Please use 'programs', 'stipends', 'meetings', 'meetingnotes', or 'clients'.");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}