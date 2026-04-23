# API Endpoints Reference

## Anonymous

- `POST /api/signup`
- `POST /api/signin`
- `POST /api/forgotpasswordwithemail`

## Authenticated

- `GET /api/UserProfile`

## Admin

- `POST /api/addtimetable`
- `POST /api/addbooks`
- `GET /api/fetchallusers`
- `GET /api/fetchallemail`
- `GET /api/fetchuseronemail`
- `PATCH /api/updateuserdetails`
- `GET /api/AdminOnly`

## Admin Or Teacher

- `GET /api/AdminOrTeacherOnly`

## Student Or Teacher

- `POST /api/borrowbook?bookId=<id>`

## Teacher Or Student

- `GET /api/fetchtimetable`

## Admin Or Student

- `GET /api/fetchbooks`
- `POST /api/submitbooks?bookId=<id>`

## Student

- `GET /api/fetchborrowedbooks`

## Policy-Based

- `GET /api/LibraryMembersOnly`
- `GET /api/ApplyForMaternityLeave`
- `GET /api/Under10AndFemale`
