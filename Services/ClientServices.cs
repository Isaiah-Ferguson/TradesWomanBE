using TradesWomanBE.Models;
using TradesWomanBE.Services.Context;
using Microsoft.EntityFrameworkCore;


namespace TradesWomanBE.Services
{
    public class ClientServices
    {
        private readonly DataContext _dataContext;

        public ClientServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        private async Task<bool> DoesClientExistAsync(int? SSNLastFour)
        {
            return await _dataContext.ClientInfo
                .SingleOrDefaultAsync(client => client.SSNLastFour == SSNLastFour) != null;
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
            if (!await DoesClientExistAsync(clientModel.SSNLastFour))
            {
                await _dataContext.AddAsync(clientModel);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<bool> EditClientAsync(ClientModel clientModel)
        {
            if (await DoesClientExistAsync(clientModel.SSNLastFour))
            {
                ClientModel existingClient = await GetClientByEmailAsync(clientModel.Email);
                existingClient.Age = clientModel.Age;
                existingClient.Firstname = clientModel.Firstname;
                existingClient.Lastname = clientModel.Lastname;
                existingClient.MiddleInitial = clientModel.MiddleInitial;
                existingClient.Email = clientModel.Email;
                existingClient.ChildrenUnderSix = clientModel.ChildrenUnderSix;
                existingClient.ChildrenOverSix = clientModel.ChildrenOverSix;
                existingClient.TotalHouseholdFamily = clientModel.TotalHouseholdFamily;
                existingClient.SSNLastFour = clientModel.SSNLastFour;
                existingClient.ValidSSNAuthToWrk = clientModel.ValidSSNAuthToWrk;
                existingClient.CriminalHistory = clientModel.CriminalHistory;
                existingClient.Disabled = clientModel.Disabled;
                existingClient.FoundUsOn = clientModel.FoundUsOn;
                existingClient.DateJoinedEAW = clientModel.DateJoinedEAW;
                existingClient.Stipends = clientModel.Stipends;
                existingClient.Address = clientModel.Address;
                existingClient.City = clientModel.City;
                existingClient.State = clientModel.State;
                existingClient.ZipCode = clientModel.ZipCode;
                existingClient.Gender = clientModel.Gender;
                existingClient.Employed = clientModel.Employed;
                existingClient.RecruiterName = clientModel.RecruiterName;
                existingClient.DateRegistered = clientModel.DateRegistered;
                existingClient.DateOfBirth = clientModel.DateOfBirth;
                existingClient.ActiveOrFormerMilitary = clientModel.ActiveOrFormerMilitary;
                existingClient.TotalMonthlyIncome = clientModel.TotalMonthlyIncome;
                existingClient.PhoneNumber = clientModel.PhoneNumber;
                existingClient.ProgramInfoId = clientModel.ProgramInfoId;
                existingClient.IsDeleted = clientModel.IsDeleted;

                _dataContext.Update(existingClient);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            return false;
        }



        public async Task<IEnumerable<ClientModel>> GetAllClientsAsync()
        {
            return await _dataContext.ClientInfo
            .Include(c => c.ProgramInfo)
                    .Include(c => c.Stipends)
                        .Include(c => c.Meetings)
                            .ThenInclude(m => m.MeetingNotes).ToListAsync();
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
    }
}
