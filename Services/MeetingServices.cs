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
        public async Task<bool> AddMeetingNotesAsync(MeetingNotesModel newMeetingNotes)
        {
            if (newMeetingNotes == null)
            {
                return false;
            }

            // Retrieve the meeting by ID
            var meeting = await _dataContext.Meetings
                .Include(m => m.MeetingNotes)
                .FirstOrDefaultAsync(m => m.Id == newMeetingNotes.MeetingId);

            if (meeting == null)
            {
                return false; // If the meeting doesn't exist, return false
            }

            // Add the new notes to the meeting's notes collection
            meeting.MeetingNotes.Add(newMeetingNotes);

            // Save changes to the database
            await _dataContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditMeetingAsync(MeetingsModel newMeeting)
        {
            if (newMeeting == null)
            {
                return false;
            }
            MeetingsModel exisintMeeting = await GetMeetingByIdAsync(newMeeting.Id);
            exisintMeeting.LastContactMethod = newMeeting.LastContactMethod;
            exisintMeeting.PreferedContact = newMeeting.PreferedContact;
            exisintMeeting.LastDateContacted = newMeeting.LastDateContacted;
            exisintMeeting.GrantName = newMeeting.GrantName;
            exisintMeeting.RecruiterName = newMeeting.RecruiterName;
            exisintMeeting.NumOfContacts = newMeeting.NumOfContacts;
            _dataContext.Meetings.Update(exisintMeeting);
            await _dataContext.SaveChangesAsync();

            return true;
        }

        public async Task<MeetingsModel?> GetMeetingByIdAsync(int id)
        {
            return await _dataContext.Meetings.Include(m => m.MeetingNotes).FirstOrDefaultAsync(meetings => meetings.Id == id);
        }

        public async Task<IEnumerable<MeetingNotesModel>> GetMeetingNotesByMeetingIdAsync(int meetingId)
        {
            return await _dataContext.MeetingNotes
                .Where(meetingnotes => meetingnotes.MeetingId == meetingId)
                .ToListAsync();
        }
    }
}
