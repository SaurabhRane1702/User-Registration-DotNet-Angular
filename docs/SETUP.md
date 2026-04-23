# Setup

## Prerequisites

- Node.js and npm for the Angular client
- Angular CLI compatible with Angular 19
- .NET SDK 8.x for the API and tests
- SQL Server LocalDB or a SQL Server instance compatible with the configured connection string

## Default Local Endpoints

- Angular client: `http://localhost:4200`
- API: `http://localhost:5181`
- Frontend API base URL: `http://localhost:5181/api`

These values come from:

- `AuthECClient/src/environments/environment.ts`
- `AuthECClient/src/environments/environment.development.ts`
- `AuthECAPI/AuthECAPI/Properties/launchSettings.json`

## Run The API

From `AuthECAPI/AuthECAPI`:

```bash
dotnet restore
dotnet run
```

The API uses the `http` launch profile and opens Swagger/Scalar on startup in development.

## Run The Angular Client

From `AuthECClient`:

```bash
npm install
npm start
```

This starts the Angular dev server on `http://localhost:4200`.

## Database Configuration

The default connection string in `AuthECAPI/AuthECAPI/appsettings.json` points to:

```text
Server=(LocalDB)\MSSQLLocalDB;Database=AuthECDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;
```

If your local SQL configuration differs, update the `DevDB` connection string before running the API.

## EF Core Migrations

The repo includes multiple migration files under `AuthECAPI/AuthECAPI/Migrations`.

Common commands from `AuthECAPI/AuthECAPI`:

```bash
dotnet ef database update
dotnet ef migrations add <MigrationName>
```

Use `dotnet ef database update` to apply the existing schema to your local database.

## Required Role Seed Data

The backend expects roles to exist for authorization flows:

- `Admin`
- `Teacher`
- `Student`

If the database is fresh, insert the role rows before testing role-restricted features. The repo's earlier setup notes show the expected values in `AspNetRoles`.

## JWT Secret

`AuthECAPI/AuthECAPI/appsettings.json` contains a development JWT secret under `AppSettings:JWTSecret`.

- Keep this synchronized with local development.
- Replace it for any real deployment.

## CORS

The API currently allows requests from:

```text
http://localhost:4200
```

If the Angular app runs on a different origin, update the CORS setup in `Extensions/AppConfigExtensions.cs`.
