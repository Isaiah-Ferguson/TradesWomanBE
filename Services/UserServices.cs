using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradesWomanBE.Services.Context;
using TradesWomanBE.Services.Interfaces;

namespace TradesWomanBE.Services
{
    public class UserServices : IUserServices
    {
        private readonly DataContext _dataContext;
        public UserServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool DoesUserExist(int? SSNLastFour)
        {
            return _dataContext.ClientInfo.SingleOrDefault(client => client.SSNLastFour == SSNLastFour) != null;
        }

        
    }
}