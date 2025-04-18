# User Registration

This project implements Authorization and Authentication for Angular by calling a WebAPI written in .NET 9.

---

## AuthECAPI

### Required NPM Packages
Ensure the following NPM packages are installed:

1. **Microsoft.EntityFrameworkCore.SqlServer**
2. **Microsoft.EntityFrameworkCore.Design**
3. **Microsoft.EntityFrameworkCore.Tools**

### Run Migration Command
To apply migrations, use the following command:
```bash
add-migration <Name of commit>
```

USE [AuthECDB]
GO

INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
VALUES 
    (1, 'Admin', 'ADMIN', NULL),
    (2, 'Teacher', 'TEACHER', NULL),
    (3, 'Student', 'STUDENT', NULL);
GO

## AuthECClient
