using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Models.QueryModel;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class InfectiousDiseaseRepository : BaseEMISRepository<InfectiousDiseaseModel>
    {
        public List<InfectiousDiseaseModel> GetByFilter(InfectiousDiseaseFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter);
        }
        public bool IsExistsByFilter(InfectiousDiseaseFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return IsExistsByWhereSQL(whereSQL, filter);
        }
        private string GetWhereSQLByFilter(InfectiousDiseaseFilterParameter filter)
        {
            string whereSQL = "Where 1=1";
            if(filter.CityIds.IsNotEmpty())
            {
                whereSQL += " AND CityId IN @CityIds";
            }
            if (filter.TownIds.IsNotEmpty())
            {
                whereSQL += " AND TownId IN @TownIds";
            }
            if (filter.Date.HasValue)
            {
                whereSQL += " AND Date = @Date";
            }
            if (filter.StartDate.HasValue)
            {
                whereSQL += " AND Date >= @StartDate";
            }
            if (filter.EndDate.HasValue)
            {
                whereSQL += " AND Date < @EndDate";
            }
            return whereSQL;
        }

        public List<InfectiousDiseaseStatisticsModel> GetStatistics(InfectiousDiseaseFilterParameter filter)
        {
            string whereSQL = "WHERE C.IsCounty = 1";
            string infectiousDiseaseWhereSQL = GetWhereSQLByFilter(filter);
            if (filter.CityIds.IsNotEmpty())
            {
                whereSQL += " AND C.Id IN @CityIds";
            }
            string sql = $@"
SELECT 
	T.Id TownId
	,T.Name TownName
	,C.Id CityId
	, C.City CityName
	, ISNULL(I.HomeQuarantineCount,0) HomeQuarantineCount
	, ISNULL(I.HomeQuarantineGarbageAmount,0) HomeQuarantineGarbageAmount
	, ISNULL(I.HomeInspectionCount,0) HomeInspectionCount
	, ISNULL(I.HomeInspectionGarbageAmount,0)HomeInspectionGarbageAmount
	, ISNULL(I.InspectionHotelCount,0)InspectionHotelCount
	, ISNULL(I.InspectionHotelGarbageAmount,0)InspectionHotelGarbageAmount
	, ISNULL(I.MaskCheckTimes,0)MaskCheckTimes
	, ISNULL(I.ReportTimes,0)ReportTimes
FROM Town T 
JOIN City C ON T.CityId = C.Id
LEFT JOIN 
(
	SELECT 
		[CityId]
		,[TownId]
		,SUM(HomeQuarantineCount)HomeQuarantineCount
		,SUM(HomeQuarantineGarbageAmount)HomeQuarantineGarbageAmount
		,SUM(HomeInspectionCount)HomeInspectionCount
		,SUM(HomeInspectionGarbageAmount)HomeInspectionGarbageAmount
		,SUM(InspectionHotelCount)InspectionHotelCount
		,SUM(InspectionHotelGarbageAmount)InspectionHotelGarbageAmount
		,SUM(MaskCheckTimes)MaskCheckTimes
		,SUM(ReportTimes)ReportTimes
	FROM InfectiousDisease
    {infectiousDiseaseWhereSQL}
	GROUP BY [CityId],[TownId]
) AS I ON I.TownId = T.Id
{whereSQL}
ORDER BY T.IsTown";
            return GetListBySQL<InfectiousDiseaseStatisticsModel>(sql, filter);
        }
    }
}