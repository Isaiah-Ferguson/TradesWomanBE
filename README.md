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
- `POST /Client/AddUser`: Adds a new client.
- `POST /Client/AddRecruiter`: Adds a New Recruiter.
- `POST /Client/Login`: Login for Recrutier & Admin.
- `PUT /Client/ChangePassword`: Admin Changes Password.

### ClientController
- `POST /Client/AddClient`: Adds a new client.
- `PUT /Client/EditClient`: Edits an existing client.
- `GET /Client/GetAllClients`: Retrieves all clients.
- `GET /Client/GetLast30Clients`: Retrieves last 30 clients.
- `GET /Client/ExportClients`: Exports all clients to a CSV file.

### MeetingsController
- `POST /Meetings/AddMeeting`: Adds a new meeting.
- `GET /Meetings/GetMeeting/{id}`: Retrieves meeting information by ID.
- `GET /Meetings/GetMeetingNotes/{meetingId}.`: Retrieves meeting notes by ID.

### Why Use Async Methods / Task<IActionResult>?

- Async Methods Improves Scalibility and responsiviness of your application.

-In C#, Task<IActionResult> is used in the method signature when you want to perform asynchronous operations and return an IActionResult.

-Using 'Task' allows the method to be await-ed, which is crucial for non-blocking I/O operations, such as database queries or network requests.

-By using async and await, you can efficiently handle I/O-bound tasks like database operations in your web API, ensuring your application remains responsive under load.

-Asynchronous Programming: Using Task<IActionResult> and async/await is best practice for handling potentially long-running tasks in web APIs.

-Consistent Response Handling: Returning IActionResult allows for clear and appropriate HTTP responses, improving client-side error handling and user experience.