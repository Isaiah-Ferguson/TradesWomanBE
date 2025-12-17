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

        public async Task<IEnumerable<ClientModel>> GetLast30ClientsAsync()
        {
            return await _dataContext.ClientInfo
                .AsNoTracking()
                .OrderByDescending(client => client.DateRegistered)
                .Take(30)
                .ToListAsync();
        }

        public async Task<bool> AddClientAsync(ClientModel clientModel)
        {
            var ssnLastFour = clientModel.SSNLastFour;
            var exists = await _dataContext.ClientInfo
                .AsNoTracking()
                .AnyAsync(c => c.SSNLastFour == ssnLastFour);

            if (exists)
            {
                return false;
            }

            await _dataContext.AddAsync(clientModel);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> EditClientAsync(ClientModel clientModel)
        {
            var existingClient = await _dataContext.ClientInfo
                .Where(c => c.IsDeleted == false)
                .FirstOrDefaultAsync(c => c.Id == clientModel.Id);

            if (existingClient == null)
            {
                return false;
            }

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

            _dataContext.Update(existingClient);
            return await _dataContext.SaveChangesAsync() > 0;
        }


        public async Task<bool> DeleteClientAsync(ClientModel clientModel)
        {
            var existingClient = await _dataContext.ClientInfo
                .Where(c => c.IsDeleted == false)
                .FirstOrDefaultAsync(c => c.Id == clientModel.Id);

            if (existingClient == null)
            {
                return false;
            }

            existingClient.IsDeleted = clientModel.IsDeleted;
            _dataContext.Update(existingClient);
            return await _dataContext.SaveChangesAsync() > 0;
        }


        public async Task<IEnumerable<ClientModel>> GetAllClientsAsync()
        {
            return await _dataContext.ClientInfo
            .Where(c => c.IsDeleted == false)
                .AsNoTracking()
                .Include(c => c.ProgramInfo)
                    .Include(c => c.Stipends)
                        .Include(c => c.Meetings)
                            .ThenInclude(m => m.MeetingNotes).ToListAsync();
        }
        public async Task<IEnumerable<object>> GetAllClientsOnLoadAsync()
        {
            return await _dataContext.ClientInfo
                .Where(c => c.IsDeleted == false)
                .AsNoTracking()
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
                .AsNoTracking()
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
                .AsNoTracking()
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
                .AsNoTracking()
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
                .AsNoTracking()
                .Include(c => c.ProgramInfo)
                    .Include(c => c.Meetings)
                        .ThenInclude(m => m.MeetingNotes)
                                .ToListAsync();
        }
    }
}
