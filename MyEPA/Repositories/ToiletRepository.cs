using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using MyEPA.Services;
using System;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class ToiletRepository : BaseEMISRepository<ToiletModel>
    {
        public List<ToiletModel> GetByFilter(ToiletFilterParameter filter)
        {
            string sql = GetSQLByFilter(filter);

            return GetListBySQL<ToiletModel>(sql, filter);
        }
        public List<ToiletReportModel> GetReportByFilter(ToiletFilterParameter filter)
        {
            string querySql = GetSQLByFilter(filter);

            string sql = $@"
SELECT ContactUnit,City,Town,ROCyear,COUNT(1) COUNT, SUM(CAST(Amount AS decimal(18, 2)))Amount,MAX(UpdateTime) UpdateTime
FROM
(
	{querySql}
)AS T
GROUP BY ContactUnit,City,Town,ROCyear";

            return GetListBySQL<ToiletReportModel>(sql, filter);
        }
        private static string GetSQLByFilter(ToiletFilterParameter filter)
        {
            string where = GetWhereSQLByFilter(filter);
            string sql = $@"
SELECT D.*
FROM [dbo].[Toilet] D
JOIN City C ON D.City = C.City
JOIN Town T ON D.Town = T.Name AND T.CityId = C.Id
{where}";
            return sql;
        }
        private static string GetWhereSQLByFilter(ToiletFilterParameter filter)
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

            return whereSQL;
        }
    }
}