using Microsoft.EntityFrameworkCore;
using TradesWomanBE.Models;
using TradesWomanBE.Services.Context;

namespace TradesWomanBE.Services
{
    public class MeetingsServices
    {
        private readonly DataContext _dataContext;

        public MeetingsServices(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<bool> AddMeetingAsync(MeetingsModel newMeeting)
        {
            if (newMeeting == null)
            {
                return false;
            }

            _dataContext.Meetings.Add(newMeeting);
            await _dataContext.SaveChangesAsync();

            return true;
        }

        public async Task<MeetingsModel?> GetMeetingByIdAsync(int id)
        {
            return await _dataContext.Meetings.FindAsync(id);
        }
        public async Task<IEnumerable<MeetingNotesModel>> GetMeetingNotesByMeetingIdAsync(int meetingId)
        {
            return await _dataContext.MeetingNotes
                .Where(mn => mn.Meeting.Id == meetingId)
                .ToListAsync();
        }
    }
}
