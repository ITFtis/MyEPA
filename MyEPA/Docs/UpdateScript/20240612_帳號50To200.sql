--1.EMIS (ÂÂ¯¸)
--Select Name From Registers
alter TABLE [Registers] ALTER COLUMN [Name] [nvarchar](200) NULL

--Select Name From [Users]
alter TABLE [Users] ALTER COLUMN [Name] [nvarchar](200) NULL



--2.DEDS (·s¯¸)
--Select Name From [User]
alter TABLE [User] ALTER COLUMN [Name] [nvarchar](200) NOT NULL

--Select Name From [UserBasic]
alter TABLE [UserBasic] ALTER COLUMN [Name] [nvarchar](200) NULL

--Select Name From [ConUnitPerson]
alter TABLE [ConUnitPerson] ALTER COLUMN [Name] [nvarchar](200) NULL

