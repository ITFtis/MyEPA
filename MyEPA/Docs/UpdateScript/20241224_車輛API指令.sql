--車輛API指令
alter Table Vehicle add [WriteTime] [datetime] NULL
Go

--新增Table(z_AR4_newCarKind)
CREATE TABLE [dbo].[z_AR4_newCarKind](
	[id] [int] NULL,
	[updDate] [nvarchar](255) NULL,
	[DBID] [nvarchar](255) NULL,
	[ZipID] [nvarchar](255) NULL,
	[CityName] [nvarchar](255) NULL,
	[TownName] [nvarchar](255) NULL,
	[DepName] [nvarchar](255) NULL,
	[AssetNo] [nvarchar](255) NULL,
	[VhlName] [nvarchar](255) NULL,
	[VhlCount] [nvarchar](255) NULL,
	[VhlKindName] [nvarchar](255) NULL,
	[OtherVhlRecRptCarKindID] [nvarchar](255) NULL,
	[CarNo] [nvarchar](255) NULL,
	[Capacity] [nvarchar](255) NULL,
	[HorsePower] [nvarchar](255) NULL,
	[UseMemo] [nvarchar](255) NULL,
	[BuyYear] [nvarchar](255) NULL,
	[IsEpaSpr] [nvarchar](255) NULL,
	[CarNow] [nvarchar](255) NULL,
	[CanSupportCity] [nvarchar](255) NULL,
	[CanSupportEpa] [nvarchar](255) NULL,
	[Memo] [nvarchar](255) NULL,
	[TWD97_X] [nvarchar](255) NULL,
	[TWD97_Y] [nvarchar](255) NULL,
	[IsDeleted] [nvarchar](255) NULL,
	[DeletedDate] [nvarchar](255) NULL,
	[WriteTime] [datetime] NULL
) ON [PRIMARY]
GO

--新增Table(z_AR5_newCarKind)
CREATE TABLE [dbo].[z_AR5_newCarKind](
	[id] [int] NULL,
	[VhlRecUptDate] [nvarchar](255) NULL,
	[VhlRecCmpRecID] [nvarchar](255) NULL,
	[CityName] [nvarchar](255) NULL,
	[TownName] [nvarchar](255) NULL,
	[VhlRecCarNo] [nvarchar](255) NULL,
	[VhlRecModel] [nvarchar](255) NULL,
	[VhlRecCompany] [nvarchar](255) NULL,
	[VhlRecVhlBotCmpID] [nvarchar](255) NULL,
	[VhlRecBotOtCountry] [nvarchar](255) NULL,
	[VhlRecBotOtManufacturer] [nvarchar](255) NULL,
	[VhlRecVhlBdyCmpID] [nvarchar](255) NULL,
	[VhlRecBdyOtCountry] [nvarchar](255) NULL,
	[VhlRecBotOtManufacturer1] [nvarchar](255) NULL,
	[VhlRecPrdDate] [nvarchar](255) NULL,
	[VhlRecBuyDate] [nvarchar](255) NULL,
	[VhlRecRptCarKindID] [nvarchar](255) NULL,
	[VhlRecCapacity] [nvarchar](255) NULL,
	[VhlRecCapOtNote] [nvarchar](255) NULL,
	[VhlRecGear] [nvarchar](255) NULL,
	[VhlRecGearCountF] [nvarchar](255) NULL,
	[VhlRecExhaust] [nvarchar](255) NULL,
	[VhlRecFuel] [nvarchar](255) NULL,
	[VhlRecFuelAdd] [nvarchar](255) NULL,
	[VhlRecSeat] [nvarchar](255) NULL,
	[VhlRecLoad] [nvarchar](255) NULL,
	[VhlRecGrossWeight] [nvarchar](255) NULL,
	[VhlRecAdditionItem] [nvarchar](255) NULL,
	[VhlRecAdditionItemOtNote] [nvarchar](255) NULL,
	[VhlRecBuyCompany] [nvarchar](255) NULL,
	[VhlRecRealBuyDate] [nvarchar](255) NULL,
	[VhlRecBuyPrice] [nvarchar](255) NULL,
	[VhlRecWarrantyDate] [nvarchar](255) NULL,
	[VhlRecBuyWayID] [nvarchar](255) NULL,
	[VhlRecBuyWayOtNote] [nvarchar](255) NULL,
	[VhlRecBuyMoneyFrom] [nvarchar](255) NULL,
	[VhlRecBuyMoneyFromOtNote] [nvarchar](255) NULL,
	[VhlRecDiscardDate] [nvarchar](255) NULL,
	[VhlRecDiscardReason] [nvarchar](255) NULL,
	[VhlRecDiscardReasonNote] [nvarchar](255) NULL,
	[VhlRecDiscard] [nvarchar](255) NULL,
	[VhlRecDiscardMoney] [nvarchar](255) NULL,
	[R_Year] [nvarchar](255) NULL,
	[R_NewCarNo] [nvarchar](255) NULL,
	[R_RenewYear] [nvarchar](255) NULL,
	[VhlRecRemark] [nvarchar](255) NULL,
	[VhlRecCanSupportEpa] [nvarchar](255) NULL,
	[VhlRecCanSupportCity] [nvarchar](255) NULL,
	[VhlRecTWD97_X] [nvarchar](255) NULL,
	[VhlRecTWD97_Y] [nvarchar](255) NULL,
	[VhlRecRegYear] [nvarchar](255) NULL,
	[VhlRecCatID] [nvarchar](255) NULL,
	[VhlRecUseCondition] [nvarchar](255) NULL,
	[WriteTime] [datetime] NULL
) ON [PRIMARY]
GO

--新增SP(sp_ApiToVehicle)
CREATE PROCEDURE [dbo].[sp_ApiToVehicle]
AS
BEGIN

	--特殊空白符號(Load,EnginePower,ROCyear)
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

	--轉移資料至保留區(VehicleKeepData)
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
	--End 轉移資料至保留區

	Delete Vehicle 

	--將自動+1欄位，從0算起
	DBCC CHECKIDENT('Vehicle', RESEED, 0) 

	Insert Into Vehicle(PlateNumber, City, Town, ContactUnit, VehicleName, VehicleState, UpdateTime, Load, EnginePower, ROCyear, EPAsubsidy, CrossCityUse, CrossTownUse, Note, Xpos, Ypos, VehicleType, Purpose, ConfirmTime, RptKind, VhlRecDiscard,
	                    AddFrom, AddKey, WriteTime)
	Select PlateNumber, City, Town, ContactUnit, 
		   Case When bbb.Id Is Not Null Then bbb.Name Else VehicleName End AS VehicleName, 
		   VehicleState, UpdateTime, Load, EnginePower, ROCyear, EPAsubsidy, CrossCityUse, 
		   CrossTownUse, Note, Xpos, Ypos, VehicleType, Purpose, ConfirmTime, RptKind, VhlRecDiscard,
		   AddFrom, AddKey, WriteTime
	From
	(
		Select Trim(CarNo) As 'PlateNumber',  Trim(CityName) As 'City',  Trim(TownName) As 'Town',  Trim(DepName) As 'ContactUnit',  
			   Trim(OtherVhlRecRptCarKindID) As 'VehicleName',  '使用中' As 'VehicleState',  GetDate() As 'UpdateTime',  Trim(Capacity) As 'Load', 
			   Trim(HorsePower) As 'EnginePower',  Trim(BuyYear) As 'ROCyear',  Trim(IsEpaSpr) As 'EPAsubsidy',  Trim(CanSupportCity) As 'CrossCityUse',  
			   Trim(CanSupportCity) As 'CrossTownUse',  Trim(Memo) As 'Note',  
			   TRY_CONVERT([decimal](38, 19), TWD97_X) As 'Xpos',  
			   TRY_CONVERT([decimal](38, 19), TWD97_Y) As 'Ypos',  
			   Trim(OtherVhlRecRptCarKindID) As 'VehicleType',  NULL As 'Purpose',  Null As 'ConfirmTime',  
			   Trim(OtherVhlRecRptCarKindID) As 'RptKind',  'X' As 'VhlRecDiscard',
			   'AR4' AS 'AddFrom', DBID AS 'AddKey', WriteTime
		From z_AR4_newCarKind
		Union All
		Select Trim(VhlRecCarNo) As 'PlateNumber', Trim(CityName) As 'City', Trim(TownName) As 'Town', '清潔隊' As 'ContactUnit', 
			   Trim(VhlRecRptCarKindID) As 'VehicleName', '使用中' As 'VehicleState', GetDate() As 'UpdateTime', Trim(VhlRecCapacity) As 'Load', 
			   NULL As 'EnginePower', Trim(VhlRecBuyDate) As 'ROCyear', Trim(VhlRecRemark) As 'EPAsubsidy', Trim(VhlRecCanSupportEpa) As 'CrossCityUse',
			   Trim(VhlRecCanSupportCity) As 'CrossTownUse', NULL As 'Note', 
			   TRY_CONVERT([decimal](38, 19), VhlRecTWD97_X) As 'Xpos', 
			   TRY_CONVERT([decimal](38, 19), VhlRecTWD97_Y) As 'Ypos', 
			   Trim(VhlRecRptCarKindID) As 'VehicleType', NULL As 'Purpose', Null As 'ConfirmTime', 
			   Trim(VhlRecRptCarKindID) As 'RptKind', 'X' As 'VhlRecDiscard',
			   'AR5' AS 'AddFrom', VhlRecCarNo AS 'AddKey', WriteTime
		From z_AR5_newCarKind
	)aaa
	Left Join VehicleType bbb On aaa.VehicleName = (bbb.Type collate Chinese_Taiwan_Stroke_CI_AS)

	--特殊空白符號(PlateNumber,Load,EnginePower,ROCyear)
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

	--保留區(VehicleKeepData)回存
	Update Vehicle
	Set ConfirmTime = b.ConfirmTime, 
		UpdateUser = b.UpdateUser
	From Vehicle a
	Inner Join VehicleKeepData b On a.AddFrom = b.AddFrom And a.AddKey = b.AddKey

END
GO
