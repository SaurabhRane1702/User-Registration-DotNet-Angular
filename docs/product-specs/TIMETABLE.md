# Timetable Workflow

## Available Operations

- Admin creates a timetable row through `POST /api/addtimetable`.
- Teacher or student fetches their own timetable through `GET /api/fetchtimetable`.

## Behavioral Notes

- Timetable creation uses a target user email (`InputEmail`) to associate the row.
- Timetable fetching is filtered by the authenticated user's `userID` claim.
- The API returns `204 No Content` when no timetable entries exist.
