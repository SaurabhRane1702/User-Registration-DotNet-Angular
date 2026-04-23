# Books Workflow

## Available Operations

- Admin adds books through `POST /api/addbooks`.
- Admin or student can fetch the catalog through `GET /api/fetchbooks`.
- Student or teacher can borrow a book through `POST /api/borrowbook?bookId=<id>`.
- Student can fetch their borrowed books through `GET /api/fetchborrowedbooks`.
- Admin or student can submit a borrowed book through `POST /api/submitbooks?bookId=<id>`.

## Behavioral Notes

- Borrowing fails if the book is already marked as borrowed.
- Returning a book clears `BorrowedByEmail`.
- Borrowed-book lookup is based on the authenticated user's email.
