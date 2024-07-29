--1.·s¼WÄæ¦ì()
alter Table [Users] add [PwdUpdateDate] [datetime] NULL
Go

Update [Users]
Set PwdUpdateDate = DATEADD(Day, 90, GETDATE())
Where PwdUpdateDate¡@Is Null
Go

