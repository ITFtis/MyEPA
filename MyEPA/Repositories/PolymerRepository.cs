using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class PolymerRepository : BaseEMISRepository<PolymerModel>
    {
        public bool IsExistsByFilter(PolymerFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return IsExistsByWhereSQL(whereSQL, filter);
        }
        public List<PolymerModel> GetByFilter(PolymerFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter);
        }

        private static string GetWhereSQLByFilter(PolymerFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (filter.DiasterIds.IsNotEmpty())
            {
                whereSQL += " And DiasterId IN @DiasterIds";
            }
            if (filter.PolymerIds.IsNotEmpty())
            {
                whereSQL += " And Id IN @PolymerIds";
            }

            return whereSQL;
        }
    }
}