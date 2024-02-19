using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class ResourcesReportRepository : BaseEMISRepository
    {
        public List<ResourcesReportCityModel> GetResourcesCityReport()
        {

            string townSql = GetResourcesReportSQL();
            string sql = $@"
SELECT
    CityId
	,City
	, SUM(UserCount) UserCount
	, SUM(VehicleCount) VehicleCount
	, SUM(DisinfectorCount) DisinfectorCount
	, SUM(DisinfectantSolidAmount) DisinfectantSolidAmount
	, SUM(DisinfectantLiquidAmount) DisinfectantLiquidAmount
	, SUM(PestCount) PestCount
	, SUM(DumpCount) DumpCount
	, SUM(ToiletCount) ToiletCount
	, SUM(VolunteerCount) VolunteerCount
	, OwnerName
	, OwnerMobilePhone
	, MAX(VehicleConfirmTime)VehicleConfirmTime
	, MAX(VehicleUpdateTime)VehicleUpdateTime
	, MAX(UsersConfirmTime)UsersConfirmTime
	, MAX(UsersUpdateTime)UsersUpdateTime
	, MAX(DisinfectorConfirmTime)DisinfectorConfirmTime
	, MAX(DisinfectorUpdateTime)DisinfectorUpdateTime
	, MAX(DisinfectantConfirmTime)DisinfectantConfirmTime
	, MAX(DisinfectantUpdateTime)DisinfectantUpdateTime
	, MAX(PestConfirmTime)PestConfirmTime
	, MAX(PestUpdateTime)PestUpdateTime
	, MAX(DumpConfirmTime)DumpConfirmTime
	, MAX(DumpUpdateTime)DumpUpdateTime
	, MAX(ToiletConfirmTime)ToiletConfirmTime
	, MAX(ToiletUpdateTime)ToiletUpdateTime
	, MAX(VolunteerConfirmTime)VolunteerConfirmTime
	, MAX(VolunteerUpdateTime)	VolunteerUpdateTime
FROM
(
    {townSql}
) AS CityReport
GROUP BY CityReport.City,CityReport.CityId,CityReport.OwnerName,CityReport.OwnerMobilePhone,CityReport.Sort
ORDER BY CityReport.Sort";
            return GetListBySQL<ResourcesReportCityModel>(sql);
        }


        public List<ResourcesReportTownModel> GetResourcesTownReport(int? cityId = null)
        {
            string sql = GetResourcesReportSQL();

            if(cityId.HasValue)
            {
                sql += " WHERE C.Id = @CityId";
            }

            return GetListBySQL<ResourcesReportTownModel>(sql, new { CityId = cityId });
        }

        private string GetResourcesReportSQL()
        {
            string sql = $@"

SELECT
	C.Id CityId
	,C.City
	,C.Sort
    ,Town.Id TownId
	,Town.Name Town
	,ISNULL(U.Count,0) UserCount							    --人員組織
	,ISNULL(V.Count,0) VehicleCount							    --車輛設備
	,ISNULL(Dfector.Count,0) DisinfectorCount				    --消毒設備
	,ISNULL(Dfectant.SolidCount,0) DisinfectantSolidAmount	    --消毒藥品固體
	,ISNULL(Dfectant.LiquidCount,0) DisinfectantLiquidAmount    --消毒藥品液體
	,ISNULL(P.Count,0) PestCount							    --病媒防治業查閱
	,ISNULL(D.Count,0) DumpCount							    --臨時廢棄物堆置場
	,ISNULL(T.Count,0) ToiletCount							    --流動廁所
	,ISNULL(Vlt.Count,0) VolunteerCount						    --民間志工
	,UMain.Name OwnerName
	,UMain.MobilePhone  OwnerMobilePhone
	,V.ConfirmTime VehicleConfirmTime
	,V.UpdateTime VehicleUpdateTime
	,U.ConfirmTime UsersConfirmTime
	,U.UpdateTime UsersUpdateTime
	,Dfector.ConfirmTime DisinfectorConfirmTime
	,Dfector.UpdateTime DisinfectorUpdateTime
	,Dfectant.ConfirmTime DisinfectantConfirmTime
	,Dfectant.UpdateTime DisinfectantUpdateTime
	,P.ConfirmTime PestConfirmTime
	,P.UpdateTime PestUpdateTime
	,D.ConfirmTime DumpConfirmTime
	,D.UpdateTime DumpUpdateTime
	,T.ConfirmTime ToiletConfirmTime
	,T.UpdateTime ToiletUpdateTime
	,Vlt.ConfirmTime VolunteerConfirmTime
	,Vlt.UpdateTime VolunteerUpdateTime
FROM Town
JOIN City C ON Town.CityId = C.Id AND IsCounty = 1
LEFT JOIN 
	(
		SELECT City,Town,COUNT(1) Count,MAX(ConfirmTime) ConfirmTime,MAX(UpdateTime)UpdateTime
		FROM [dbo].[Vehicle]
		GROUP BY City,Town) AS V ON C.City = V.City AND Town.Name = V.Town
LEFT JOIN
	(  
		SELECT  City,Town,COUNT(1) Count,MAX(ConfirmTime) ConfirmTime,MAX(UpdateDate)UpdateTime
		FROM Users
		GROUP BY City,Town) AS U ON C.City = U.City AND Town.Name = U.Town
LEFT JOIN
	(	SELECT  City,Town,SUM(Amount) Count,MAX(ConfirmTime) ConfirmTime,MAX(UpdateTime)UpdateTime
		FROM Disinfector
		GROUP BY City,Town
	) AS Dfector ON C.City = Dfector.City AND Town.Name = Dfector.Town
LEFT JOIN
	(
		SELECT p.City, p.Town,SUM(p.固體) SolidCount,SUM(ISNULL(p.液體, 0)+ISNULL(p.乳劑, 0)+ISNULL(p.油劑, 0)) LiquidCount,MAX(UpdateTime)UpdateTime,MAX(ConfirmTime) ConfirmTime
		FROM (
			SELECT  City,Town,DrugState,SUM(Amount) Amount,MAX(UpdateTime)UpdateTime,MAX(ConfirmTime)ConfirmTime
			FROM Disinfectant
			GROUP BY City, Town,DrugState
		) t 
		PIVOT (
			-- 設定彙總欄位及方式
			MAX(Amount) 
			-- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
			FOR DrugState IN ([固體],[液體],[乳劑],[油劑])
		) p
		GROUP BY p.City,p.Town
	) AS Dfectant ON C.City = Dfectant.City AND  Town.Name = Dfectant.Town
LEFT JOIN
	(
		SELECT City,Town, COUNT(1) Count,MAX(ConfirmTime) ConfirmTime,MAX(UpdateTime)UpdateTime
		FROM Pest
		GROUP BY City,Town
	) AS P ON P.City = C.City AND Town.Name = P.Town 
LEFT JOIN
	(
		SELECT City,Town, COUNT(1) Count,MAX(ConfirmTime) ConfirmTime,MAX(UpdateTime)UpdateTime
		FROM Dump
		GROUP BY City,Town
	) AS D ON C.City = D.City AND Town.Name = D.Town
LEFT JOIN
	(
		SELECT City,Town, COUNT(1) Count,MAX(ConfirmTime) ConfirmTime,MAX(UpdateTime)UpdateTime
		FROM Toilet
		GROUP BY City,Town
	) AS T ON C.City = T.City AND Town.Name = T.Town
LEFT JOIN
	(
		SELECT City,[Town], COUNT(1) Count,MAX(ConfirmTime) ConfirmTime,MAX(UpdateTime)UpdateTime
		FROM Volunteer
		GROUP BY City,[Town]
	) AS Vlt ON C.City = Vlt.City AND Town.Name = Vlt.[Town]
LEFT JOIN
	(
		SELECT 
			[Name]
			,[City]
			,[Town]
			,[MobilePhone]
		FROM [dbo].[Users]
		WHERE MainContacter = '是' AND DutyId = {DutyEnum.EPB.ToInteger()}
	) AS UMain ON C.City = UMain.City
	";
            return sql;
        }

        public List<ResourcesConfirmUpdateTimeDataModel> GetResourcesConfirmUpdateTimeDatas(ResourcesConfirmUpdateTimeDataFilterParameter filter)
        {
            string whereSql = GetWhereSQLByFilter(filter);
            string sql = $@"
SELECT
	C.Id CityId
	,C.City
    ,Town.Id TownId
	,Town.Name Town
	,VehicleUpdateTime
	,VehicleConfirmTime
	,UserUpdateTime
	,UserConfirmTime
	,DisinfectorUpdateTime
	,DisinfectorConfirmTime
	,DisinfectantUpdateTime
	,DisinfectantConfirmTime
	,PestUpdateTime
	,PestConfirmTime
	,DumpUpdateTime
	,DumpConfirmTime
	,ToiletUpdateTime
	,ToiletConfirmTime
	,VolunteerUpdateTime
	,VolunteerConfirmTime
	,LandfillUpdateTime
	,LandfillConfirmTime
	,IncineratorUpdateTime
	,IncineratorConfirmTime
FROM Town
JOIN City C ON Town.CityId = C.Id
LEFT JOIN 
	(
		SELECT City,Town,MAX(UpdateTime) VehicleUpdateTime,MAX(ConfirmTime) VehicleConfirmTime
		FROM [dbo].[Vehicle]
		GROUP BY City,Town) AS V ON C.City = V.City AND Town.Name = V.Town
LEFT JOIN
	(  
		SELECT  City,Town,MAX(UpdateDate) UserUpdateTime,MAX(ConfirmTime) UserConfirmTime
		FROM Users
		GROUP BY City,Town) AS U ON C.City = U.City AND Town.Name = U.Town
LEFT JOIN
	(	SELECT  City,Town,MAX(UpdateTime) DisinfectorUpdateTime,MAX(ConfirmTime) DisinfectorConfirmTime
		FROM Disinfector
		GROUP BY City,Town
	) AS Dfector ON C.City = Dfector.City AND Town.Name = Dfector.Town
LEFT JOIN
	(
		SELECT City, Town,MAX(UpdateTime) DisinfectantUpdateTime,MAX(ConfirmTime) DisinfectantConfirmTime
		FROM Disinfectant
		GROUP BY City, Town
	) AS Dfectant ON C.City = Dfectant.City AND  Town.Name = Dfectant.Town
LEFT JOIN
	(
		SELECT City,Town,MAX(UpdateTime) PestUpdateTime,MAX(ConfirmTime) PestConfirmTime
		FROM Pest
		GROUP BY City,Town
	) AS P ON P.City = C.City AND Town.Name = P.Town 
LEFT JOIN
	(
		SELECT City,Town,MAX(UpdateTime) DumpUpdateTime,MAX(ConfirmTime) DumpConfirmTime
		FROM Dump
		GROUP BY City,Town
	) AS D ON C.City = D.City AND Town.Name = D.Town
LEFT JOIN
	(
		SELECT City,Town,MAX(UpdateTime) ToiletUpdateTime,MAX(ConfirmTime) ToiletConfirmTime
		FROM Toilet
		GROUP BY City,Town
	) AS T ON C.City = T.City AND Town.Name = T.Town
LEFT JOIN
	(
		SELECT City,[Town],MAX(UpdateTime) VolunteerUpdateTime,MAX(ConfirmTime) VolunteerConfirmTime
		FROM Volunteer
		GROUP BY City,[Town]
	) AS Vlt ON C.City = Vlt.City AND Town.Name = Vlt.[Town]
LEFT JOIN
	(
		SELECT City,[Town],MAX(UpdateTime) LandfillUpdateTime,MAX(ConfirmTime) LandfillConfirmTime
		FROM Landfill LF
		GROUP BY City,[Town]
	) AS LF ON C.City = LF.City AND Town.Name = LF.[Town]
LEFT JOIN
	(
		SELECT City,[Town],MAX(UpdateTime) IncineratorUpdateTime,MAX(ConfirmTime) IncineratorConfirmTime
		FROM Incinerator Inc
		GROUP BY City,[Town]
	) AS Inc ON C.City = Inc.City AND Town.Name = Inc.[Town]
{whereSql}";
            return GetListBySQL<ResourcesConfirmUpdateTimeDataModel>(sql, filter);

        }
        private string GetWhereSQLByFilter(ResourcesConfirmUpdateTimeDataFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (filter.CityIds.IsNotEmpty())
            {
                whereSQL += " AND C.Id IN @CityIds";
            }

            if (filter.TownIds.IsNotEmpty())
            {
                whereSQL += " AND Town.Id IN @TownIds";
            }

            return whereSQL;
        }
    }
}