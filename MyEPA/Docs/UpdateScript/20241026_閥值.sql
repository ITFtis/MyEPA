--閥值
--1.新增資料表(LogDisinfector)


--2.新增資料表(LogDisinfectant)


------------------------------------------------------------------------
------閥值(消毒設備)
------DiasterId int Not Null,  CtPoint int NULL, LogBDate [datetime] NULL, [LogUser] [nvarchar](200) NULL,
----Select * From LogDisinfector Order By Id Desc

------閥值(藥品)
------DiasterId int Not Null,  CtPoint int NULL, LogBDate [datetime] NULL, [LogUser] [nvarchar](200) NULL,
----Select * From LogDisinfectant Order By Id Desc
