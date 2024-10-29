--閥值重建，定序問題
Drop Table LogDisinfector
Drop Table LogDisinfectant

--閥值
--1.新增資料表(LogDisinfector)
CREATE TABLE [dbo].[LogDisinfector](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DiasterId] [int] NOT NULL,
	[City] [nvarchar](20) collate SQL_Latin1_General_CP1_CI_AS NULL,
	[Town] [nvarchar](30) collate SQL_Latin1_General_CP1_CI_AS NULL,
	[ContactUnit] [nvarchar](30) collate SQL_Latin1_General_CP1_CI_AS NULL,
	[DisinfectInstrument] [nvarchar](100) collate SQL_Latin1_General_CP1_CI_AS NULL,
	[Standard] [nvarchar](100) NULL,
	[Amount] [int] NULL,
	[ROCyear] [varchar](3) NULL,
	[UpdateTime] [datetime] NULL,
	[ConfirmTime] [datetime] NULL,
	[UseType] [int] NULL,
	[Duty] [nvarchar](30) NULL,
	[UserTypeDesc] [nvarchar](1000) NULL,
	[Type] [nvarchar](200) NULL,
	[Shape] [nvarchar](200) NULL,
	[PowerFrom] [nvarchar](200) NULL,
	[Xpos] [float] NULL,
	[Ypos] [float] NULL,
	[BuyDate] [datetime] NULL,
	[UpdateUser] [nvarchar](200) NULL,
	[IsSupportCity] [bit] NULL,
	[SupportCityNum] [int] NULL,
	[CtPoint] [float] NULL,
	[LogBDate] [datetime] NULL,
	[LogBUser] [nvarchar](200) NULL,
 CONSTRAINT [PK_LogDisinfector] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--2.新增資料表(LogDisinfectant)
CREATE TABLE [dbo].[LogDisinfectant](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DiasterId] [int] NOT NULL,
	[City] [nvarchar](20) NULL,
	[Town] [nvarchar](30) NULL,
	[ContactUnit] [nvarchar](30) NULL,
	[DrugName] [nvarchar](30) NULL,
	[DrugType] [nvarchar](5) NULL,
	[DrugState] [nvarchar](2) NULL,
	[Amount] [decimal](18, 2) NULL,
	[Density] [varchar](6) NULL,
	[Area] [decimal](18, 2) NULL,
	[ServiceLife] [datetime] NULL,
	[UpdateTime] [datetime] NULL,
	[ConfirmTime] [datetime] NULL,
	[UseType] [int] NULL,
	[ActiveIngredients1] [nvarchar](20) NULL,
	[ActiveIngredients2] [nvarchar](20) NULL,
	[UpdateUser] [nvarchar](200) NULL,
	[Duty] [nvarchar](30) NULL,
	[LicenseHas] [nvarchar](1) NULL,
	[LicenseNo] [nvarchar](100) NULL,
	[PName] [nvarchar](100) NULL,
	[PEnvType] [nvarchar](200) NULL,
	[PType] [nvarchar](200) NULL,
	[Dosage] [nvarchar](200) NULL,
	[CompositionQuantity] [nvarchar](200) NULL,
	[PreventionFun] [nvarchar](100) NULL,
	[Xpos] [float] NULL,
	[Ypos] [float] NULL,
	[BuyDate] [datetime] NULL,
	[UnDosage] [nvarchar](200) NULL,
	[UnCompositionQuantity] [nvarchar](200) NULL,
	[IsSupportCity] [bit] NULL,
	[SupportCityNum] [int] NULL,
	[CtPoint] [float] NULL,
	[LogBDate] [datetime] NULL,
	[LogBUser] [nvarchar](200) NULL,
 CONSTRAINT [PK_LogDisinfectant] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

----------------------------------------------
