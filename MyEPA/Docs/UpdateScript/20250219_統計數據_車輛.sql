CREATE View [dbo].[vw_OutTown] AS
--(API)¥~³¡¶mÂí
--1.¨®½ø
SELECT Distinct b.Id AS CityId, 9999 AS TownId, a.City, 
       a.Town AS Town, 
	   '(API)' collate SQL_Latin1_General_CP1_CI_AS  AS TownDesc
FROM [dbo].[Vehicle] a
Left Join City b On a.City = b.City
Where Not Exists
(
	Select *
	FROM Town
	JOIN City C ON Town.CityId = C.Id AND IsCounty = 1
	Where a.City = C.City And Town.Name = a.Town
)
GO


