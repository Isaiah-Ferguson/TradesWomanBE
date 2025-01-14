using TradesWomanBE.Models;
using Microsoft.EntityFrameworkCore;
using TradesWomanBE.Services.Context;
using AutoMapper;

namespace TradesWomanBE.Services
{
    public class ProgramServices
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public ProgramServices(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        // ************************* Program Methods and Logic ***********************
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
                var client = await _dataContext.ClientInfo.FirstOrDefaultAsync(client => client.Id == newProgram.ClientID);

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

            var existingProgram = await GetProgramByIdAsync(newProgram.Id);

            if (existingProgram == null)
            {
                return false;
            }
            _mapper.Map(newProgram, existingProgram);

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

        //************************ Stipends Methods and Logic ********************************
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

            var existingStipend = await GetStipendByIdAsync(newStipend.Id);

            if (existingStipend == null)
            {
                return false;
            }

            _mapper.Map(newStipend, existingStipend);
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

        // *************Program Look Up Table Logic and Functions***************

        public async Task<bool> AddProgramLookUpAsync(ProgramLookUpModel programLookUp)
        {
            if (!await DoesProgramNameExistAsync(programLookUp.ProgramName))
            {
                await _dataContext.AddAsync(programLookUp);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<bool> EditProgramLookUpAsync(ProgramLookUpModel programLookUp)
        {
            if (await DoesProgramNameExistIdAsync(programLookUp))
            {
                var existingProgramName = await GetProgramByIdAsync(programLookUp);
                existingProgramName.ProgramName = programLookUp.ProgramName;
                existingProgramName.IsDeleted = false;

                _dataContext.Update(existingProgramName);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<bool> DeleteProgramLookUpAsync(ProgramLookUpModel programLookUp)
        {
            if (await DoesProgramNameExistIdAsync(programLookUp))
            {
                var existingProgramName = await GetProgramByIdAsync(programLookUp);
                existingProgramName.IsDeleted = true;

                _dataContext.Update(existingProgramName);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<IEnumerable<ProgramLookUpModel>> GetProgramLookUpAsync()
        {
            return await _dataContext.ProgramLookUps.ToListAsync();
        }

        public async Task<bool> DoesProgramNameExistAsync(string programName)
        {
            return await _dataContext.ProgramLookUps.AnyAsync(p => p.ProgramName == programName);
        }

        public async Task<bool> DoesProgramNameExistIdAsync(ProgramLookUpModel program)
        {
            return await _dataContext.ProgramLookUps.AnyAsync(p => p.Id == program.Id);
        }
        public async Task<ProgramLookUpModel> GetProgramByIdAsync(ProgramLookUpModel programLookUp)
        {
            return await _dataContext.ProgramLookUps.FirstOrDefaultAsync(p => p.Id == programLookUp.Id);
        }

    }
}