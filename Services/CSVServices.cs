using Microsoft.EntityFrameworkCore;
using TradesWomanBE.Services.Context;
using System.Text;
using TradesWomanBE.Models;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http.HttpResults;


namespace TradesWomanBE.Services
{
    public class CSVServices
    {
        private readonly DataContext _dataContext;
        private readonly ProgramServices _programServices;
        private readonly MeetingsServices _meetingServices;

        public CSVServices(DataContext dataContext, ProgramServices programServices, MeetingsServices meetingServices)
        {
            _dataContext = dataContext;
            _programServices = programServices;
            _meetingServices = meetingServices;
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
            sb.AppendLine("Id,Age,Firstname,Lastname,MiddleInitial,Email,ChildrenUnderSix,ChildrenOverSix,TotalHouseholdFamily,SSNLastFour,ValidSSNAuthToWrk,CriminalHistory,Disabled,FoundUsOn,DateJoinedEAW,Address,City,State,ZipCode,Gender,Employed,RecruiterName,DateRegistered,DateOfBirth,ActiveOrFormerMilitary,TotalMonthlyIncome,PhoneNumber,ProgramInfoId,HighestEducation,ValidCALicense,County,Ethnicity,IsDeleted,ProgramEnrolled,ProgramEnrolledDate,ProgramEndDate,CurrentStatus,PreApprenticeshipProgram,TypeOfStipend");

            foreach (var client in clients)
            {
                var program = client.ProgramInfo;
                var stipend = client.Stipends;
                sb.AppendLine($"{client.Id},{client.Age},{client.Firstname},{client.Lastname},{client.MiddleInitial},{client.Email},{client.ChildrenUnderSix},{client.ChildrenOverSix},{client.TotalHouseholdFamily},{client.SSNLastFour},{client.ValidSSNAuthToWrk},{client.CriminalHistory},{client.Disabled},{client.FoundUsOn},{client.DateJoinedEAW},{client.Address},{client.City},{client.State},{client.ZipCode},{client.Gender},{client.Employed},{client.RecruiterName},{client.DateRegistered},{client.DateOfBirth},{client.ActiveOrFormerMilitary},{client.TotalMonthlyIncome},{client.PhoneNumber},{client.ProgramInfoId},{client.HighestEducation},{client.ValidCALicense},{client.County},{client.Ethnicity},{client.IsDeleted}" +
                              $"{program?.ProgramEnrolled},{program?.EnrollDate},{program?.ProgramEndDate},{program?.CurrentStatus}" + $"{stipend?.PreApprenticeshipProgram}, {stipend?.TypeOfStipend}");
            }

            return sb.ToString(); // Return the final CSV string
        }

        public async Task<List<ClientModel>> ImportClientsFromCsvAsync(Stream csvStream)
        {
            var clients = new List<ClientModel>();

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null, 
                MissingFieldFound = null // Ignore missing fields
            };

            using (var reader = new StreamReader(csvStream))
            using (var csv = new CsvReader(reader, csvConfig))
            {

                try
                {
                    var records = csv.GetRecords<ClientModel>();
                    clients.AddRange(records);
                    
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while importing clients from CSV", ex);
                }
            }

            return clients;
        }

        public async Task ImportProgramsFromCsvAsync(Stream filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var programRecords = csv.GetRecords<ProgramModel>().ToList();

            foreach (var program in programRecords)
            {
                await _programServices.AddProgramAsync(program);
            }
        }
        public async Task ImportStipendsFromCsvAsync(Stream filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var stipendsRecords = csv.GetRecords<StipendsModel>().ToList();

            foreach (var stipend in stipendsRecords)
            {
                await _programServices.AddStipendAsync(stipend);
            }
        }

        public async Task ImportMeetingsFromCsvAsync(Stream filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var meetingRecords = csv.GetRecords<MeetingsModel>().ToList();

            foreach (var meeting in meetingRecords)
            {
                await _meetingServices.AddMeetingAsync(meeting);
            }
        }

        public async Task ImportMeetingsNotesFromCsvAsync(Stream filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var meetingNotesRecords = csv.GetRecords<MeetingNotesModel>().ToList();

            foreach (var meetingNote in meetingNotesRecords)
            {
                await _meetingServices.AddMeetingNotesAsync(meetingNote);
            }
        }



        public async Task SaveClientsToDatabaseAsync(List<ClientModel> clients)
        {
            if (clients == null || !clients.Any())
            {
                throw new ArgumentException("The client list is empty or null.");
            }

            await _dataContext.ClientInfo.AddRangeAsync(clients);

            await _dataContext.SaveChangesAsync();
        }
    }
}