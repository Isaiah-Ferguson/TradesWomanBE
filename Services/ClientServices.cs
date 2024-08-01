using TradesWomanBE.Models;
using TradesWomanBE.Models.DTO;
using TradesWomanBE.Services.Context;
using System.Security.Cryptography;
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

        public bool DoesClientExist(int? SSNLastFour, string Firstname)
        {
            return _dataContext.ClientInfo.SingleOrDefault(client => client.SSNLastFour == SSNLastFour && client.Firstname == Firstname) != null;
        }

        public bool AddClient(ClientModel clientModel)
        {
            if (!DoesClientExist(clientModel.SSNLastFour, clientModel.Firstname))
            {
                _dataContext.Add(clientModel);
                return _dataContext.SaveChanges() > 0;
            }
            return false;
        }

        public bool UpdateClient(ClientModel clientToUpdate)
        {
            _dataContext.Update<ClientModel>(clientToUpdate);
            return _dataContext.SaveChanges() != 0;
        }

        public bool UpdateProgram(ProgramModel programToUpdate)
        {
            _dataContext.Update<ProgramModel>(programToUpdate);
            return _dataContext.SaveChanges() != 0;
        }

        public bool UpdateCTWIStipend(CTWIStipendsModel stipendInfo)
        {
            _dataContext.Update<CTWIStipendsModel>(stipendInfo);
            return _dataContext.SaveChanges() != 0;
        }


        public IEnumerable<ClientModel> GetAllClients()
        {
            return _dataContext.ClientInfo;
        }

        public ClientModel GetClientById(int userId)
        {
            return _dataContext.ClientInfo
                .Include(c => c.ProgramInfo)
                .Include(c => c.Meetings)
                    .ThenInclude(m => m.MeetingNotes)
                .FirstOrDefault(item => item.Id == userId);
        }

        public IEnumerable<ClientModel> GetClientsByFirstNameAndLastname(string Firstname, string Lastname)
        {
            return _dataContext.ClientInfo
                .Include(c => c.ProgramInfo)
                .Include(c => c.Meetings)
                    .ThenInclude(m => m.MeetingNotes).Where(item => item.Firstname == Firstname && item.Lastname == Lastname);
        }


        // public SSNDTO HashSSN(int? SSN)
        // {
        //     string? newSSN = SSN.ToString();
        //     SSNDTO newHashSSN = new();

        //     byte[] saltByte = new byte[64];
        //     using (var rng = RandomNumberGenerator.Create())
        //     {
        //         rng.GetBytes(saltByte);
        //     }

        //     var salt = Convert.ToBase64String(saltByte);

        //     using (var deriveBytes = new Rfc2898DeriveBytes(newSSN, saltByte, 310000, HashAlgorithmName.SHA256))
        //     {
        //         var hash = Convert.ToBase64String(deriveBytes.GetBytes(32)); // 256 bits

        //         newHashSSN.Salt = salt;
        //         newHashSSN.Hash = hash;
        //     }

        //     return newHashSSN;
        // }
    }


}