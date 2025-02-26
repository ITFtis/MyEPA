--開口合約縣市鄉鎮資料修正

--備份
Select *
Into OpenContract_20250226
From OpenContract
Go

--1.部分縣市環保局鄉鎮代碼錯誤(368)
--Select a.CityId, a.TownId, b.rTownId
Update OpenContract
Set TownId = b.rTownId
From OpenContract a
Inner Join
(
	Select CityId, Id AS rTownId
	From Town
	Where Name = '環保局'
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
	Where Name = '環保局'
)b On a.CityId = b.CityId
Where a.TownId Is Null Or a.TownId = 0

