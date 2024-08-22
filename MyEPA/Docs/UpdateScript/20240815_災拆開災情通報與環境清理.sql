--1.�s�W���(���ҲM�z CleanDay)
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
--[CLE_DisinfectorL] �W�[�u�w�ϥ��ľ��ƶq(����)�v
--[CLE_DisinfectorW]  �W�[�u�w�ϥ��ľ��ƶq(����v
--[CLE_EquipmentDesc] �W�[�u�w�ϥξ���(�ЦC�ܾ���W�٤μƶq)�v
--[CLE_CarDesc] �W�[�u�w�ϥΨ���(�ЦC�ܨ����W�٤μƶq)�v



alter Table Damage add [CLE_DisinfectorL] [float] NULL
alter Table Damage add [CLE_DisinfectorW] [float] NULL
alter Table Damage add [CLE_EquipmentDesc] Nvarchar(300) NULL
alter Table Damage add [CLE_CarDesc] Nvarchar(300) NULL
