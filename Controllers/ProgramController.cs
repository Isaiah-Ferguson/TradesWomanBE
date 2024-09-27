using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradesWomanBE.Models;
using TradesWomanBE.Services;

namespace TradesWomanBE.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProgramController : ControllerBase
    {

        private readonly ProgramServices _programServices;

        public ProgramController(ProgramServices programServices)
        {
            _programServices = programServices;
        }

        [HttpPost("AddProgram")]
        public async Task<IActionResult> AddProgram(ProgramModel newStipend)
        {
            var success = await _programServices.AddProgramAsync(newStipend);

            if (!success)
            {
                return BadRequest("Program data is null or invalid.");
            }

            return CreatedAtAction(nameof(GetProgram), new { id = newStipend.Id }, newStipend);
        }
        [HttpPut("EditProgram/{id}")]
        public async Task<IActionResult> EditProgram(ProgramModel programToEdit)
        {
            var success = await _programServices.EditProgramAsync(programToEdit);

            if (!success)
            {
                return BadRequest("Program data is null or invalid.");
            }

            return CreatedAtAction(nameof(GetProgram), new { id = programToEdit.Id }, programToEdit);
        }
        [HttpGet("GetProgram/{id}")]
        public async Task<IActionResult> GetProgram(int id)
        {
            var program = await _programServices.GetProgramByIdAsync(id);

            if (program == null)
            {
                return NotFound();
            }

            return Ok(program);
        }

        [HttpPost("AddStipend")]
        public async Task<IActionResult> AddStipend(StipendsModel newStipend)
        {
            var success = await _programServices.AddStipendAsync(newStipend);

            if (!success)
            {
                return BadRequest("Stipend data is null or invalid.");
            }

            return CreatedAtAction(nameof(GetStipend), new { id = newStipend.Id }, newStipend);
        }
        
        [HttpPut("EditStipend/{id}")]
        public async Task<IActionResult> EditStipend(StipendsModel stipendToEdit)
        {
            var success = await _programServices.EditStipendAsync(stipendToEdit);

            if (!success)
            {
                return BadRequest("Stipend data is null or invalid.");
            }

            return CreatedAtAction(nameof(GetStipend), new { id = stipendToEdit.Id }, stipendToEdit);
        }
        [HttpGet("GetStipend/{id}")]
        public async Task<IActionResult> GetStipend(int id)
        {
            var stipend = await _programServices.GetStipendByIdAsync(id);

            if (stipend == null)
            {
                return NotFound();
            }

            return Ok(stipend);
        }
    }
}