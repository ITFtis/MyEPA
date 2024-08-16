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