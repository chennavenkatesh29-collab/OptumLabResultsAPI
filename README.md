# Optum Grade 26 – Lab Results API

A small ASP.NET Core Web API for creating, retrieving, updating and deleting patient lab results. This solution was prepared against the supplied take-home assignment.

## Technology

- C# / ASP.NET Core Web API targeting .NET 10
- In-memory repository (permitted by the assignment)
- xUnit unit tests
- SQL Server schema and sample queries
- Swagger/OpenAPI in Development

## Prerequisites

- .NET 10 SDK
- Visual Studio 2022 (with a compatible .NET 10 SDK) or VS Code
- Internet access on first build to restore NuGet packages

## Run the API

From the solution root:

```bash
dotnet restore
dotnet run --project src/OptumLabResultsAPI
```

Open the Swagger URL printed in the terminal. With the included launch profile, the URLs are:

- https://localhost:7184/swagger
- http://localhost:5184/swagger

If HTTPS development certificates are not configured, use the HTTP URL.

## Run unit tests

```bash
dotnet test
```

Four unit tests are included: create, patient filtering, missing record retrieval, and delete.

## API endpoints

| Method | Route | Description |
|---|---|---|
| POST | `/labresults` | Create a lab result |
| GET | `/labresults/{id}` | Retrieve by ID |
| GET | `/labresults?patientId=PAT-1001` | Retrieve results for a patient |
| GET | `/labresults` | Retrieve all results |
| PUT | `/labresults/{id}` | Update a result |
| DELETE | `/labresults/{id}` | Delete a result |

### Example POST body

```json
{
  "patientId": "PAT-1001",
  "testName": "Hemoglobin",
  "resultValue": "13.5",
  "unit": "g/dL",
  "observedDate": "2026-10-20T09:30:00Z"
}
```

A successful POST returns **201 Created**, the created resource, and a Location header.

### Example PUT body

```json
{
  "patientId": "PAT-1001",
  "testName": "Hemoglobin",
  "resultValue": "14.0",
  "unit": "g/dL",
  "observedDate": "2026-10-21T09:30:00Z"
}
```

## SQL artifacts

Run `sql/LabResults.sql` in SQL Server Management Studio. It contains the table definition, a patient/date filtering query, and an aggregation query counting tests per patient.

## Design decisions and tradeoffs

- **Controller:** Handles HTTP routing, request binding and HTTP response codes.
- **DTOs:** Define and validate the API input contract separately from the domain model.
- **Service:** Contains application logic and date validation.
- **Repository:** Abstracts data access from business logic.
- **In-memory storage:** Keeps the exercise small and avoids requiring a database server. Data is lost when the API process restarts.
- **SQL script:** Demonstrates the requested SQL schema, index, filtering and aggregation independently of the in-memory runtime repository.
- **Exception middleware:** Converts expected argument errors to HTTP 400 and unexpected errors to a generic HTTP 500 response while logging details server-side.

## Important production considerations

This is a take-home demonstration, not a production healthcare system. Before production use, add authentication and authorization, audit logging, encryption in transit and at rest, retention controls, secure secret management, and appropriate privacy/compliance review. Never use real patient data in this sample.

## Suggested walkthrough

1. Start at `Program.cs` and explain dependency injection.
2. Explain the request DTO and model validation.
3. Trace a POST request through Controller → Service → Repository.
4. Explain how GET filtering works.
5. Show update/delete behavior and HTTP status codes.
6. Open the SQL script and explain the index, filtering and aggregation.
7. Run `dotnet test` and explain the test cases.
8. Explain that replacing the in-memory repository with EF Core or Dapper would provide persistent storage without changing the controller/service contract.
