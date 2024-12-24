CREATE PROCEDURE [dbo].[sp_ApiToVehicle]
AS
BEGIN

	--�S��ťղŸ�(Load,EnginePower,ROCyear)
	Declare @spec1 AS nvarchar(100)
	Declare @spec2 AS nvarchar(100)
	Declare @spec3 AS nvarchar(100)
	Declare @spec4 AS nvarchar(100)

	Set @spec1 = N'
	'
	Set @spec2 = N'
'

	Set @spec3 = N'
'

	Set @spec4 = N'
'

	--�ಾ��ƦܫO�d��(VehicleKeepData)
	Delete VehicleKeepData
	From VehicleKeepData a
	Where Exists
	(
		Select AddFrom, AddKey, ConfirmTime, UpdateUser 
		From Vehicle b
		Where a.AddFrom = b.AddFrom And a.AddKey =  b.AddKey
	)

	Insert Into VehicleKeepData(Id, AddFrom, AddKey, ConfirmTime, UpdateUser)
	Select NewId(), AddFrom, AddKey, ConfirmTime, UpdateUser 
	From Vehicle
	--End �ಾ��ƦܫO�d��

	Delete Vehicle 

	--�N�۰�+1���A�q0��_
	DBCC CHECKIDENT('Vehicle', RESEED, 0) 

	Insert Into Vehicle(PlateNumber, City, Town, ContactUnit, VehicleName, VehicleState, UpdateTime, Load, EnginePower, ROCyear, EPAsubsidy, CrossCityUse, CrossTownUse, Note, Xpos, Ypos, VehicleType, Purpose, ConfirmTime, RptKind, VhlRecDiscard,
	                    AddFrom, AddKey)
	Select PlateNumber, City, Town, ContactUnit, 
		   Case When bbb.Id Is Not Null Then bbb.Name Else VehicleName End AS VehicleName, 
		   VehicleState, UpdateTime, Load, EnginePower, ROCyear, EPAsubsidy, CrossCityUse, 
		   CrossTownUse, Note, Xpos, Ypos, VehicleType, Purpose, ConfirmTime, RptKind, VhlRecDiscard,
		   AddFrom, AddKey
	From
	(
		Select Trim(CarNo) As 'PlateNumber',  Trim(CityName) As 'City',  Trim(TownName) As 'Town',  Trim(DepName) As 'ContactUnit',  
			   Trim(OtherVhlRecRptCarKindID) As 'VehicleName',  '�ϥΤ�' As 'VehicleState',  GetDate() As 'UpdateTime',  Trim(Capacity) As 'Load', 
			   Trim(HorsePower) As 'EnginePower',  Trim(BuyYear) As 'ROCyear',  Trim(IsEpaSpr) As 'EPAsubsidy',  Trim(CanSupportCity) As 'CrossCityUse',  
			   Trim(CanSupportCity) As 'CrossTownUse',  Trim(Memo) As 'Note',  
			   TRY_CONVERT([decimal](38, 19), TWD97_X) As 'Xpos',  
			   TRY_CONVERT([decimal](38, 19), TWD97_Y) As 'Ypos',  
			   Trim(OtherVhlRecRptCarKindID) As 'VehicleType',  NULL As 'Purpose',  Null As 'ConfirmTime',  
			   Trim(OtherVhlRecRptCarKindID) As 'RptKind',  'X' As 'VhlRecDiscard',
			   'AR4' AS 'AddFrom', DBID AS 'AddKey'
		From z_AR4_newCarKind
		Union All
		Select Trim(VhlRecCarNo) As 'PlateNumber', Trim(CityName) As 'City', Trim(TownName) As 'Town', '�M�䶤' As 'ContactUnit', 
			   Trim(VhlRecRptCarKindID) As 'VehicleName', '�ϥΤ�' As 'VehicleState', GetDate() As 'UpdateTime', Trim(VhlRecCapacity) As 'Load', 
			   NULL As 'EnginePower', Trim(VhlRecBuyDate) As 'ROCyear', Trim(VhlRecRemark) As 'EPAsubsidy', Trim(VhlRecCanSupportEpa) As 'CrossCityUse',
			   Trim(VhlRecCanSupportCity) As 'CrossTownUse', NULL As 'Note', 
			   TRY_CONVERT([decimal](38, 19), VhlRecTWD97_X) As 'Xpos', 
			   TRY_CONVERT([decimal](38, 19), VhlRecTWD97_Y) As 'Ypos', 
			   Trim(VhlRecRptCarKindID) As 'VehicleType', NULL As 'Purpose', Null As 'ConfirmTime', 
			   Trim(VhlRecRptCarKindID) As 'RptKind', 'X' As 'VhlRecDiscard',
			   'AR5' AS 'AddFrom', VhlRecCarNo AS 'AddKey'
		From z_AR5_newCarKind
	)aaa
	Left Join VehicleType bbb On aaa.VehicleName = bbb.Type

	--�S��ťղŸ�(PlateNumber,Load,EnginePower,ROCyear)
	--Update Vehicle Set xxxxx = '' Where xxxxx = @spec1 Or xxxxx = @spec2 Or xxxxx = @spec3 Or xxxxx = @spec4
	Update Vehicle Set PlateNumber = '' Where PlateNumber = @spec1 Or PlateNumber = @spec2 Or PlateNumber = @spec3 Or PlateNumber = @spec4
	Update Vehicle Set [Load] = '' Where [Load] = @spec1 Or [Load] = @spec2 Or [Load] = @spec3 Or [Load] = @spec4 
	Update Vehicle Set EnginePower = '' Where EnginePower = @spec1 Or EnginePower = @spec2 Or EnginePower = @spec3 Or EnginePower = @spec4
	Update Vehicle Set ROCyear = '' Where ROCyear = @spec1 Or ROCyear = @spec2 Or ROCyear = @spec3 Or ROCyear = @spec4
	Update Vehicle Set ContactUnit = '' Where ContactUnit = @spec1 Or ContactUnit = @spec2 Or ContactUnit = @spec3 Or ContactUnit = @spec4
	Update Vehicle Set EPAsubsidy = '' Where EPAsubsidy = @spec1 Or EPAsubsidy = @spec2 Or EPAsubsidy = @spec3 Or EPAsubsidy = @spec4
	Update Vehicle Set Note = '' Where Note = @spec1 Or Note = @spec2 Or Note = @spec3 Or Note = @spec4
	Update Vehicle Set City = '' Where City = @spec1 Or City = @spec2 Or City = @spec3 Or City = @spec4
	Update Vehicle Set Town = '' Where Town = @spec1 Or Town = @spec2 Or Town = @spec3 Or Town = @spec4

	--�O�d��(VehicleKeepData)�^�s
	Update Vehicle
	Set ConfirmTime = b.ConfirmTime, 
		UpdateUser = b.UpdateUser
	From Vehicle a
	Inner Join VehicleKeepData b On a.AddFrom = b.AddFrom And a.AddKey = b.AddKey

END
GO


