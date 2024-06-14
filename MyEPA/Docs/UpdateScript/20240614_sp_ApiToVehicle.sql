CREATE PROCEDURE [dbo].[sp_ApiToVehicle]
AS
BEGIN

	--特殊空白符號(Load,EnginePower,ROCyear)
	Declare @spec1 AS nvarchar(100)
	Declare @spec2 AS nvarchar(100)
	Declare @spec3 AS nvarchar(100)

	Set @spec1 = N'
	'
	Set @spec2 = N'
'

	Set @spec3 = N'
'

	Delete Vehicle 

	--將自動+1欄位，從0算起
	DBCC CHECKIDENT('Vehicle', RESEED, 0) 

	Insert Into Vehicle(PlateNumber, City, Town, ContactUnit, VehicleName, VehicleState, UpdateTime, Load, EnginePower, ROCyear, EPAsubsidy, CrossCityUse, CrossTownUse, Note, Xpos, Ypos, VehicleType, Purpose, ConfirmTime, RptKind, VhlRecDiscard)
	Select PlateNumber, City, Town, ContactUnit, 
		   Case When bbb.Id Is Not Null Then bbb.Name Else VehicleName End AS VehicleName, 
		   VehicleState, UpdateTime, Load, EnginePower, ROCyear, EPAsubsidy, CrossCityUse, 
		   CrossTownUse, Note, Xpos, Ypos, VehicleType, Purpose, ConfirmTime, RptKind, VhlRecDiscard
	From
	(
		Select Trim(CarNo) As 'PlateNumber',  Trim(CityName) As 'City',  Trim(TownName) As 'Town',  Trim(DepName) As 'ContactUnit',  
			   Trim(OtherVhlRecRptCarKindID) As 'VehicleName',  '使用中' As 'VehicleState',  GetDate() As 'UpdateTime',  Trim(Capacity) As 'Load', 
			   Trim(HorsePower) As 'EnginePower',  Trim(BuyYear) As 'ROCyear',  Trim(IsEpaSpr) As 'EPAsubsidy',  Trim(CanSupportCity) As 'CrossCityUse',  
			   Trim(CanSupportCity) As 'CrossTownUse',  Trim(Memo) As 'Note',  
			   TRY_CONVERT([decimal](38, 19), TWD97_X) As 'Xpos',  
			   TRY_CONVERT([decimal](38, 19), TWD97_Y) As 'Ypos',  
			   Trim(OtherVhlRecRptCarKindID) As 'VehicleType',  NULL As 'Purpose',  GetDate() As 'ConfirmTime',  
			   Trim(OtherVhlRecRptCarKindID) As 'RptKind',  'X' As 'VhlRecDiscard'
		From z_AR4_newCarKind
		Union All
		Select Trim(VhlRecCarNo) As 'PlateNumber', Trim(CityName) As 'City', Trim(TownName) As 'Town', '清潔隊' As 'ContactUnit', 
			   Trim(VhlRecRptCarKindID) As 'VehicleName', '使用中' As 'VehicleState', GetDate() As 'UpdateTime', Trim(VhlRecCapacity) As 'Load', 
			   NULL As 'EnginePower', Trim(VhlRecBuyDate) As 'ROCyear', Trim(VhlRecRemark) As 'EPAsubsidy', Trim(VhlRecCanSupportEpa) As 'CrossCityUse',
			   Trim(VhlRecCanSupportCity) As 'CrossTownUse', NULL As 'Note', 
			   TRY_CONVERT([decimal](38, 19), VhlRecTWD97_X) As 'Xpos', 
			   TRY_CONVERT([decimal](38, 19), VhlRecTWD97_Y) As 'Ypos', 
			   Trim(VhlRecRptCarKindID) As 'VehicleType', NULL As 'Purpose', GetDate() As 'ConfirmTime', 
			   Trim(VhlRecRptCarKindID) As 'RptKind', 'X' As 'VhlRecDiscard'
		From z_AR5_newCarKind
	)aaa
	Left Join VehicleType bbb On aaa.VehicleName = bbb.Type

	--特殊空白符號(PlateNumber,Load,EnginePower,ROCyear)
	--Update Vehicle Set xxxxx = '' Where xxxxx = @spec1 Or xxxxx = @spec2 Or xxxxx = @spec3
	Update Vehicle Set PlateNumber = '' Where PlateNumber = @spec1 Or PlateNumber = @spec2 Or PlateNumber = @spec3
	Update Vehicle Set [Load] = '' Where [Load] = @spec1 Or [Load] = @spec2 Or [Load] = @spec3 
	Update Vehicle Set EnginePower = '' Where EnginePower = @spec1 Or EnginePower = @spec2 Or EnginePower = @spec3
	Update Vehicle Set ROCyear = '' Where ROCyear = @spec1 Or ROCyear = @spec2 Or ROCyear = @spec3
	Update Vehicle Set ContactUnit = '' Where ContactUnit = @spec1 Or ContactUnit = @spec2 Or ContactUnit = @spec3
	Update Vehicle Set EPAsubsidy = '' Where EPAsubsidy = @spec1 Or EPAsubsidy = @spec2 Or EPAsubsidy = @spec3
	Update Vehicle Set Note = '' Where Note = @spec1 Or Note = @spec2 Or Note = @spec3
	Update Vehicle Set City = '' Where City = @spec1 Or City = @spec2 Or City = @spec3
	Update Vehicle Set Town = '' Where Town = @spec1 Or Town = @spec2 Or Town = @spec3

END
GO


