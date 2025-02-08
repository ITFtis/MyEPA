using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using MyEPA.Services;
using System;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class DisinfectantRepository : BaseEMISRepository<DisinfectantModel>
    {
        public List<DisinfectantModel> GetByFilter(DisinfectantFilterParameter filter)
        {
            string sql = GetSQLByFilter(filter);
            
            return GetListBySQL<DisinfectantModel>(sql, filter);
        }
        private static string GetSQLByFilter(DisinfectantFilterParameter filter)
        {
            string where = GetWhereSQLByFilter(filter);
            string sql = $@"
                            SELECT P.*
                            FROM [dbo].[Disinfectant] P
                            JOIN City C ON P.City = C.City
                            JOIN Town T ON P.Town = T.Name AND T.CityId = C.Id
                            {where}";
            return sql;
        }

        public List<DisinfectantTownStatisticsModel> GetStatistics(DisinfectantStatisticsFilterParameter filter)
        {
            string where = GetWhereSQLByFilter(filter);
            string sql = $@"
                            SELECT D.*,c.Id CityId, t.Id TownId
                            FROM
                            (
	                            SELECT  City,Town,DrugState,SUM(Amount) Amount,COUNT(*) COUNT,MAX(UpdateTime)UpdateTime,MIN(ServiceLife)ServiceLife
	                            FROM Disinfectant
	                            GROUP BY City, Town,DrugState
                            ) AS D
                            JOIN City c ON D.City = c.City
                            JOIN Town t ON t.Name = D.Town AND t.CityId = c.Id
                            {where}";
            return GetListBySQL<DisinfectantTownStatisticsModel>(sql, filter);
        }

        public List<DisinfectantSummaryTownReportModel> GetSummaryTownReport()
        {
            string sql = GetSummaryTownReportSQL();

            return GetListBySQL<DisinfectantSummaryTownReportModel>(sql);
        }

        private string GetSummaryTownReportSQL()
        {
            string sql = @"
SELECT 
		p.City
		, p.Town
		,ISNULL(SUM(p.[1]),0) EnvironmentCount
		,ISNULL(SUM(p.[2]),0) DengueCount
		,ISNULL(SUM(p.[3]),0) RIFACount
		,ISNULL(SUM(p.[4]),0) TessaratomaPapillosaDruryCount
		,ISNULL(SUM(p.[5]),0) OtherCount
		,MAX(UpdateTime)UpdateTime
		,MAX(ConfirmTime) ConfirmTime
	FROM (
		SELECT City,Town,UseType,SUM(Amount)Amount,MAX(UpdateTime)UpdateTime,MAX(ConfirmTime)ConfirmTime
		FROM Disinfectant
		WHERE UseType IS NOT NULL
		GROUP BY City,Town, UseType
	) t 
	PIVOT (
		-- 設定彙總欄位及方式
		MAX(Amount) 
		-- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
		FOR UseType IN ([1],[2],[3],[4],[5])
	) p
	GROUP BY p.City,p.Town
";
            return sql;
        }

        public List<DisinfectantSummaryCityReportModel> GetSummaryCityReport()
        {
            string townSql = GetSummaryTownReportSQL();
            string sql = $@"
SELECT
	T.City
	,ISNULL(SUM(T.EnvironmentCount),0) EnvironmentCount
	,ISNULL(SUM(T.DengueCount),0) DengueCount
	,ISNULL(SUM(T.RIFACount),0) RIFACount
	,ISNULL(SUM(T.TessaratomaPapillosaDruryCount),0) TessaratomaPapillosaDruryCount
	,ISNULL(SUM(T.OtherCount),0) OtherCount
	,MAX(UpdateTime)UpdateTime
	,MAX(ConfirmTime) ConfirmTime
FROM
(
	{townSql}
) T
GROUP BY T.City";
            return GetListBySQL<DisinfectantSummaryCityReportModel>(sql);
        }

        //跨縣市調度
        private string GetSupportCityTownReportSQL()
        {
            string sql = @"
SELECT 
		p.City
		, p.Town
		,ISNULL(SUM(p.[1]),0) EnvironmentCount
		,ISNULL(SUM(p.[2]),0) DengueCount
		,ISNULL(SUM(p.[3]),0) RIFACount
		,ISNULL(SUM(p.[4]),0) TessaratomaPapillosaDruryCount
		,ISNULL(SUM(p.[5]),0) OtherCount
		,MAX(UpdateTime)UpdateTime
		,MAX(ConfirmTime) ConfirmTime
	FROM (
		SELECT City,Town,UseType,MAX(UpdateTime)UpdateTime,MAX(ConfirmTime)ConfirmTime,
               SUM(Case When ISNULL(IsSupportCity, '') = 1 Then ISNULL(SupportCityNum, 0) Else 0 End) AS SupportCityNum
		FROM Disinfectant
		WHERE UseType IS NOT NULL
		GROUP BY City,Town, UseType
	) t 
	PIVOT (
		-- 設定彙總欄位及方式
		MAX(SupportCityNum) 
		-- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
		FOR UseType IN ([1],[2],[3],[4],[5])
	) p
	GROUP BY p.City,p.Town
";
            return sql;
        }

        //跨縣市調度
        public List<DisinfectantSummaryCityReportModel> GetSupportCityReport()
        {
            string townSql = GetSupportCityTownReportSQL();
            string sql = $@"
SELECT
	T.City
	,ISNULL(SUM(T.EnvironmentCount),0) EnvironmentCount
	,ISNULL(SUM(T.DengueCount),0) DengueCount
	,ISNULL(SUM(T.RIFACount),0) RIFACount
	,ISNULL(SUM(T.TessaratomaPapillosaDruryCount),0) TessaratomaPapillosaDruryCount
	,ISNULL(SUM(T.OtherCount),0) OtherCount
	,MAX(UpdateTime)UpdateTime
	,MAX(ConfirmTime) ConfirmTime
FROM
(
	{townSql}
) T
GROUP BY T.City";
            return GetListBySQL<DisinfectantSummaryCityReportModel>(sql);
        }

        public List<DisinfectantCityReportModel> GetCityReport(DisinfectantReportFilterParameter filter)
        {
            string where = GetWhereSQLByFilter(filter);
            string sql = $@"
SELECT c.Id CityId,D.*
FROM
(
	SELECT  City,UseType,DrugName,DrugType,DrugState,SUM(Amount) Amount,MAX(UpdateTime) UpdateTime
	FROM Disinfectant
	{where}
	GROUP BY City,DrugState,DrugName,UseType,DrugType
) AS D
JOIN City c ON D.City = c.City

";
            return GetListBySQL<DisinfectantCityReportModel>(sql, filter);
        }
        public List<DisinfectantCityReportModel> GetTownReport(DisinfectantReportFilterParameter filter)
        {
            string where = GetWhereSQLByFilter(filter);
            string sql = $@"
SELECT T.Id TownId,c.Id CityId,D.*
FROM
(
	SELECT ISNULL(ActiveIngredients1, '無') ActiveIngredients1,ISNULL(ActiveIngredients2, '無') ActiveIngredients2,Town,City,UseType,DrugName,DrugType,DrugState,SUM(Amount) Amount,MAX(UpdateTime) UpdateTime
	FROM Disinfectant
	{where}
	GROUP BY Town,City,DrugState,DrugName,UseType,DrugType,ActiveIngredients1,ActiveIngredients2
) AS D
JOIN City c ON D.City = c.City
Left JOIN Town t ON D.Town = T.Name AND C.Id = T.CityId
";
            return GetListBySQL<DisinfectantCityReportModel>(sql, filter);
        }
        private static string GetWhereSQLByFilter(DisinfectantReportFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (string.IsNullOrWhiteSpace(filter.City) == false)
            {
                whereSQL += " AND City = @City";
            }
            if (string.IsNullOrWhiteSpace(filter.Town) == false)
            {
                whereSQL += " AND Town = @Town";
            }
            if (filter.UseType.HasValue)
            {
                whereSQL += " AND UseType = @UseType";
            }
            if (string.IsNullOrWhiteSpace(filter.DrugName) == false)
            {
                whereSQL += " AND DrugName = @DrugName";
            }
            if (string.IsNullOrWhiteSpace(filter.DrugType) == false)
            {
                whereSQL += " AND DrugType = @DrugType";
            }
            if (filter.ServiceLifeStartTime.HasValue)
            {
                whereSQL += " AND ServiceLife >= @ServiceLifeStartTime";
            }
            if (filter.ServiceLifeEndTime.HasValue)
            {
                whereSQL += " AND ServiceLife < @ServiceLifeEndTime";
            }
            if (!filter.ActiveIngredients1.IsEmptyOrNull())
            {
                whereSQL += " AND ActiveIngredients1 = @ActiveIngredients1";
            }
            if (!filter.ActiveIngredients2.IsEmptyOrNull())
            {
                whereSQL += " AND ActiveIngredients2 = @ActiveIngredients2";
            }

            if (!filter.DrugState.IsEmptyOrNull())
            {
                whereSQL += " AND DrugState = @DrugState";
            }
            return whereSQL;
        }
        private static string GetWhereSQLByFilter(DisinfectantStatisticsFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (filter.CityIds.IsNotEmpty())
            {
                whereSQL += " AND CityId = @CityIds";
            }
            if (filter.ServiceLifeStartTime.HasValue)
            {
                whereSQL += " AND ServiceLife >= @ServiceLifeStartTime";
            }
            if (filter.ServiceLifeEndTime.HasValue)
            {
                whereSQL += " AND ServiceLife < @ServiceLifeEndTime";
            }
            return whereSQL;
        }
        private static string GetWhereSQLByFilter(DisinfectantFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (filter.CityIds.IsNotEmpty())
            {
                whereSQL += "  AND C.Id IN @CityIds";
            }
            if (filter.TownIds.IsNotEmpty())
            {
                whereSQL += " AND T.Id IN @TownIds";
            }
            if (filter.DrugState.HasValue)
            {
               
                if(filter.DrugState== MyEPA.Enums.DrugStateEnum.Liquid)
                {
                    whereSQL += $" AND (P.DrugState = N'{filter.DrugState.Value.GetDescription()}' or P.DrugState = N'乳劑' or P.DrugState = N'油劑')";
                }
                else
                {
                    whereSQL += $" AND P.DrugState = N'{filter.DrugState.Value.GetDescription()}'";
                }
            }

            return whereSQL;
        }
    }
}