--1.新增資料表(RecResource)
--2.新增資料表(RecResourceSet)

CREATE TABLE [dbo].[RecResource](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DiasterId] [int] NULL,
	[Type] [int] NOT NULL,
	[CityId] [int] NULL,
	[Reason] [nvarchar](300) NULL,
	[ContactPerson] [nvarchar](50) NULL,
	[ContactMobilePhone] [nvarchar](50) NULL,
	[Items] [int] NULL,
	[Spec] [nvarchar](50) NULL,
	[Quantity] [int] NULL,
	[Unit] [nvarchar](50) NULL,
	[USDate] [datetime] NULL,
	[UEDate] [datetime] NULL,
	[Status] [int] NULL,
	[CreateUser] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_RecResource] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[RecResourceSet](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RecResourceIdNeed] [int] NULL,
	[RecResourceIdHelp] [int] NULL,
	[SetCityId] [int] NULL,
	[SetContactPerson] [nvarchar](50) NULL,
	[SetContactMobilePhone] [nvarchar](50) NULL,
	[SetItems] [int] NULL,
	[SetSpec] [nvarchar](50) NULL,
	[SetQuantity] [int] NULL,
	[SetUnit] [nvarchar](50) NULL,
	[CreateUser] [nvarchar](50) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateUser] [nvarchar](50) NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_RecResourceSet] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO