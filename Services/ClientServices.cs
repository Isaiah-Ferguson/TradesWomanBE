using TradesWomanBE.Models;
using TradesWomanBE.Services.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TradesWomanBE.Services
{
    public class ClientServices
    {
        private readonly DataContext _dataContext;
        public ClientServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> DoesClientExistAsync(int? SSNLastFour, string Firstname)
        {
            return await _dataContext.ClientInfo
                .SingleOrDefaultAsync(client => client.SSNLastFour == SSNLastFour && client.Firstname == Firstname) != null;
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
