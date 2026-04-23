# Architecture

## Summary

This repository contains an Angular frontend and an ASP.NET Core backend that work together to demonstrate authentication, authorization, and a few protected application workflows.

- Frontend: Angular 19 frontend in `AuthECClient`
- Backend: ASP.NET Core minimal API in `AuthECAPI/AuthECAPI`
- Data layer: EF Core with SQL Server / LocalDB
- Auth: ASP.NET Core Identity + JWT bearer tokens

## System Context

The user signs up or signs in through Angular forms. The Angular client sends requests to the backend under `/api`. On successful sign-in, the backend issues a JWT containing role- and profile-derived claims. The frontend stores that token in `localStorage`, attaches it to subsequent requests through an HTTP interceptor, and uses the claims to control navigation and UI access.

## Main Subsystems

### Frontend

- `src/app/app.routes.ts`: public and protected route definitions.
- `src/app/shared/services/auth.service.ts`: sign-up, sign-in, forgot-password, token storage, and claim decoding.
- `src/app/shared/services/user.service.ts`: protected API calls for profile, users, books, and timetable flows.
- `src/app/shared/auth.guard.ts`: gate for authenticated and claim-restricted routes.
- `src/app/shared/auth.interceptor.ts`: attaches bearer token and handles `401`/`403`.
- `src/app/shared/utils/claimReq-utils.ts`: client-side claim predicates used by route data.

### Backend

- `Program.cs`: service registration, middleware, endpoint mapping, Scalar/Swagger setup.
- `Extensions/`: extension methods for Swagger, EF Core, app config, Identity, JWT auth, CORS, and authorization policies.
- `Controllers/IdentityUserEndpoints.cs`: auth, user, timetable, and book handlers.
- `Controllers/AccountEndpoints.cs`: authenticated profile endpoint.
- `Controllers/AuthorizationDemoEndpoints.cs`: role/policy demo endpoints.
- `Models/`: `AppUser`, EF Core entities, settings, and `AppDbContext`.

## Request Flow

1. Angular calls the API using `environment.apiBaseUrl`.
2. The interceptor adds `Authorization: Bearer <token>` when a token exists.
3. ASP.NET Core validates the JWT using the configured `AppSettings:JWTSecret`.
4. Minimal API handlers apply role or policy checks.
5. The frontend route guard separately checks decoded claims before allowing navigation to protected views.

## Authorization Model

The backend is the final authority for access control. The frontend mirrors that logic to improve UX by hiding or blocking routes before the request is made.

- Roles: `Admin`, `Teacher`, `Student`
- Claims in JWT: `userID`, `gender`, `age`, role claim, and optional `libraryID`
- Policies: `HasLibraryID`, `FemalesOnly`, `Under10`

See [AUTHORIZATION.md](./AUTHORIZATION.md) for the crosswalk.
