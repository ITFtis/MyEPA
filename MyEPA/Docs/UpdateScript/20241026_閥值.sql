--閥值
--1.新增資料表(LogDisinfector)


--2.新增資料表(LogDisinfectant)



------------------------------------------------------------------------------------------------------------------
--閥值(消毒設備)
----DB：epaemis_local_20240930_t
--Select 1 AS Id, 999999999 AS DiasterId, 
--       City, Town, ContactUnit, DisinfectInstrument, Standard, Amount, ROCyear, UpdateTime, ConfirmTime, UseType, 
--	   Null AS Duty, Null AS UserTypeDesc, Null AS Type, Null AS Shape, Null AS PowerFrom, Null AS Xpos, Null AS Ypos, Null AS BuyDate, 
--	   UpdateUser, Null AS IsSupportCity, Null AS SupportCityNum,
--	   0 AS CtPoint, GetDate() AS LogBDate, 'system' AS LogUser
--From epaemis_local_20240930_t.dbo.Disinfector 

--閥值(藥品)
----預測資料
--Select 1 AS Id, 999999999 AS DiasterId,
--       City, Town, ContactUnit, DrugName, DrugType, DrugState, Amount, Density, Area, ServiceLife, UpdateTime, ConfirmTime, UseType, ActiveIngredients1, ActiveIngredients2, UpdateUser, 
--	   NULL AS Duty, NULL AS LicenseHas, NULL AS LicenseNo, NULL AS PName, NULL AS PEnvType, NULL AS PType, NULL AS Dosage, NULL AS CompositionQuantity, NULL AS PreventionFun, NULL AS Xpos, NULL AS Ypos, NULL AS BuyDate, NULL AS UnDosage, NULL AS UnCompositionQuantity, 
--	   NULL AS IsSupportCity, NULL AS SupportCityNum,
--	   0 AS CtPoint, GetDate() AS LogBDate, 'system' AS LogUser
--From epaemis_local_20240930_t.dbo.Disinfectant 



--Update LogDisinfector
--Set CtPoint = Convert(float, ISNULL(Amount, 0)) / 2 
--Where DiasterId = 999999999

--Update LogDisinfectant
--Set CtPoint = Convert(float, ISNULL(Amount, 0)) / 2 
--Where DiasterId = 999999999
