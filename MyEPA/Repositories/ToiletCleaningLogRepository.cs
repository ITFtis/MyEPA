using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class ToiletCleaningLogRepository : BaseEMISRepository<ToiletCleaningLogModel>
    {
        public List<ToiletCleaningLogModel> GetByFilter(ToiletCleaningLogFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter);
        }
        private static string GetWhereSQLByFilter(ToiletCleaningLogFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (filter.ToiletLocationIds.IsNotEmpty())
            {
                whereSQL += " And ToiletLocationId IN @ToiletLocationIds";
            }

            return whereSQL;
        }
    }
}