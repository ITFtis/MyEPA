using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using MyEPA.Services;
using System;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class PestRepository : BaseEMISRepository<PestModel>
    {
        public List<PestModel> GetByFilter(PestFilterParameter filter)
        {
            string sql = GetSQLByFilter(filter);
            
            return GetListBySQL<PestModel>(sql, filter);
        }
        private static string GetSQLByFilter(PestFilterParameter filter)
        {
            string where = GetWhereSQLByFilter(filter);
            string sql = $@"
                            SELECT P.*
                            FROM [dbo].[Pest] P
                            JOIN City C ON P.City = C.City
                            JOIN Town T ON P.Town = T.Name AND T.CityId = C.Id
                            {where}";
            return sql;
        }
        private static string GetWhereSQLByFilter(PestFilterParameter filter)
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