--�����䴩

--1.�s�W��� Disinfector(IsSupportCity)
alter Table Disinfector add [IsSupportCity] [bit] NULL
alter Table Disinfector add [SupportCityNum] [int] NULL


--2.�s�W��� Disinfectant(IsSupportCity)
alter Table Disinfectant add [IsSupportCity] [bit] NULL
alter Table Disinfectant add [SupportCityNum] [int] NULL

--3.�s�W��� Vehicle(IsSupportCity)
alter Table Vehicle add [IsSupportCity] [bit] NULL
alter Table Vehicle add [SupportCityNum] [int] NULL
