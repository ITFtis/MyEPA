using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class IncineratorRepository : BaseEMISRepository<IncineratorModel>
    {
        public List<IncineratorModel> GetByFilter(IncineratorFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter);
        }
        private static string GetWhereSQLByFilter(IncineratorFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (string.IsNullOrWhiteSpace(filter.City) == false)
            {
                whereSQL += " And City = @City";
            }
            if (string.IsNullOrWhiteSpace(filter.Town) == false)
            {
                whereSQL += " And Town = @Town";
            }
            return whereSQL;
        }
    }
}