# Registration

## Flow

The registration flow is initiated from the Angular client and posts to `POST /api/signup`.

Expected input:

- `Email`
- `Password`
- `FullName`
- `Role`
- `Gender`
- `Age`
- optional `LibraryId`

## Backend Behavior

The API creates an `AppUser`, stores the email as both `Email` and `UserName`, derives `DOB` from `Age`, and assigns the selected role.

## Notes

- Role creation must exist in the database before registration can assign one.
- Additional registration state is also tracked in the client through a shared `BehaviorSubject`.
