using Microsoft.EntityFrameworkCore;
using TradesWomanBE.Services.Context;
using System.Text;
using TradesWomanBE.Models;

namespace TradesWomanBE.Services
{
    public class CSVServices
    {
        private readonly DataContext _dataContext;

        public CSVServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public string GetClientsAsCsv()
        {
            var clients = _dataContext.ClientInfo.Include(c => c.ProgramInfo).ToList();
            return CSVHelper(clients);
        }

        public string GetClientsAsCsvByDate(string startDate, string endDate)
        {
            DateTime start = DateTime.Parse(startDate);
            DateTime end = DateTime.Parse(endDate);

            var clients = _dataContext.ClientInfo
                .Include(c => c.ProgramInfo)
                .Where(c => DateTime.Parse(c.DateRegistered) >= start && DateTime.Parse(c.DateRegistered) <= end)
                .ToList();

            return CSVHelper(clients);
        }
        public string GetClientsAsCsvByProgramAndDate(string programName, DateTime startDate, DateTime endDate)
        {
            var clients = _dataContext.ClientInfo
                .Include(c => c.ProgramInfo)
                .Where(c => c.ProgramInfo != null && c.ProgramInfo.ProgramEnrolled == programName
                            && DateTime.Parse(c.DateRegistered) >= startDate
                            && DateTime.Parse(c.DateRegistered) <= endDate)
                .ToList();

            return CSVHelper(clients);
        }

        public string GetClientsAsCsvByProgram(string programName)
        {
            var clients = _dataContext.ClientInfo
                .Include(c => c.ProgramInfo)
                .Where(c => c.ProgramInfo != null && c.ProgramInfo.ProgramEnrolled == programName)
                .ToList();

            return CSVHelper(clients);
        }

        private static string CSVHelper(List<ClientModel> clients)
        {
            var sb = new StringBuilder();
            sb.AppendLine("FirstName,LastName,Email,ValidSSNAuthToWrk,CriminalHistory,Disabled,FoundUsOn,DateJoinedEAW,Stipends,Address,Gender,Employed,ProgramEnrolled,ProgramEndDate,CurrentStatus,GrantName");

            foreach (var client in clients)
            {
                var program = client.ProgramInfo;
                sb.AppendLine($"{client.Id},{client.Firstname},{client.Lastname},{client.SSNLastFour},{client.MiddleInitial},{client.Email},{client.ValidSSNAuthToWrk},{client.CriminalHistory},{client.FoundUsOn},{client.DateJoinedEAW},{client.Stipends},{client.Address},{client.Gender},{client.Employed}," +
                              $"{program?.ProgramEnrolled},{program?.EnrollDate},{program?.ProgramEndDate}");
            }

            return sb.ToString(); // Return the final CSV string
        }


    }
}