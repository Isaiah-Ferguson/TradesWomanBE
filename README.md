# TradesWomanBE API

## Description
This API manages client information and meeting records for the TradesWomanBE project.

## Getting Started

### Prerequisites
- .NET 7.0 SDK
- SQL Server
- Azure

### Front End
Next Js.

## API Endpoints

### UserCotnroller
- `POST /User/AddUser`: Adds a new client.
- `POST /User/AddRecruiter`: Adds a New Recruiter.
- `GET /User/GetAllRecruiters`: Get all Recruiters.
- `GET /User/GetRecruiterByEmail/{email}`: Get Recruiters by email
- `POST /User/Login`: Login for Recrutier & Admin.
- `PUT /User/ChangePassword`: Admin Changes Password.

### ClientController
- `POST /Client/AddClient`: Adds a new client.
- `PUT /Client/EditClient`: Edits an existing client.
- `PUT /Client/DeleteClient`: Edits an existing client.
- `GET /Client/GetAllClients`: Retrieves all clients.
- `GET /Client/GetLast30Clients`: Retrieves last 30 clients.
- `GET /Client/GetClientsByFirstAndLastName`: Retrieves Clients by Firstname or Lastname.
- `GET /Client/GetClientSummary`: Retrieves Clients by Firstname or Lastname.
- `GET /Client/GetClientSummaryByRecruiter/{firstName/{lastName}}`: Retrieves Clients by Firstname or Lastname.


### MeetingsController
- `POST /Meetings/AddMeeting`: Adds a new meeting.
- `PUT /Meetings/EditMeeting`: Edits a meeting.
- `GET /Meetings/GetMeeting/{id}`: Retrieves meeting information by ID.
- `POST /Meetings/AddMeetingNotes/{meetingId}.`: Adds a new meeting notes.
- `GET /Meetings/GetMeetingNotes/{meetingId}.`: Retrieves meeting notes by ID.

### ProgramController
- `POST /Program/AddProgram`: Adds a new Program.
- `PUT /Program/EditProgram`: Edits a Program.
- `GET /Program/GetProgram/{id}`: Retrieves Program information by ID.
- `POST /Program/AddStipend.`: Adds a new Stipend.
- `PUT /Program/EditStipend/{clientId}`: Edits a Stipend.
- `GET /Program/GetStipend/{id}.`: Retrieves Stipend notes by ID.

### CSVController
- `GET /CSV/ExportClients.`: Exports Clients into a CSV File.
- `GET /CSV/ExportClientsByProgram/{Program}.`: Exports Clients into a CSV File Filtered By Program.
- `GET /CSV/ExportClientsByDate/{StartDate}/{EndDate}`: Exports Clients into a CSV File filtered By Date.
- `GET /CSV/ExportClientsByDate{StartDate}/{EndDate}/{Program}`: Exports Clients into a CSV File filtered By Date and Program.
- `POST /CSV/ImportClientsFromCsv`: Imports Clients Info from a CSV File.

### What is IActionResult?

 - It's Very Flexible and Returns Response Status IE 200, 400, and 500
 - Allows use to customize our Responses

### Why Use Async Methods / Task<IActionResult>?

- Async Methods Improves Scalibility and responsiviness of your application.

-In C#, Task<IActionResult> is used in the method signature when you want to perform asynchronous operations and return an IActionResult.

-Using 'Task' allows the method to be await-ed, which is crucial for non-blocking I/O operations, such as database queries or network requests.

-By using async and await, you can efficiently handle I/O-bound tasks like database operations in your web API, ensuring your application remains responsive under load.

-Asynchronous Programming: Using Task<IActionResult> and async/await is best practice for handling potentially long-running tasks in web APIs.

-Consistent Response Handling: Returning IActionResult allows for clear and appropriate HTTP responses, improving client-side error handling and user experience.

### SMTP (Simple Mail Transfer Protocol)

- SMTP is a standard protocol used to send and route emails across networks. It acts as a communication channel between the email client and the mail server to deliver emails to the recipient's mail server.

### How SMTP Was Implemented:

1. Mail Server Configuration: Configured the application to use an SMTP server, specifying the server address, port, and authentication credentials.

2. Sending Email: The application uses the SmtpClient class from the .NET System.Net.Mail namespace to connect to the SMTP server and send emails. The email message is created using MailMessage, with fields like the recipient, subject, and body being populated.

3. Security: To authenticate with the SMTP server, credentials are provided (username and password). However, since Google requires 2-step verification, an app-specific password was generated for secure access.

### Generating CSV Files from Database Queries

## Data Context & Querying:
The application uses Entity Framework to interact with the database. Data is fetched by querying the ClientInfo table and its related ProgramInfo using the .Include() method, ensuring that the related program data is eagerly loaded.

## CSVHelper Method for Reusability:
Instead of repeating code, a helper method, CSVHelper, is used to take a list of clients and generate the corresponding CSV data.
The HelperMethod constructs a CSV file using a StringBuilder, which efficiently appends string data line by line.

## Returning CSV as String:
The result of the helper method is a string that represents the full CSV content, which can be further used for downloading or storing the data.

### What 'using' Does

1. Manages Resources Automatically: When you open a connection to something external (like a database or email service), that connection needs to be closed to free up system resources.

2. Simplifies Cleanup: Instead of having to manually close or dispose of the connection, using automatically handles that when you're done, even if there's an error.

## Example of using
In our HashPassword method, we are creating two disposable objects: RandomNumberGenerator and Rfc2898DeriveBytes. Both of these implement the IDisposable interface, which means they use resources like unmanaged memory that need to be released when you're done with them. The using statement helps ensure these resources are cleaned up properly and as soon as they are no longer needed, even if an exception is thrown.

## JWT & Auth 

Authentication (auth) is the process of verifying the identity of a user, ensuring they are who they claim to be, typically by using credentials like a username and password. JSON Web Tokens (JWT) are a compact, URL-safe way to securely transmit information between parties as a JSON object. In authentication, a server issues a JWT after verifying a user's credentials, and this token is then used by the client (usually in HTTP headers) to authenticate requests. The JWT contains claims (like user info and expiration time) and is signed by the server's secret key to ensure its integrity, meaning it can't be tampered with.