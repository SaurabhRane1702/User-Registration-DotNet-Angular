# Authorization

## Summary

Authorization is enforced in two layers:

- server-side in ASP.NET Core through roles and policies
- client-side in Angular through route claim predicates for UX guidance

The server remains the source of truth.

## Roles

The current system expects these roles to exist:

- `Admin`
- `Teacher`
- `Student`

They are assigned during user registration and then embedded into the issued JWT.

## JWT Claims Used By The App

The custom sign-in endpoint adds:

- `userID`
- `gender`
- `age`
- role claim
- optional `libraryID`

The frontend decodes those claims from the token payload using `window.atob(...)`.

## Backend Policies

Defined in `Extensions/IndentityExtensions.cs`:

- `HasLibraryID`: requires the `libraryID` claim
- `FemalesOnly`: requires `gender = Female`
- `Under10`: requires numeric `age < 10`

## Frontend Claim Predicates

Defined in `AuthECClient/src/app/shared/utils/claimReq-utils.ts`:

- `adminOnly`
- `adminOrTeacher`
- `studentOnly`
- `hasLibraryId`
- `femaleAndTeacher`
- `femaleAndBelow10`

These are attached to route metadata and evaluated by `authGuard`.

## Route And Endpoint Crosswalk

- Admin-only pages and API actions:
  - `/admin-only`
  - `/admin-timeTable`
  - `/view-users`
  - `/add-book`
  - `/api/addtimetable`
  - `/api/addbooks`
  - `/api/fetchallusers`
  - `/api/fetchallemail`
  - `/api/fetchuseronemail`
  - `/api/updateuserdetails`
  - `/api/AdminOnly`
- Admin-or-teacher:
  - `/admin-or-teacher`
  - `/api/AdminOrTeacherOnly`
- Student-only routes:
  - `/view-time-table`
  - `/view-books`
  - `/submit-books`
- Claim/policy demos:
  - `/library-members-only` <-> `HasLibraryID`
  - `/apply-for-maternity-leave` <-> `Teacher` + `FemalesOnly`
  - `/under10-and-female` <-> `Under10` + `FemalesOnly`

## Important Caveat

The client uses token claims to decide whether to allow navigation, but a user can still bypass client checks. Any new protected feature must keep the backend authorization rule in place even if the frontend also checks claims.
