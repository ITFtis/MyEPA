--Select * From Vehicle
--Select * From VehicleKeepData

--1.新增欄位Vehicle(AddFrom, AddKey, UpdateUser)
alter TABLE Vehicle Add AddFrom Nvarchar(50) NULL
alter TABLE Vehicle Add AddKey Nvarchar(50) NULL
alter TABLE Vehicle Add UpdateUser Nvarchar(200) collate SQL_Latin1_General_CP1_CI_AS NULL
Go

--2.新增資料確認暫存資料表VehicleKeepData(Id, AddFrom, AddKey, ConfirmTime, UpdateUser)
CREATE TABLE [dbo].[VehicleKeepData](
	[Id] [uniqueidentifier] NOT NULL,
	[AddFrom] [nvarchar](50) NULL,
	[AddKey] [nvarchar](50) NULL,
	[ConfirmTime] [datetime] NULL,
	[UpdateUser] [nvarchar](200) NULL,
 CONSTRAINT [PK_VehicleKeepData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

alter TABLE VehicleKeepData alter Column UpdateUser Nvarchar(200) collate SQL_Latin1_General_CP1_CI_AS NULL
Go


--3.修改SP(sp_ApiToVehicle)

