using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradesWomanBE.Models;
using TradesWomanBE.Models.DTO;
using TradesWomanBE.Services.Context;
using TradesWomanBE.Services.Interfaces;
using System.Security.Cryptography;


namespace TradesWomanBE.Services
{
    public class ClientServices : IClientServices
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

        public bool AddUser(ClientModel clientModel)
        {
            bool result = false;

            if (!DoesClientExist(clientModel.SSNLastFour, clientModel.Firstname))
            {
                ClientModel newClient = new();

                //var hashSSN = HashSSN(clientModel.SSNLastFour);

                newClient.Firstname = clientModel.Firstname;
                newClient.Lastname = clientModel.Lastname;
                newClient.SSNLastFour = clientModel.SSNLastFour;
                newClient.Address = clientModel.Address;
                newClient.Age = clientModel.Age;
                newClient.Email = clientModel.Email;
                newClient.Disabled = clientModel.Disabled;
                newClient.MiddleInnitial = clientModel.MiddleInnitial;
                newClient.ChildrenOverSix = clientModel.ChildrenOverSix;
                newClient.ChildrenUnderSix = clientModel.ChildrenUnderSix;
                newClient.CriminalHistory = clientModel.CriminalHistory;
                newClient.CTWIStipends = clientModel.CTWIStipends;
                newClient.DateJoinedEAW = clientModel.DateJoinedEAW;
                newClient.IsDeleted = clientModel.IsDeleted;
                newClient.Id = clientModel.Id;


                _dataContext.Add(newClient);
                result = _dataContext.SaveChanges() != 0;

            }
            return result;

        }

        public IEnumerable<ClientModel> GetAllClients()
        {
            return _dataContext.ClientInfo;
        }

        public IEnumerable<ClientModel> GetClientsById(int userId)
        {
            return _dataContext.ClientInfo.Where(item => item.Id == userId);
        }

        public IEnumerable<ClientModel> GetClientsByFirstName(string Firstname, string Lastname)
        {
            return _dataContext.ClientInfo.Where(item => item.Firstname == Firstname && item.Lastname == Lastname);
        }


        public SSNDTO HashSSN(int? SSN)
        {
            string? newSSN = SSN.ToString();
            SSNDTO newHashSSN = new();

            byte[] saltByte = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltByte);
            }

            var salt = Convert.ToBase64String(saltByte);

            using (var deriveBytes = new Rfc2898DeriveBytes(newSSN, saltByte, 310000, HashAlgorithmName.SHA256))
            {
                var hash = Convert.ToBase64String(deriveBytes.GetBytes(32)); // 256 bits

                newHashSSN.Salt = salt;
                newHashSSN.Hash = hash;
            }

            return newHashSSN;
        }
    }


}