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
            var clients = _dataContext.ClientInfo.Include(c => c.ProgramInfo).Include(c => c.Stipends).ToList();
            return CSVHelper(clients);
        }
        public string GetClientsAsCsvByDate(string startDate, string endDate)
        {
            DateTime start;
            DateTime end;

            // Try parsing the input start and end dates
            if (!DateTime.TryParse(startDate, out start) || !DateTime.TryParse(endDate, out end))
            {
                throw new FormatException("Start or End date is not in a valid DateTime format.");
            }

            var clients = _dataContext.ClientInfo
                .Include(c => c.ProgramInfo)
                .Include(c => c.Stipends)
                .ToList() // Move the data into memory before filtering the date.
                .Where(c => DateTime.TryParse(c.DateRegistered, out var dateRegistered) &&
                            dateRegistered >= start && dateRegistered <= end)
                .ToList();

            return CSVHelper(clients);
        }


        public string GetClientsAsCsvByProgramAndDate(string programName, string startDate, string endDate)
        {
            DateTime start;
            DateTime end;

            if (!DateTime.TryParse(startDate, out start) || !DateTime.TryParse(endDate, out end))
            {
                throw new FormatException("Start or End date is not in a valid DateTime format.");
            }

            var clients = _dataContext.ClientInfo
                .Include(c => c.ProgramInfo)
                .Where(c => c.ProgramInfo != null && c.ProgramInfo.ProgramEnrolled == programName)
                .ToList()
                .Where(c => DateTime.TryParse(c.DateRegistered, out var dateRegistered) &&
                            dateRegistered >= start && dateRegistered <= end)
                .ToList();

            return CSVHelper(clients);
        }

        public string GetClientsAsCsvByProgram(string programName)
        {
            var clients = _dataContext.ClientInfo
                .Include(c => c.ProgramInfo)
                .Include(c => c.Stipends)
                .Where(c => c.ProgramInfo != null && c.ProgramInfo.ProgramEnrolled == programName)
                .ToList();

            return CSVHelper(clients);
        }

        private static string CSVHelper(List<ClientModel> clients)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Client Id,FirstName,LastName,Last SNN, Middle Innitial,Email,ValidSSNAuthToWrk,CriminalHistory,Disabled,FoundUsOn,DateJoinedEAW,Stipends,Address,Gender,Employed,ProgramEnrolled,ProgramEnrolledDate,ProgramEndDate,CurrentStatus,PreApprenticeshipProgram,TypeOfStipend");

            foreach (var client in clients)
            {
                var program = client.ProgramInfo;
                var stipend = client.Stipends;
                sb.AppendLine($"{client.Id},{client.Firstname},{client.Lastname},{client.SSNLastFour},{client.MiddleInitial},{client.Email},{client.ValidSSNAuthToWrk},{client.CriminalHistory},{client.Disabled},{client.FoundUsOn},{client.DateJoinedEAW},{client.Stipends},{client.Address},{client.Gender},{client.Employed}," +
                              $"{program?.ProgramEnrolled},{program?.EnrollDate},{program?.ProgramEndDate},{program?.CurrentStatus}" + $"{stipend?.PreApprenticeshipProgram}, {stipend?.TypeOfStipend}");
            }

            return sb.ToString(); // Return the final CSV string
        }


    }
}