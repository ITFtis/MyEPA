using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class LandfillRepository : BaseEMISRepository<LandfillModel>
    {
        public List<LandfillModel> GetByFilter(LandfillFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter);
        }
        private static string GetWhereSQLByFilter(LandfillFilterParameter filter)
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