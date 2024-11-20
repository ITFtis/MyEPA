using System;
using System.Collections.Generic;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Models.QueryModel;
using MyEPA.Repositories.BaseRepositories;

namespace MyEPA.Repositories
{
    public class WaterCheckRepository : BaseEMISRepository<WaterCheckModel>
    {
        public List<WaterCheckModel> GetByFilter(WaterCheckFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter);
        }
        public int GetCountByFilter(WaterCheckFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetCountByWhereSQL(whereSQL, filter);
        }
        private string GetWhereSQLByFilter(WaterCheckFilterParameter filter)
        {
            string wherwSQL = "Where 1=1";

            if(filter.CityIds.IsNotEmpty())
            {
                wherwSQL += " AND CityId IN @CityIds";
            }

            if(filter.DiasterIds.IsNotEmpty())
            {
                wherwSQL += " AND DiasterId IN @DiasterIds";
            }

            if (filter.Types.IsNotEmpty())
            {
                wherwSQL += " AND Type IN @Types";
            }

            if (filter.CheckDate.HasValue)
            {
                wherwSQL += " AND CheckDate = @CheckDate";
            }

            return wherwSQL;
        }

        public List<WaterCheckStatisticsQueryModel> GetWaterCheckStatistics(int diasterId)
        {
            string sql = @"
SELECT WCD.CityId,WC.Type,WCD.Status ,COUNT(WCD.ID) COUNT
FROM WaterCheck  WC
JOIN WaterCheckDetail WCD ON WC.Id = WCD.WaterCheckId
WHERE WC.DiasterId = @DiasterId
GROUP BY WCD.CityId,WC.Type,WCD.Status";
            return GetListBySQL<WaterCheckStatisticsQueryModel>(sql, new { diasterId });
        }

        public List<WaterCheckStatisticsQueryModel> GetWaterCheckStatisticsEasy(int diasterId)
        {
            string sql = @"
SELECT WCD.CityId,WC.Type,WCD.Status,
       WCD.WaterCheckId, WCD.[Address]
FROM WaterCheck  WC
JOIN WaterCheckDetail WCD ON WC.Id = WCD.WaterCheckId
WHERE WC.DiasterId = @DiasterId";
            return GetListBySQL<WaterCheckStatisticsQueryModel>(sql, new { diasterId });
        }
    }
}