using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System.Collections.Generic;

namespace MyEPA.Repositories
{

    public class WaterCheckDetailRepository : BaseEMISRepository<WaterCheckDetailModel>
    {
        
        public List<WaterCheckDetailModel> GetListByWaterCheckId(int waterCheckId)
        {
            return GetByFilter(new WaterCheckDetailFilterParameter 
            {
                WaterCheckIds = waterCheckId.ToListCollection()
            });
        }
        public int GetCountByFilter(WaterCheckDetailFilterParameter filter)
        {
            string whereSql = GetWhereSQLByFilter(filter);

            return GetCountByWhereSQL(whereSql, filter);
        }
        public List<WaterCheckDetailModel> GetByFilter(WaterCheckDetailFilterParameter filter)
        {
            string whereSql = GetWhereSQLByFilter(filter);

            return GetListByWhereSQL(whereSql, filter);
        }
        private static string GetWhereSQLByFilter(WaterCheckDetailFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (filter.WaterCheckIds.IsNotEmpty())
            {
                whereSQL += " AND WaterCheckId IN @WaterCheckIds";
            }
            if (filter.CityIds.IsNotEmpty())
            {
                whereSQL += " AND CityId IN @CityIds";
            }
            if (filter.Rechecks.IsNotEmpty())
            {
                whereSQL += " AND Recheck IN @Rechecks";
            }
            if (filter.Status.IsNotEmpty())
            {
                whereSQL += " AND Status IN @Status";
            }
            if (filter.UpdateUsers.IsNotEmpty())
            {
                whereSQL += " AND UpdateUser IN @UpdateUsers";
            }
            return whereSQL;
        }
        public List<WaterCheckReportModel> GetReport(int diasterId, WaterCheckTypeEnum type)
        {
            string sql = $@"
SELECT P2.*,U.Name,U.OfficePhone
FROM
(
	SELECT 
		wcd.CityId
		,wcd.CityName
		,Count(case when (wcd.status=1) then 1 else null end) as  FailureCount
		,Count(case when (wcd.status=2) then 1 else null end) as  NothingHappenedCount
		,Count(case when (wcd.status=3) then 1 else null end) as  CannotCount
		,Count(case when (wcd.status=4) then 1 else null end) as  SuccessCount
		,Count(case when (wcd.status=5) then 1 else null end) as  TestingCount
		,Count(1) as  Count
		,MAX(UpdateTime) UpdateTime
	FROM WaterCheckDetail wcd
	JOIN WaterCheck wc ON wcd.WaterCheckId = wc.Id
	WHERE wc.DiasterId = @DiasterId AND wc.Type = @Type AND WCD.STATUS IN (1,2,3,4,5)
	GROUP BY wcd.CityId,wcd.CityName
) P2
LEFT JOIN Users U ON P2.CityId = U.CityId AND U.MainContacter = '是' AND U.DutyId = {DutyEnum.EPB.ToInteger()}
";
            return GetListBySQL<WaterCheckReportModel>(sql, new { DiasterId = diasterId, Type = type.ToInteger() });
        }

        public List<WaterCheckYearReportModel> GetYearReport(WaterCheckTypeEnum type,int year)
        {
            string sql = $@"
SELECT 
	wc.DiasterId
	,Count(case when (wcd.status=1) then 1 else null end) as  FailureCount
	,Count(case when (wcd.status=2) then 1 else null end) as  NothingHappenedCount
	,Count(case when (wcd.status=3) then 1 else null end) as  CannotCount
	,Count(case when (wcd.status=4) then 1 else null end) as  SuccessCount
	,Count(case when (wcd.status=5) then 1 else null end) as  TestingCount
	,Count(1) as  Count
FROM WaterCheckDetail wcd
JOIN WaterCheck wc ON wcd.WaterCheckId = wc.Id
WHERE wc.Type = @Type AND WCD.STATUS IN (1,2,3,4,5)
AND Datepart(year,wcd.CheckTime) = @year
GROUP BY wc.DiasterId
";
            return GetListBySQL<WaterCheckYearReportModel>(sql, new { Type = type.ToInteger() , year });
        }
    }
}