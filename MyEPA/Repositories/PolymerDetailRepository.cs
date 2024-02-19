using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class PolymerDetailRepository : BaseEMISRepository<PolymerDetailModel>
    {
        public List<PolymerDetailModel> GetByFilter(PolymerDetailFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter);
        }

        private static string GetWhereSQLByFilter(PolymerDetailFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (filter.PolymerIds.IsNotEmpty())
            {
                whereSQL += " And PolymerId IN @PolymerIds";
            }

            return whereSQL;
        }

        public void DeleteByFilter(PolymerDetailFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            DeleteByWhereSQL(whereSQL, filter);
        }
    }
}