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

### Different Angular Features used in the this solution 
1. The solution leverages Angular's advanced capabilities by implementing custom directives to enable role-based access control. These directives dynamically evaluate the user's claims, specifically the 'Role' attribute, to determine and restrict access to specific UI elements based on authorization rules.

   
# AuthECClient

AuthECClient is a feature-rich application that incorporates robust authentication and authorization mechanisms, along with a dynamic multi-step form built using Angular. 

## Features

### Authentication & Authorization
- **JWT Authentication**: Secure authentication using JSON Web Tokens (JWT).
- **Guards for Authorization**: Implemented Angular guards to control access to different parts of the application based on user roles and permissions.

### Multi-Step Angular Form
- **Dynamic Multi-Step Form**: Created a multi-page form workflow that allows users to navigate through multiple steps seamlessly.
- **Shared Services with BehaviorSubject**: Utilized Angular's shared services along with `BehaviorSubject` for managing state across different steps of the form, ensuring smooth data flow and interaction between components.

## Highlights
- Leveraged modern authentication techniques to secure the application.
- Designed an intuitive and user-friendly multi-step form process.
- Ensured modularity and reusability of components with shared services.

Feel free to explore the project and contribute!
