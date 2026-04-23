# Frontend

## Summary

The Angular app is a standalone Angular 19 application focused on authentication, route protection, and authorization demos.

## App Structure

- `src/app/app.config.ts`: registers router, HTTP interceptor, animations, and toastr.
- `src/app/app.routes.ts`: route table for public pages, protected pages, and fallback behavior.
- `src/app/layouts/main-layout/`: shell for authenticated pages.
- `src/app/user/`: sign-up, sign-in, forgot-password, and additional registration details flows.
- `src/app/authorizeDemo/`: feature pages gated by roles or claims.
- `src/app/shared/`: guard, interceptor, services, constants, directives, pipes, and claim helpers.

## Routing Model

Public routes live under `UserComponent`:

- `/signup`
- `/signin`
- `/forgotpassword`
- `/additionalRegDetails`

Authenticated routes live under `MainLayoutComponent` and are protected by `authGuard`:

- `/dashboard`
- `/admin-only`
- `/view-time-table`
- `/view-books`
- `/submit-books`
- `/admin-timeTable`
- `/view-users`
- `/admin-or-teacher`
- `/apply-for-maternity-leave`
- `/library-members-only`
- `/under10-and-female`
- `/add-book`
- `/forbidden`

Unknown routes fall back to `LoginComponent`.

## Auth Flow

`AuthService` is responsible for:

- posting sign-up data to `/signup`
- posting sign-in data to `/signin`
- posting password-change data to `/forgotpasswordwithemail`
- storing the JWT in `localStorage`
- decoding claims from the JWT payload

The client treats the token as the local session source of truth.

## Protected Requests

`authInterceptor` adds the bearer token to outgoing requests when one exists.

- `401`: delete token, show a session-expired toast, redirect to `/signin`
- `403`: show an unauthorized toast

## Claim-Based Navigation

`authGuard` first checks whether the user is logged in. If the route has a `claimReq` function in route data, it decodes the token claims and runs the predicate before allowing access.

Current client predicates in `claimReq-utils.ts`:

- admin only
- admin or teacher
- student only
- has library membership claim
- female teacher
- female and under 10

These mirror backend role/policy checks but do not replace server-side enforcement.

## Client-Service Surface

`UserService` wraps the main authenticated API calls:

- profile lookup
- timetable fetch/create
- users fetch/email lookup/update
- books fetch/add/borrow/submit

It also stores in-progress multi-step registration data in a `BehaviorSubject`.

## Environment

Both environment files currently point to:

```text
http://localhost:5181/api
```
