# API

## Summary

All custom endpoints described here are mapped under `/api`. Unless stated otherwise, authentication is required because the backend configures an authorization fallback policy.

## Authentication And Profile

### `POST /api/signup`

- Auth: anonymous
- Body: `Email`, `Password`, `FullName`, `Role`, `Gender`, `Age`, `LibraryId`
- Behavior: creates an `AppUser`, converts `Age` into `DOB`, and assigns the requested role

### `POST /api/signin`

- Auth: anonymous
- Body: `Email`, `Password`
- Behavior: validates credentials and returns `{ token }`
- Token claims: `userID`, `gender`, `age`, role, optional `libraryID`

### `POST /api/forgotpasswordwithemail`

- Auth: anonymous
- Body: `Email`, `OldPassword`, `NewPassword`
- Behavior: changes the password for the matching user if the old password is valid

### `GET /api/UserProfile`

- Auth: authenticated
- Behavior: returns the current user's `Email` and `FullName`

## Timetable

### `POST /api/addtimetable`

- Auth: `Admin`
- Body: `SubjectName`, `ClassName`, `InputEmail`, `TimeActivity`, `WeekDayActivity`, `TeacherSelection`
- Behavior: creates a timetable row for the user identified by `InputEmail`

### `GET /api/fetchtimetable`

- Auth: `Teacher` or `Student`
- Behavior: returns timetable rows for the authenticated user's `userID`
- Responses:
  - `200` with timetable rows
  - `204` when no entries exist

## Books

### `POST /api/addbooks`

- Auth: `Admin`
- Body: `BookTitle`, `Genre`, `IsBorrowed`, optional `BorrowedByEmail`
- Behavior: adds a new book

### `GET /api/fetchbooks`

- Auth: `Admin` or `Student`
- Behavior: returns all books and their borrow state
- Response: `204` when no books exist

### `POST /api/borrowbook?bookId=<id>`

- Auth: `Student` or `Teacher`
- Behavior: marks the requested book as borrowed by the authenticated user

### `GET /api/fetchborrowedbooks`

- Auth: `Student`
- Behavior: returns books borrowed by the authenticated user
- Response: `204` when none exist

### `POST /api/submitbooks?bookId=<id>`

- Auth: `Admin` or `Student`
- Behavior: marks the requested book as returned

## User Management

### `GET /api/fetchallusers`

- Auth: `Admin`
- Behavior: returns users projected into `UserRegistrationModel` shape

### `GET /api/fetchallemail`

- Auth: `Admin`
- Behavior: returns the list of known user email addresses

### `GET /api/fetchuseronemail?email=<email>`

- Auth: `Admin`
- Behavior: returns one user projected into `UserRegistrationModel`

### `PATCH /api/updateuserdetails`

- Auth: `Admin`
- Body: `UserRegistrationModel`
- Behavior: updates `FullName`, `LibraryId`, and `Gender` for the target user

## Authorization Demo Endpoints

### `GET /api/AdminOnly`

- Auth: `Admin`

### `GET /api/AdminOrTeacherOnly`

- Auth: `Admin` or `Teacher`

### `GET /api/LibraryMembersOnly`

- Auth: policy `HasLibraryID`

### `GET /api/ApplyForMaternityLeave`

- Auth: role `Teacher` and policy `FemalesOnly`

### `GET /api/Under10AndFemale`

- Auth: policies `Under10` and `FemalesOnly`

## Notes

- Route casing is mixed in the codebase. The docs keep the exact route names currently mapped by the API.
- The Angular client only calls a subset of the available endpoints directly; some authorization demo pages exist mainly to show policy behavior.
