# User_Registration
 
Readme

#AuthECAPI
Add Below NPM packages 
1. Microsoft.EntityFrameworkCOre.SqlServer
2. Microsoft.EntityFramewoekCore.Design
3. Microsoft.EntityFramewoekCore.Tools

Run Migration Command
add-migration <Name of commit>

USE [AuthECDB]
GO

INSERT INTO [dbo].[AspNetRoles] 
	([Id],[Name],[NormalizedName],[ConcurrencyStamp])
     VALUES(1,'Admin','ADMIN',NULL)

INSERT INTO [dbo].[AspNetRoles] 
	([Id],[Name],[NormalizedName],[ConcurrencyStamp])
     VALUES(2,'Teacher','TEACHER',NULL)

INSERT INTO [dbo].[AspNetRoles] 
	([Id],[Name],[NormalizedName],[ConcurrencyStamp])
     VALUES(3,'Student','STUDENT',NULL)
GO

#AuthECClient
