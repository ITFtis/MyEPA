--�s�W�T�ϥN�X
Insert Into Duty(Id, Name)
Select 5 AS Id, '�T�Ϥ���' AS Name



--����
Insert Into City(City, AreaId, IsCounty, WaterDivisionId, Sort, type)
select '�T�Ϥ���' AS City, 1 AS AreaId, 0 AS IsCounty, 0 AS WaterDivisionId, 1 AS Sort, 3 AS type 
Go

--�m��
Declare @CityId AS Int
Select @CityId = Id from City Where City = '�T�Ϥ���'

Insert Into Town(CityId, Name, IsTown)
select @CityId AS CityId, '�T�Ϥ���' AS Name, 0 AS IsTown
Go

