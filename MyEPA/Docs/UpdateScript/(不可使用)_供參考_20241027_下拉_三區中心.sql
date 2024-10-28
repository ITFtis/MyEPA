--新增三區代碼
Insert Into Duty(Id, Name)
Select 5 AS Id, '三區中心' AS Name



--縣市
Insert Into City(City, AreaId, IsCounty, WaterDivisionId, Sort, type)
select '三區中心' AS City, 1 AS AreaId, 0 AS IsCounty, 0 AS WaterDivisionId, 1 AS Sort, 3 AS type 
Go

----鄉鎮不需要多一筆三區中心
------鄉鎮
Declare @CityId AS Int
Select @CityId = Id from City Where City = '三區中心'

Insert Into Town(CityId, Name, IsTown)
select @CityId AS CityId, '三區中心北區' AS Name, 0 AS IsTown

Insert Into Town(CityId, Name, IsTown)
select @CityId AS CityId, '三區中心中區' AS Name, 0 AS IsTown

Insert Into Town(CityId, Name, IsTown)
select @CityId AS CityId, '三區中心南區' AS Name, 0 AS IsTown
Go
