create procedure [dbo].[backupdb]
--drop procedure [dbo].[backupdb]
as
BACKUP DATABASE [Database1.mdf] TO  DISK = N'C:\backup\test.bak'
  WITH NOFORMAT
     , NOINIT
     , NAME = N'test copy'
     , SKIP
     , NOREWIND
     , NOUNLOAD
     , STATS = 10

     exec [dbo].[backupdb]