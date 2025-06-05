USE [master];
GO

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'EmployeesService')
BEGIN
    CREATE DATABASE [EmployeesService];
END

GO
