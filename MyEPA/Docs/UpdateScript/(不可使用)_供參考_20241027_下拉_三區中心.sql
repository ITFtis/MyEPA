--�s�W�T�ϥN�X
Insert Into Duty(Id, Name)
Select 5 AS Id, '�T�Ϥ���' AS Name



--����
Insert Into City(City, AreaId, IsCounty, WaterDivisionId, Sort, type)
select '�T�Ϥ���' AS City, 1 AS AreaId, 0 AS IsCounty, 0 AS WaterDivisionId, 1 AS Sort, 3 AS type 
Go

----�m���ݭn�h�@���T�Ϥ���
------�m��
Declare @CityId AS Int
Select @CityId = Id from City Where City = '�T�Ϥ���'

Insert Into Town(CityId, Name, IsTown)
select @CityId AS CityId, '�T�Ϥ��ߥ_��' AS Name, 0 AS IsTown

Insert Into Town(CityId, Name, IsTown)
select @CityId AS CityId, '�T�Ϥ��ߤ���' AS Name, 0 AS IsTown

Insert Into Town(CityId, Name, IsTown)
select @CityId AS CityId, '�T�Ϥ��߫n��' AS Name, 0 AS IsTown
Go
