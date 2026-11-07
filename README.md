# Library Management API

A RESTful library management system built with ASP.NET Core and .NET 10. The API enables librarians to manage books, authors, categories, members, and borrowing operations, while providing authenticated access for library members.

## Features

- JWT-based authentication and role-based authorization
- Member and librarian registration
- Secure login with bearer tokens
- Book, author, and category management
- Book search with filtering, sorting, and pagination
- Loan creation and book-return tracking
- Automatic loan status handling, including overdue loans
- Member loan history
- Soft-delete and restore support for members
- Consistent validation and problem-details error responses
- OpenAPI documentation with Scalar
- Unit and integration test projects

## Roles

### Member

- Register and log in
- Browse books, authors, and categories
- View their own loan history

### Librarian

- Manage books, authors, and categories
- Manage library members
- Create and return loans
- View and manage all loans
- Register other librarians

## API Areas

| Area | Route | Description |
| --- | --- | --- |
| Authentication | `/api/Authentication` | Registration and login |
| Books | `/api/Books` | Browse and manage books |
| Authors | `/api/Authors` | Browse and manage authors |
| Categories | `/api/Categories` | Browse and manage categories |
| Members | `/api/Members` | Manage members and view loan history |
| Loans | `/api/Loans` | Manage borrowing and book returns |

Public read operations are available for books, authors, and categories. Management operations require a JWT token and librarian privileges.

## Technology Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server
- ASP.NET Core Identity
- JWT Bearer Authentication
- FluentValidation
- ErrorOr
- OpenAPI and Scalar
- NUnit

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server or SQL Server Express
- Visual Studio 2026 or another .NET-compatible IDE

### Configuration

Update the connection string and JWT settings in:

```text
LibraryManagement.Api/appsettings.Development.json
```

For production environments, configure these values using environment variables or a secure configuration provider. Do not commit real JWT keys or database credentials to source control.

### Database Setup

From the repository root, apply the Entity Framework Core migrations:

```powershell
dotnet ef database update --project .\LibraryManagement.Api
```

The development environment includes sample data for authors, categories, and books. Sample data is seeded automatically when the application runs in the `Development` environment.

### Run the API

```powershell
dotnet restore
dotnet run --project .\LibraryManagement.Api
```

The API is available at:

- HTTP: `http://localhost:5132`
- HTTPS: `https://localhost:7145`

When running in development mode, interactive API documentation is available at:

```text
https://localhost:7145/scalar
```

## Authentication

1. Register a member using `POST /api/Authentication/register`.
2. Log in using `POST /api/Authentication/login`.
3. Copy the returned JWT token.
4. Send it with protected requests:

```http
Authorization: Bearer <your-token>
```

Librarian-only endpoints require a token issued to a user with the `Librarian` role.

## Running Tests

Run all tests with:

```powershell
dotnet test
```

The solution includes:

- `LibraryManagement.UnitTests` for validators, mappings, request parameters, and domain behavior
- `LibraryManagement.IntegrationTests` for API and database integration scenarios

## Project Structure

```text
LibraryManagement.Api/
├── Controllers/       # HTTP API endpoints
├── DataAccess/        # EF Core DbContext, configurations, and seed data
├── DTOs/              # Request and response models
├── Entities/          # Domain entities and roles
├── Filters/           # Validation and action filters
├── Middleware/        # Request pipeline middleware
├── Services/          # Application business logic
└── RequestParameters/ # Filtering, sorting, and pagination models

LibraryManagement.UnitTests/
LibraryManagement.IntegrationTests/
```

## License

This project is available for educational and development purposes.
