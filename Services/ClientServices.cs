using TradesWomanBE.Models;
using TradesWomanBE.Services.Context;
using Microsoft.EntityFrameworkCore;
using System.Text;


namespace TradesWomanBE.Services
{
    public class ClientServices
    {
        private readonly DataContext _dataContext;

        public ClientServices(DataContext dataContext, EmailServices emailService)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> DoesClientExistAsync(int? SSNLastFour, string? Firstname)
        {
            return await _dataContext.ClientInfo
                .SingleOrDefaultAsync(client => client.SSNLastFour == SSNLastFour && client.Firstname == Firstname) != null;
        }

        public async Task<IEnumerable<ClientModel>> GetLast30ClientsAsync()
        {
            return await _dataContext.ClientInfo
                .OrderByDescending(client => client.DateRegistered)
                .Take(30)
                .ToListAsync();
        }

        public async Task<bool> AddClientAsync(ClientModel clientModel)
        {
            if (!await DoesClientExistAsync(clientModel.SSNLastFour, clientModel.Firstname))
            {
                await _dataContext.AddAsync(clientModel);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> EditClientAsync(ClientModel clientModel)
        {
            if (!await DoesClientExistAsync(clientModel.SSNLastFour, clientModel.Firstname))
            {
                ClientModel client = await GetClientByEmailAsync(clientModel.Email);

                _dataContext.Update(clientModel);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> UpdateClientAsync(ClientModel clientToUpdate)
        {
            _dataContext.Update(clientToUpdate);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> UpdateProgramAsync(ProgramModel programToUpdate)
        {
            _dataContext.Update(programToUpdate);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> UpdateCTWIStipendAsync(CTWIStipendsModel stipendInfo)
        {
            _dataContext.Update(stipendInfo);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<IEnumerable<ClientModel>> GetAllClientsAsync()
        {
            return await _dataContext.ClientInfo.ToListAsync();
        }

        public async Task<ClientModel> GetClientByIdAsync(int userId)
        {
            return await _dataContext.ClientInfo
                .Include(c => c.ProgramInfo)
                    .Include(c => c.Meetings)
                        .ThenInclude(m => m.MeetingNotes)
                            .FirstOrDefaultAsync(item => item.Id == userId);
        }

        public async Task<ClientModel> GetClientByEmailAsync(string email)
        {
            return await _dataContext.ClientInfo
                .Include(c => c.ProgramInfo)
                    .Include(c => c.Meetings)
                        .ThenInclude(m => m.MeetingNotes)
                            .FirstOrDefaultAsync(item => item.Email == email);
        }

        public async Task<IEnumerable<ClientModel>> GetClientsByFirstNameAndLastnameAsync(string Firstname, string Lastname)
        {
            return await _dataContext.ClientInfo
                .Include(c => c.ProgramInfo)
                    .Include(c => c.Meetings)
                        .ThenInclude(m => m.MeetingNotes)
                            .Where(item => item.Firstname == Firstname && item.Lastname == Lastname)
                                .ToListAsync();
        }
        public string GetClientsAsCsv()
        {
            var clients = _dataContext.ClientInfo.Include(c => c.ProgramInfo).ToList();
            var sb = new StringBuilder();
            sb.AppendLine("FirstName,LastName,Email,ValidSSNAuthToWrk,CriminalHistory,Disabled,FoundUsOn,DateJoinedEAW,Stipends,Address,Gender,Employed,ProgramEnrolled,ProgramEndDate,CurrentStatus,GrantName");

            foreach (var client in clients)
            {
                var program = client.ProgramInfo;
                sb.AppendLine($"{client.Id},{client.Firstname},{client.Lastname},{client.SSNLastFour},{client.MiddleInnitial},{client.Email},{client.ValidSSNAuthToWrk},{client.CriminalHistory},{client.FoundUsOn},{client.DateJoinedEAW},{client.Stipends},{client.Address},{client.Gender},{client.Employed}" +
                      $"{program?.ProgramEnrolled},{program?.EnrollDate},{program?.ProgramEndDate}");
            }

            return sb.ToString();
        }

    }
}
