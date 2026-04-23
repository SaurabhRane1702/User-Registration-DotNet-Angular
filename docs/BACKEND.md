# Backend

## Summary

The backend is an ASP.NET Core `net8.0` application using minimal APIs, ASP.NET Core Identity, EF Core, JWT bearer authentication, and SQL Server.

## Bootstrap Flow

`Program.cs` composes the application through extension methods:

- adds controllers
- adds Swagger/Scalar explorer support
- injects `AppDbContext`
- binds `AppSettings`
- configures Identity stores and password settings
- configures JWT bearer authentication and authorization policies
- configures CORS
- maps controllers, Identity API endpoints, and custom minimal API groups

## Extension Responsibilities

- `Extensions/SwaggerExtensions.cs`: Swagger and Scalar setup
- `Extensions/EFCoreExtensions.cs`: SQL Server `AppDbContext` registration
- `Extensions/AppConfigExtensions.cs`: app settings binding and CORS config
- `Extensions/IndentityExtensions.cs`: Identity, JWT auth, fallback policy, and custom authorization policies

## Authentication

Identity is configured with `AddIdentityApiEndpoints<AppUser>()`, role support, and EF Core stores.

Password configuration currently:

- does not require digit
- does not require uppercase
- does not require lowercase
- requires unique email

JWT validation uses `AppSettings:JWTSecret` and enables lifetime validation with zero clock skew.

## Authorization

The backend uses:

- a fallback policy requiring authenticated users by default
- role checks such as `Admin`, `Teacher`, and `Student`
- custom policies:
  - `HasLibraryID`
  - `FemalesOnly`
  - `Under10`

Because of the fallback policy, endpoints are authenticated unless explicitly marked `[AllowAnonymous]`.

## Data Layer

`AppDbContext` stores the application entities used by:

- user identity/profile data
- timetable entries
- books and borrowing state
- library-related identifiers

Migrations live in `AuthECAPI/AuthECAPI/Migrations`.

## API Organization

The application maps:

- built-in Identity API endpoints under `/api`
- custom account endpoint(s) under `/api`
- custom identity/user/book/timetable endpoints under `/api`
- authorization demo endpoints under `/api`

See [API.md](./API.md) for the full endpoint inventory.
