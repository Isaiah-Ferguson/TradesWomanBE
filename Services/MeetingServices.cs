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

            if (await DoesMeetingExistAsync(newMeeting.Id))
            {
                await EditMeetingAsync(newMeeting);
            }
            else
            {
                var client = await _dataContext.ClientInfo
                    .Include(c => c.ProgramInfo)
                    .Include(c => c.Meetings) // Single Meeting
                    .FirstOrDefaultAsync(item => item.Id == newMeeting.ClientID);

                if (client != null)
                {
                    client.Meetings = newMeeting; // Assign newMeeting to the single `Meetings` property
                    _dataContext.ClientInfo.Update(client);
                    await _dataContext.SaveChangesAsync();
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> AddMeetingNotesAsync(MeetingNotesModel newMeetingNotes)
        {
            if (newMeetingNotes == null)
            {
                return false;
            }

            var meeting = await _dataContext.Meetings
                .Include(m => m.MeetingNotes)
                .FirstOrDefaultAsync(m => m.Id == newMeetingNotes.MeetingId);

            if (meeting == null)
            {
                return false;
            }

            meeting.MeetingNotes.Add(newMeetingNotes);

            await _dataContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditMeetingAsync(MeetingsModel newMeeting)
        {
            if (newMeeting == null)
            {
                return false;
            }

            // Get existing meeting by id
            MeetingsModel exisintMeeting = await GetMeetingByIdAsync(newMeeting.Id);

            // Check if the existing meeting was found
            if (exisintMeeting == null)
            {
                return false;  // Handle case when meeting doesn't exist
            }

            // Update properties
            exisintMeeting.LastContactMethod = newMeeting.LastContactMethod;
            exisintMeeting.PreferedContact = newMeeting.PreferedContact;
            exisintMeeting.LastDateContacted = newMeeting.LastDateContacted;
            exisintMeeting.GrantName = newMeeting.GrantName;
            exisintMeeting.RecruiterName = newMeeting.RecruiterName;
            exisintMeeting.NumOfContacts = newMeeting.NumOfContacts;

            // Update the entity in the data context
            _dataContext.Meetings.Update(exisintMeeting);
            await _dataContext.SaveChangesAsync();

            return true;
        }


        public async Task<MeetingsModel?> GetMeetingByIdAsync(int id)
        {
            return await _dataContext.Meetings.Include(m => m.MeetingNotes).FirstOrDefaultAsync(meetings => meetings.Id == id);
        }

        public async Task<bool> DoesMeetingExistAsync(int id)
        {
            return await _dataContext.Meetings
                .Include(m => m.MeetingNotes)
                    .AnyAsync(meetings => meetings.Id == id);
        }

        public async Task<IEnumerable<MeetingNotesModel>> GetMeetingNotesByMeetingIdAsync(int meetingId)
        {
            return await _dataContext.MeetingNotes
                .Where(meetingnotes => meetingnotes.MeetingId == meetingId)
                    .ToListAsync();
        }
    }
}
