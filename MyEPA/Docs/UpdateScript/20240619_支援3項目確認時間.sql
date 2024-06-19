--新增欄位([Users])
alter TABLE [Users] Add UpdateUser Nvarchar(200) collate SQL_Latin1_General_CP1_CI_AS NULL

--新增欄位(Disinfector)
alter TABLE Disinfector Add UpdateUser Nvarchar(200) collate SQL_Latin1_General_CP1_CI_AS NULL
