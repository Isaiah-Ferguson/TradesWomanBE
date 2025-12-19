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

        public async Task WriteClientsCsvAsync(Stream outputStream)
        {
            await using var writer = new StreamWriter(outputStream, Encoding.UTF8, bufferSize: 16 * 1024, leaveOpen: true);
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                ShouldQuote = _ => true
            };
            await using var csv = new CsvWriter(writer, csvConfig);

            WriteClientsHeader(csv);

            await foreach (var client in _dataContext.ClientInfo
                               .AsNoTracking()
                               .Include(c => c.ProgramInfo)
                               .Include(c => c.Stipends)
                               .AsAsyncEnumerable())
            {
                WriteClientRow(csv, client);
            }

            await writer.FlushAsync();
        }

        public async Task WriteClientsCsvByProgramAsync(Stream outputStream, string programName)
        {
            await using var writer = new StreamWriter(outputStream, Encoding.UTF8, bufferSize: 16 * 1024, leaveOpen: true);
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                ShouldQuote = _ => true
            };
            await using var csv = new CsvWriter(writer, csvConfig);

            WriteClientsHeader(csv);

            await foreach (var client in _dataContext.ClientInfo
                               .AsNoTracking()
                               .Include(c => c.ProgramInfo)
                               .Include(c => c.Stipends)
                               .Where(c => c.ProgramInfo != null && c.ProgramInfo.ProgramEnrolled == programName)
                               .AsAsyncEnumerable())
            {
                WriteClientRow(csv, client);
            }

            await writer.FlushAsync();
        }

        public async Task WriteClientsCsvByDateAsync(Stream outputStream, string startDate, string endDate)
        {
            if (!DateTime.TryParse(startDate, out var start) || !DateTime.TryParse(endDate, out var end))
            {
                throw new FormatException("Start or End date is not in a valid DateTime format.");
            }

            await using var writer = new StreamWriter(outputStream, Encoding.UTF8, bufferSize: 16 * 1024, leaveOpen: true);
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                ShouldQuote = _ => true
            };
            await using var csv = new CsvWriter(writer, csvConfig);

            WriteClientsHeader(csv);

            await foreach (var client in _dataContext.ClientInfo
                               .AsNoTracking()
                               .Include(c => c.ProgramInfo)
                               .Include(c => c.Stipends)
                               .AsAsyncEnumerable())
            {
                if (!DateTime.TryParse(client.DateRegistered, out var dateRegistered))
                {
                    continue;
                }

                if (dateRegistered < start || dateRegistered > end)
                {
                    continue;
                }

                WriteClientRow(csv, client);
            }

            await writer.FlushAsync();
        }

        public async Task WriteClientsCsvByProgramAndDateAsync(Stream outputStream, string programName, string startDate, string endDate)
        {
            if (!DateTime.TryParse(startDate, out var start) || !DateTime.TryParse(endDate, out var end))
            {
                throw new FormatException("Start or End date is not in a valid DateTime format.");
            }

            await using var writer = new StreamWriter(outputStream, Encoding.UTF8, bufferSize: 16 * 1024, leaveOpen: true);
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                ShouldQuote = _ => true
            };
            await using var csv = new CsvWriter(writer, csvConfig);

            WriteClientsHeader(csv);

            await foreach (var client in _dataContext.ClientInfo
                               .AsNoTracking()
                               .Include(c => c.ProgramInfo)
                               .Include(c => c.Stipends)
                               .Where(c => c.ProgramInfo != null && c.ProgramInfo.ProgramEnrolled == programName)
                               .AsAsyncEnumerable())
            {
                if (!DateTime.TryParse(client.DateRegistered, out var dateRegistered))
                {
                    continue;
                }

                if (dateRegistered < start || dateRegistered > end)
                {
                    continue;
                }

                WriteClientRow(csv, client);
            }

            await writer.FlushAsync();
        }

        private static void WriteClientsHeader(CsvWriter csv)
        {
            csv.WriteField("Id");
            csv.WriteField("Age");
            csv.WriteField("Firstname");
            csv.WriteField("Lastname");
            csv.WriteField("MiddleInitial");
            csv.WriteField("Email");
            csv.WriteField("ChildrenUnderSix");
            csv.WriteField("ChildrenOverSix");
            csv.WriteField("TotalHouseholdFamily");
            csv.WriteField("SSNLastFour");
            csv.WriteField("ValidSSNAuthToWrk");
            csv.WriteField("CriminalHistory");
            csv.WriteField("Disabled");
            csv.WriteField("FoundUsOn");
            csv.WriteField("DateJoinedEAW");
            csv.WriteField("Address");
            csv.WriteField("City");
            csv.WriteField("State");
            csv.WriteField("ZipCode");
            csv.WriteField("Gender");
            csv.WriteField("Employed");
            csv.WriteField("RecruiterName");
            csv.WriteField("DateRegistered");
            csv.WriteField("DateOfBirth");
            csv.WriteField("ActiveOrFormerMilitary");
            csv.WriteField("TotalMonthlyIncome");
            csv.WriteField("PhoneNumber");
            csv.WriteField("ProgramInfoId");
            csv.WriteField("HighestEducation");
            csv.WriteField("ValidCALicense");
            csv.WriteField("County");
            csv.WriteField("Ethnicity");
            csv.WriteField("ProgramEnrolled");
            csv.WriteField("ProgramEnrolledDate");
            csv.WriteField("ProgramEndDate");
            csv.WriteField("CurrentStatus");
            csv.WriteField("PreApprenticeshipProgram");
            csv.WriteField("TypeOfStipend");
            csv.WriteField("IsDeleted");
            csv.NextRecord();
        }

        private static void WriteClientRow(CsvWriter csv, ClientModel client)
        {
            var program = client.ProgramInfo;
            var stipend = client.Stipends;

            csv.WriteField(client.Id);
            csv.WriteField(client.Age);
            csv.WriteField(client.Firstname);
            csv.WriteField(client.Lastname);
            csv.WriteField(client.MiddleInitial);
            csv.WriteField(client.Email);
            csv.WriteField(client.ChildrenUnderSix);
            csv.WriteField(client.ChildrenOverSix);
            csv.WriteField(client.TotalHouseholdFamily);
            csv.WriteField(client.SSNLastFour);
            csv.WriteField(client.ValidSSNAuthToWrk);
            csv.WriteField(client.CriminalHistory);
            csv.WriteField(client.Disabled);
            csv.WriteField(client.FoundUsOn);
            csv.WriteField(client.DateJoinedEAW);
            csv.WriteField(client.Address);
            csv.WriteField(client.City);
            csv.WriteField(client.State);
            csv.WriteField(client.ZipCode);
            csv.WriteField(client.Gender);
            csv.WriteField(client.Employed);
            csv.WriteField(client.RecruiterName);
            csv.WriteField(client.DateRegistered);
            csv.WriteField(client.DateOfBirth);
            csv.WriteField(client.ActiveOrFormerMilitary);
            csv.WriteField(client.TotalMonthlyIncome);
            csv.WriteField(client.PhoneNumber);
            csv.WriteField(client.ProgramInfoId);
            csv.WriteField(client.HighestEducation);
            csv.WriteField(client.ValidCALicense);
            csv.WriteField(client.County);
            csv.WriteField(client.Ethnicity);

            csv.WriteField(program?.ProgramEnrolled);
            csv.WriteField(program?.EnrollDate);
            csv.WriteField(program?.ProgramEndDate);
            csv.WriteField(program?.CurrentStatus);

            csv.WriteField(stipend?.PreApprenticeshipProgram);
            csv.WriteField(stipend?.TypeOfStipend);

            csv.WriteField(client.IsDeleted ? "TRUE" : "FALSE");
            csv.NextRecord();
        }
        public string GetClientsAsCsv()
        {
            var clients = _dataContext.ClientInfo
                .AsNoTracking()
                .Include(c => c.ProgramInfo)
                .Include(c => c.Stipends)
                .ToList();
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
                .AsNoTracking()
                .Include(c => c.ProgramInfo)
                .Include(c => c.Stipends)
                .AsEnumerable()
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
                .AsNoTracking()
                .Include(c => c.ProgramInfo)
                .Where(c => c.ProgramInfo != null && c.ProgramInfo.ProgramEnrolled == programName)
                .AsEnumerable()
                .Where(c => DateTime.TryParse(c.DateRegistered, out var dateRegistered) &&
                            dateRegistered >= start && dateRegistered <= end)
                .ToList();

            return CSVHelper(clients);
        }

        public string GetClientsAsCsvByProgram(string programName)
        {
            var clients = _dataContext.ClientInfo
                .AsNoTracking()
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
                var fields = new[]
                {
                    client.Id.ToString(),
                    client.Age.ToString(),
                    client.Firstname,
                    client.Lastname,
                    client.MiddleInitial,
                    client.Email,
                    client.ChildrenUnderSix.ToString(),
                    client.ChildrenOverSix.ToString(),
                    client.TotalHouseholdFamily.ToString(),
                    client.SSNLastFour.ToString(),
                    client.ValidSSNAuthToWrk,
                    client.CriminalHistory,
                    client.Disabled,
                    client.FoundUsOn,
                    client.DateJoinedEAW,
                    client.Address,
                    client.City,
                    client.State,
                    client.ZipCode.ToString(),
                    client.Gender,
                    client.Employed,
                    client.RecruiterName,
                    client.DateRegistered,
                    client.DateOfBirth,
                    client.ActiveOrFormerMilitary,
                    client.TotalMonthlyIncome.ToString(),
                    client.PhoneNumber,
                    client.ProgramInfoId?.ToString(),
                    client.HighestEducation,
                    client.ValidCALicense,
                    client.County,
                    client.Ethnicity,
                    client.IsDeleted ? "TRUE" : "FALSE",
                    program?.ProgramEnrolled,
                    program?.EnrollDate,
                    program?.ProgramEndDate,
                    program?.CurrentStatus,
                    stipend?.PreApprenticeshipProgram,
                    stipend?.TypeOfStipend
                };

                sb.AppendLine(string.Join(",", fields.Select(EscapeCsv)));
            }

            return sb.ToString(); // Return the final CSV string
        }

        private static string EscapeCsv(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            var needsQuotes = value.Contains(',') || value.Contains('"') || value.Contains('\n') || value.Contains('\r');
            if (!needsQuotes)
            {
                return value;
            }

            var escaped = value.Replace("\"", "\"\"");
            return $"\"{escaped}\"";
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