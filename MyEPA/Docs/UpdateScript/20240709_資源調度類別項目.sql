--1.新增資料表及資料(DisinfectorType)
CREATE TABLE [dbo].[DisinfectorType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](200) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_DisinfectorType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--2.新增資料表及資料(DisinfectantType)
CREATE TABLE [dbo].[DisinfectantType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](200) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_DisinfectantType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[DisinfectantType] ON 

INSERT [dbo].[DisinfectantType] ([Id], [Type], [Name]) VALUES (1, N'A01', N'環境消毒(固態Kg)')
INSERT [dbo].[DisinfectantType] ([Id], [Type], [Name]) VALUES (2, N'A02', N'環境消毒(液態L)')
INSERT [dbo].[DisinfectantType] ([Id], [Type], [Name]) VALUES (3, N'A03', N'登革熱(固態Kg)')
INSERT [dbo].[DisinfectantType] ([Id], [Type], [Name]) VALUES (4, N'A04', N'登革熱(液態L)')
SET IDENTITY_INSERT [dbo].[DisinfectantType] OFF
GO
SET IDENTITY_INSERT [dbo].[DisinfectorType] ON 

INSERT [dbo].[DisinfectorType] ([Id], [Type], [Name]) VALUES (1, N'A01', N'噴霧機')
INSERT [dbo].[DisinfectorType] ([Id], [Type], [Name]) VALUES (2, N'A02', N'熱煙霧機')
INSERT [dbo].[DisinfectorType] ([Id], [Type], [Name]) VALUES (3, N'A03', N'高壓清洗機')
INSERT [dbo].[DisinfectorType] ([Id], [Type], [Name]) VALUES (4, N'A04', N'割草機')
INSERT [dbo].[DisinfectorType] ([Id], [Type], [Name]) VALUES (5, N'A05', N'鏈鋸機')
SET IDENTITY_INSERT [dbo].[DisinfectorType] OFF
GO

--3.新增欄位(RecResource => TypeItems)
alter Table RecResource add TypeItems int
alter TABLE RecResource ALTER COLUMN Items  [nvarchar](100)

alter Table RecResourceSet add SetTypeItems int
alter TABLE RecResourceSet ALTER COLUMN SetItems  [nvarchar](100)
Go

--4.更新資料
Update RecResource
Set TypeItems = 1,
     Items = Case When Items = '1' Then 'A01'
        When Items = '2' Then 'B01'
		When Items = '3' Then 'B02'
		When Items = '4' Then 'B03'
		When Items = '5' Then 'B04'
		When Items = '6' Then 'A02'
		When Items = '7' Then 'B05'
		When Items = '8' Then 'B06'
		When Items = '9' Then 'B07'
		When Items = '10' Then 'B08'
		When Items = '11' Then 'B09'
		When Items = '12' Then 'B10'
		When Items = '13' Then 'A03'
		Else Items
	End
Where TypeItems Is Null

Update RecResourceSet
Set SetTypeItems = 1,
    SetItems = Case When SetItems = '1' Then 'A01'
        When SetItems = '2' Then 'B01'
		When SetItems = '3' Then 'B02'
		When SetItems = '4' Then 'B03'
		When SetItems = '5' Then 'B04'
		When SetItems = '6' Then 'A02'
		When SetItems = '7' Then 'B05'
		When SetItems = '8' Then 'B06'
		When SetItems = '9' Then 'B07'
		When SetItems = '10' Then 'B08'
		When SetItems = '11' Then 'B09'
		When SetItems = '12' Then 'B10'
		When SetItems = '13' Then 'A03'
		Else SetItems
	End
Where SetTypeItems Is Null
Go