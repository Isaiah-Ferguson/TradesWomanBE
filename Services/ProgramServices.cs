using TradesWomanBE.Models;
using Microsoft.EntityFrameworkCore;
using TradesWomanBE.Services.Context;

namespace TradesWomanBE.Services
{
    public class ProgramServices
    {
        private readonly DataContext _dataContext;

        public ProgramServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> AddProgramAsync(ProgramModel newProgram)
        {
            if (newProgram == null)
            {
                return false;
            }

            if (await DoesProgramExistAsync(newProgram.Id))
            {
                await EditProgramAsync(newProgram);
            }
            else
            {
                var client = await _dataContext.ClientInfo
                    .Include(c => c.ProgramInfo)
                    .Include(c => c.Meetings) 
                    .FirstOrDefaultAsync(item => item.Id == newProgram.ClientID);

                if (client != null)
                {
                    client.ProgramInfo = newProgram; 
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

        public async Task<bool> EditProgramAsync(ProgramModel newProgram)
        {
            if (newProgram == null)
            {
                return false;
            }

            // Expecting Type ProgramModel
            var existingProgram = await GetProgramByIdAsync(newProgram.Id);

            // Check if the existing meeting was found
            if (existingProgram == null)
            {
                return false;  
            }

            // Update the entity in the data context
            _dataContext.Programs.Update(existingProgram);
            await _dataContext.SaveChangesAsync();

            return true;
        }

        public async Task<ProgramModel?> GetProgramByIdAsync(int id)
        {
            return await _dataContext.Programs.FirstOrDefaultAsync(program => program.Id == id);
        }

        public async Task<bool> DoesProgramExistAsync(int id)
        {
            return await _dataContext.Programs.AnyAsync(program => program.Id == id);
        }
        //Stipends Begings

        public async Task<bool> AddStipendAsync(StipendsModel newStipend)
        {
            if (newStipend == null)
            {
                return false;
            }

            if (await DoesStipendExistAsync(newStipend.Id))
            {
                await EditStipendAsync(newStipend);
            }
            else
            {
                var client = await _dataContext.ClientInfo.FirstOrDefaultAsync(item => item.Id == newStipend.ClientId);

                if (client != null)
                {
                    client.Stipends = newStipend;
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

        public async Task<bool> EditStipendAsync(StipendsModel newStipend)
        {
            if (newStipend == null)
            {
                return false;
            }

            // var Type is StipendsModel
            var existingStipend = await GetStipendByIdAsync(newStipend.Id);

            // Check if the existing meeting was found
            if (existingStipend == null)
            {
                return false;  
            }

            // Update the entity in the data context
            _dataContext.Stipends.Update(existingStipend);
            await _dataContext.SaveChangesAsync();

            return true;
        }

        public async Task<StipendsModel?> GetStipendByIdAsync(int id)
        {
            return await _dataContext.Stipends.FirstOrDefaultAsync(stipend => stipend.Id == id);
        }

        public async Task<bool> DoesStipendExistAsync(int id)
        {
            return await _dataContext.Stipends.AnyAsync(stipend => stipend.Id == id);
        }


    }
}