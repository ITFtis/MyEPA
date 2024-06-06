--API介接資料，整合至舊站
--調整車輛欄位
ALTER TABLE Vehicle ALTER COLUMN EPAsubsidy [nvarchar](255)
Go

--1.新增資料表(AR4_newCarKind,AR5_newCarKind)

--2.代碼資料更新(VehicleType)

--3.新增SP(sp_ApiToVehicle)

--4.執行SP排程

