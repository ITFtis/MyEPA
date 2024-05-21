CREATE TABLE [dbo].[Users_Delete_20240521](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[Pwd] [nvarchar](50) NULL,
	[VoicePwd] [nvarchar](50) NULL,
	[Duty] [nvarchar](30) NULL,
	[City] [nvarchar](20) NULL,
	[Town] [nvarchar](30) NULL,
	[MobilePhone] [nvarchar](50) NULL,
	[HumanType] [nvarchar](150) NULL,
	[MainContacter] [nvarchar](2) NULL,
	[ReportPriority] [nvarchar](2) NULL,
	[DepartmentId] [int] NULL,
	[PositionId] [int] NULL,
	[OfficePhone] [nvarchar](50) NULL,
	[FaxNumber] [nvarchar](50) NULL,
	[Email] [nvarchar](100) NULL,
	[Remark] [nvarchar](1000) NULL,
	[HomeNumber] [nvarchar](50) NULL,
	[UpdateDate] [datetime] NULL,
	[CityId] [int] NULL,
	[TownId] [int] NULL,
	[DutyId] [int] NULL,
	[ConfirmTime] [datetime] NULL,
	[isadmin] [bit] NULL,
	[ContactManualDuty] [int] NULL,
	[ContactManualDepartmentId] [int] NULL,
	[ISEnvironmentalProtectionAdministration] [nvarchar](2) NULL,
	[ISEnvironmentalProtectionDepartment] [nvarchar](2) NULL,
	[ISBook] [nvarchar](2) NULL,
 CONSTRAINT [PK_Users_Delete_20240521] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO