# User Registration

This project demonstrates authentication and authorization in Angular by calling a Web API written with ASP.NET Core. It combines a standalone Angular frontend with a backend that uses Identity, JWT authentication, EF Core, and role- and policy-based authorization.

## Features

### Authentication and Authorization

- JWT-based sign-up and sign-in flow
- Angular route guards and interceptor-based token handling
- Role-based and policy-based authorization on both client and server
- Protected user, timetable, and library/book workflows

### Frontend

- Angular 19 standalone application
- Multi-step registration flow using shared state with `BehaviorSubject`
- Custom claim-based route checks and authorization demos

### Backend

- ASP.NET Core `net8.0` minimal API
- ASP.NET Core Identity with role support
- EF Core with SQL Server / LocalDB
- JWT token issuance with custom claims such as role, gender, age, and optional library membership

## Repository Overview

### `AuthECAPI`

The backend contains the API, Identity setup, EF Core models and migrations, authorization policies, and protected endpoints for profile, timetable, books, and user-management flows.

### `AuthECClient`

The frontend contains the login, registration, forgot-password, and authorization demo pages, along with the route guard, HTTP interceptor, and services that call the API.

## Quick Start

### Prerequisites

- Node.js and npm
- .NET 8 SDK
- SQL Server LocalDB or a compatible SQL Server instance

### Run the API

From `AuthECAPI/AuthECAPI`:

```bash
dotnet restore
dotnet run
```

The development API runs on `http://localhost:5181`.

### Run the Angular App

From `AuthECClient`:

```bash
npm install
npm start
```

The Angular app runs on `http://localhost:4200` and calls the API at `http://localhost:5181/api`.

## Database and Roles

The backend uses EF Core migrations located under `AuthECAPI/AuthECAPI/Migrations`.

To apply the current schema:

```bash
dotnet ef database update
```

The application expects these roles to exist for protected flows:

- `Admin`
- `Teacher`
- `Student`

If you are setting up a fresh database, seed those roles before testing role-restricted features.

## What You Can Explore

- Sign up and sign in with JWT authentication
- Admin-only and teacher/student-specific routes
- Claim-based authorization examples such as library membership and gender/age-based policies
- Book borrowing/submission flows
- Timetable management and user-management scenarios

## Further Docs

If you want deeper setup, architecture, or contributor guidance, use:

- [AGENTS.md](./AGENTS.md) for agent and contributor entrypoints
- [docs/SETUP.md](./docs/SETUP.md) for environment and setup details
- [docs/ARCHITECTURE.md](./docs/ARCHITECTURE.md) for system design
- [docs/API.md](./docs/API.md) for endpoint details
