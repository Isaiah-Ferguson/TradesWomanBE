using TradesWomanBE.Models;
using TradesWomanBE.Services.Context;
using Microsoft.EntityFrameworkCore;
using AutoMapper;


namespace TradesWomanBE.Services
{
    public class ClientServices
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public ClientServices(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        private async Task<bool> DoesClientExistAsync(int? id)
        {
            return await _dataContext.ClientInfo
                .SingleOrDefaultAsync(client => client.Id == id) != null;
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

        // public async Task<bool> EditClientAsync(ClientModel clientModel)
        // {
        //     if (await DoesClientExistAsync(clientModel.Id))
        //     {
        //         ClientModel existingClient = await GetClientByIdAsync(clientModel.Id);

        //         _mapper.Map(clientModel, existingClient);

        //         _dataContext.Update(existingClient);
        //         return await _dataContext.SaveChangesAsync() > 0;
        //     }
        //     return false;
        // }

        public async Task<bool> EditClientAsync(ClientModel clientModel)
        {
            if (await DoesClientExistAsync(clientModel.Id))
            {
                // Get the existing client record from the database
                ClientModel existingClient = await GetClientByIdAsync(clientModel.Id);

                // Update only the required fields (excluding ProgramInfo, Meetings, Stipends, and Id)
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
                existingClient.HighestEducation = clientModel.HighestEducation;
                existingClient.ValidCALicense = clientModel.ValidCALicense;
                existingClient.County = clientModel.County;
                existingClient.Ethnicity = clientModel.Ethnicity;

                // Save changes to the database
                _dataContext.Update(existingClient);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            return false;
        }


        public async Task<bool> DeleteClientAsync(ClientModel clientModel)
        {
            if (await DoesClientExistAsync(clientModel.Id))
            {
                ClientModel existingClient = await GetClientByIdAsync(clientModel.Id);

                existingClient.IsDeleted = clientModel.IsDeleted;

                _dataContext.Update(existingClient);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            return false;
        }


        public async Task<IEnumerable<ClientModel>> GetAllClientsAsync()
        {
            return await _dataContext.ClientInfo
            .Where(c => c.IsDeleted == false)
                .Include(c => c.ProgramInfo)
                    .Include(c => c.Stipends)
                        .Include(c => c.Meetings)
                            .ThenInclude(m => m.MeetingNotes).ToListAsync();
        }
        public async Task<IEnumerable<object>> GetAllClientsOnLoadAsync()
        {
            return await _dataContext.ClientInfo
                .Where(c => c.IsDeleted == false)
                .Select(c => new
                {
                    c.Id,
                    c.Firstname,
                    c.Lastname,
                    c.Email,
                    c.RecruiterName
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<object>> GetClientsSummaryByRecruiterNameAsync(string recruitername)
        {
            return await _dataContext.ClientInfo
                .Where(c => c.IsDeleted == false && c.RecruiterName == recruitername)
                .Select(c => new
                {
                    c.Id,
                    c.Firstname,
                    c.Lastname,
                    c.Email,
                    c.RecruiterName
                })
                .ToListAsync();
        }

        public async Task<ClientModel> GetClientByIdAsync(int userId)
        {
            return await _dataContext.ClientInfo
            .Where(c => c.IsDeleted == false)
                .Include(c => c.ProgramInfo)
                    .Include(c => c.Stipends)
                        .Include(c => c.Meetings)
                            .ThenInclude(m => m.MeetingNotes)
                                .FirstOrDefaultAsync(item => item.Id == userId);
        }

        public async Task<ClientModel> GetClientByEmailAsync(string email)
        {
            return await _dataContext.ClientInfo
            .Where(c => c.IsDeleted == false)
                .Include(c => c.ProgramInfo)
                    .Include(c => c.Stipends)
                        .Include(c => c.Meetings)
                            .ThenInclude(m => m.MeetingNotes)
                                .FirstOrDefaultAsync(c => c.Email == email && c.IsDeleted == false);
        }

        public async Task<IEnumerable<ClientModel>> GetClientsByFirstNameAndLastnameAsync(string Firstname, string Lastname)
        {
            return await _dataContext.ClientInfo
            .Where(c => c.IsDeleted == false && c.Firstname == Firstname && c.Lastname == Lastname)
                .Include(c => c.ProgramInfo)
                    .Include(c => c.Meetings)
                        .ThenInclude(m => m.MeetingNotes)
                                .ToListAsync();
        }
    }
}
