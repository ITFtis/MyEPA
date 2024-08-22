--1.新增欄位(環境清理 CleanDay)
alter Table Damage add CleanDay [date] NULL
Go

Update Damage
Set CleanDay = ReportDay
Go

alter Table Damage add IsDamageClean [bit] NULL
Go

Update Damage
Set IsDamageClean = IsDamage
Go

----------------------
alter Table Damage add CleanConfirmTime [datetime] NULL
alter Table Damage add CleanTeamConfirmTime [datetime2](7) NULL
alter Table Damage add CleanStatus [int] NULL
alter Table Damage add CleanCreateDate [datetime] NULL
alter Table Damage add CleanUpdateDate [datetime] NULL
Go

Update Damage
Set CleanConfirmTime = ConfirmTime,
    CleanTeamConfirmTime = TeamConfirmTime,
	CleanStatus = [Status],
	CleanCreateDate = CreateDate,
    CleanUpdateDate = UpdateDate
Go

-----------------------------------------------------------
--2.
--[COUNTY_NAME] [nvarchar](50) NULL,
--[CLE_DisinfectorL] 增加「已使用藥劑數量(公升)」
--[CLE_DisinfectorW]  增加「已使用藥劑數量(公斤」
--[CLE_EquipmentDesc] 增加「已使用機具(請列示機具名稱及數量)」
--[CLE_CarDesc] 增加「已使用車輛(請列示車輛名稱及數量)」



alter Table Damage add [CLE_DisinfectorL] [float] NULL
alter Table Damage add [CLE_DisinfectorW] [float] NULL
alter Table Damage add [CLE_EquipmentDesc] Nvarchar(300) NULL
alter Table Damage add [CLE_CarDesc] Nvarchar(300) NULL
