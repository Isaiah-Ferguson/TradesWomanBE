# TradesWomanBE API

## Description
This API manages client information and meeting records for the TradesWomanBE project.

## Getting Started

### Prerequisites
- .NET 7.0 SDK
- SQL Server

### Front End
Next Js.


## API Endpoints

### UserCotnroller
- `POST /Client/AddClient`: Adds a new client.
- `PUT /Client/EditClient`: Edits an existing client.
- `GET /Client/GetAllClients`: Retrieves all clients.
- `GET /Client/ExportClients`: Exports all clients to a CSV file.

### ClientController
- `POST /Client/AddClient`: Adds a new client.
- `PUT /Client/EditClient`: Edits an existing client.
- `GET /Client/GetAllClients`: Retrieves all clients.
- `GET /Client/ExportClients`: Exports all clients to a CSV file.

### MeetingsController
- `POST /Meetings/AddMeeting`: Adds a new meeting.
- `GET /Meetings/GetMeeting/{id}`: Retrieves meeting information by ID.

