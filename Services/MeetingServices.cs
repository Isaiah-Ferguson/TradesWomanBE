using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TradesWomanBE.Models;
using TradesWomanBE.Services.Context;

namespace TradesWomanBE.Services
{
    public class MeetingsServices
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public MeetingsServices(DataContext context, IMapper mapper)
        {
            _dataContext = context;
            _mapper = mapper;
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
                var client = await _dataContext.ClientInfo.FirstOrDefaultAsync(item => item.Id == newMeeting.ClientID);

                if (client != null)
                {
                    client.Meetings = newMeeting;
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

            var meeting = await _dataContext.Meetings.FirstOrDefaultAsync(m => m.Id == newMeetingNotes.MeetingId);

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

            MeetingsModel exisintMeeting = await GetMeetingByIdAsync(newMeeting.Id);

            if (exisintMeeting == null)
            {
                return false; 
            }

            _mapper.Map(newMeeting, exisintMeeting);
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
