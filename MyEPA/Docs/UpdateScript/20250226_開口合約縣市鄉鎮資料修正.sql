--�}�f�X�������m���ƭץ�

--�ƥ�
Select *
Into OpenContract_20250226
From OpenContract
Go

--1.�����������O���m��N�X���~(368)
--Select a.CityId, a.TownId, b.rTownId
Update OpenContract
Set TownId = b.rTownId
From OpenContract a
Inner Join
(
	Select CityId, Id AS rTownId
	From Town
	Where Name = '���O��'
)b On a.CityId = b.CityId
Where a.TownId = 368

--2.TownId Null
Update OpenContract
Set TownId = b.rTownId
From OpenContract a
Inner Join
(
	Select CityId, Id AS rTownId
	From Town
	Where Name = '���O��'
)b On a.CityId = b.CityId
Where a.TownId Is Null Or a.TownId = 0

