--縣市支援

--1.新增欄位 Disinfector(IsSupportCity)
alter Table Disinfector add [IsSupportCity] [bit] NULL
alter Table Disinfector add [SupportCityNum] [int] NULL


--2.新增欄位 Disinfectant(IsSupportCity)
alter Table Disinfectant add [IsSupportCity] [bit] NULL
alter Table Disinfectant add [SupportCityNum] [int] NULL

--3.新增欄位 Vehicle(IsSupportCity)
alter Table Vehicle add [IsSupportCity] [bit] NULL
alter Table Vehicle add [SupportCityNum] [int] NULL
