# Seed Data Reference

## Roles

The app expects these roles to exist:

- `Admin`
- `Teacher`
- `Student`

## Legacy SQL Snippet

Earlier repo notes used this role seed pattern:

```sql
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
VALUES
    (1, 'Admin', 'ADMIN', NULL),
    (2, 'Teacher', 'TEACHER', NULL),
    (3, 'Student', 'STUDENT', NULL);
```

Use this as a reference only. If the identity schema changes, prefer an application-level seeding approach.
