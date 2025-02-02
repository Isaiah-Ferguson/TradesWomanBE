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
        public async Task<IActionResult> AddProgram([FromBody]ProgramModel newStipend)
        {
            var success = await _programServices.AddProgramAsync(newStipend);

            if (!success)
            {
                return BadRequest("Program data is null or invalid.");
            }

            return CreatedAtAction(nameof(GetProgram), new { id = newStipend.Id }, newStipend);
        }
        [HttpPut("EditProgram")]
        public async Task<IActionResult> EditProgram([FromBody]ProgramModel programToEdit)
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
        public async Task<IActionResult> AddStipend([FromBody]StipendsModel newStipend)
        {
            var success = await _programServices.AddStipendAsync(newStipend);

            if (!success)
            {
                return BadRequest("Stipend data is null or invalid.");
            }

            return CreatedAtAction(nameof(GetStipend), new { id = newStipend.Id }, newStipend);
        }
        
        [HttpPut("EditStipend")]
        public async Task<IActionResult> EditStipend([FromBody]StipendsModel stipendToEdit)
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

        [HttpGet("GetProgramLookUp")]
        public async Task<IActionResult> GetProgramLookUp(){
            var programs = await _programServices.GetProgramLookUpAsync();
            return Ok(programs);
        }

        [HttpPost("AddProgramLookUpName")]
        public async Task<IActionResult> AddProgramLookUpName([FromBody]ProgramLookUpModel newProgramName)
        {
            var success = await _programServices.AddProgramLookUpAsync(newProgramName);
            if (!success)
            {
                return BadRequest("Unable to add Program Name. Please check the provided data. Program might exist in Database");
            }
            return Ok("Program added successfully.");
        }

        [HttpPut("EditProgramLookUpName")]
        public async Task<IActionResult> EditProgramLookUpName([FromBody]ProgramLookUpModel newProgramName)
        {
            var success = await _programServices.EditProgramLookUpAsync(newProgramName);
            if (!success)
            {
                return BadRequest("Unable to Edit Program Name. Please check the provided data.");
            }
            return Ok("Program added successfully.");
        }
        [HttpDelete("DeleteProgramLookUpName")]
        public async Task<IActionResult> DeleteProgramLookUpName([FromBody]ProgramLookUpModel newProgramName)
        {
            var success = await _programServices.DeleteProgramLookUpAsync(newProgramName);
            if (!success)
            {
                return BadRequest("Unable to Edit Program Name. Please check the provided data.");
            }
            return Ok("Program Deleted successfully.");
        }

    }
}