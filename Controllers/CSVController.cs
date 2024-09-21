using System.Text;
using Microsoft.AspNetCore.Mvc;
using TradesWomanBE.Services;

namespace TradesWomanBE.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
    }
}