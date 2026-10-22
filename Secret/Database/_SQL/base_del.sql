EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'RestAndRant'
GO

USE [master]
GO

ALTER DATABASE [RestAndRant] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE
GO

USE [master]
GO

DROP DATABASE [RestAndRant]
GO
