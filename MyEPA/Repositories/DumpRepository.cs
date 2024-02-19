using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using MyEPA.Services;
using System;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class DumpRepository : BaseEMISRepository<DumpModel>
    {
        public List<DumpModel> GetByFilter(DumpFilterParameter filter)
        {
            string sql = GetSQLByFilter(filter);
            
            return GetListBySQL<DumpModel>(sql, filter);
        }
        private static string GetSQLByFilter(DumpFilterParameter filter)
        {
            string where = GetWhereSQLByFilter(filter);
            string sql = $@"
                            SELECT V.*
                            FROM [dbo].[Dump] V
                            JOIN City C ON V.City = C.City
                            JOIN Town T ON V.Town = T.Name AND T.CityId = C.Id
                            {where}";
            return sql;
        }
        private static string GetWhereSQLByFilter(DumpFilterParameter filter)
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