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

--閥值(消毒設備)
----DB：epaemis_local_20240930_t
--Select 1 AS Id, 999999999 AS DiasterId, 
--       City, Town, ContactUnit, DisinfectInstrument, Standard, Amount, ROCyear, UpdateTime, ConfirmTime, UseType, UpdateUser,
--	   0 AS CtPoint, GetDate() AS LogBDate, 'system' AS LogUser
--From epaemis_local_20240930_t.dbo.Disinfector 

--閥值(藥品)
----預測資料
--Select 1 AS Id, 999999999 AS DiasterId,
--       City, Town, ContactUnit, DrugName, DrugType, DrugState, Amount, Density, Area, ServiceLife, UpdateTime, ConfirmTime, UseType, ActiveIngredients1, ActiveIngredients2, UpdateUser,
--	   0 AS CtPoint, GetDate() AS LogBDate, 'system' AS LogUser
--From epaemis_local_20240930_t.dbo.Disinfectant 


Update LogDisinfector
Set CtPoint = Convert(float, ISNULL(Amount, 0)) / 2 
Where DiasterId = 999999999

Update LogDisinfectant
Set CtPoint = Convert(float, ISNULL(Amount, 0)) / 2 
Where DiasterId = 999999999