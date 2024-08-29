using Microsoft.AspNetCore.Mvc;
using TradesWomanBE.Models;
using TradesWomanBE.Services;

namespace TradesWomanBE.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MeetingsController : ControllerBase
    {
        private readonly MeetingsServices _meetingsService;

        public MeetingsController(MeetingsServices meetingsService)
        {
            _meetingsService = meetingsService;
        }

        [HttpPost("AddMeeting")]
        public async Task<IActionResult> AddMeeting(MeetingsModel newMeeting)
        {
            var success = await _meetingsService.AddMeetingAsync(newMeeting);
            
            if (!success)
            {
                return BadRequest("Meeting data is null or invalid.");
            }

            return CreatedAtAction(nameof(GetMeeting), new { id = newMeeting.Id }, newMeeting);
        }

        [HttpGet("GetMeeting/{id}")]
        public async Task<IActionResult> GetMeeting(int id)
        {
            var meeting = await _meetingsService.GetMeetingByIdAsync(id);

            if (meeting == null)
            {
                return NotFound();
            }

            return Ok(meeting);
        }

        [HttpGet("GetMeetingNotes/{meetingId}")]
        public async Task<IActionResult> GetMeetingNotes(int meetingId)
        {
            var notes = await _meetingsService.GetMeetingNotesByMeetingIdAsync(meetingId);
            if (notes == null || !notes.Any())
            {
                return NotFound();
            }
            return Ok(notes);
        }
    }
}