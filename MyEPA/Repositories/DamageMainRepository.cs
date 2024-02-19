using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Repositories
{
    public class DamageMainRepository : BaseEMISRepository<DamageMainModel>
    {
        public List<DamageMainModel> GetListByFilter(DamageMainFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter);
        }

        public DamageMainModel GetByFilter(DamageMainFilterParameter filter)
        {
            return GetListByFilter(filter).FirstOrDefault();
        }

        private static string GetWhereSQLByFilter(DamageMainFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (filter.CityIds.IsNotEmpty())
            {
                whereSQL += " And CityId IN @CityIds";
            }
            if (filter.DiasterIds.IsNotEmpty())
            {
                whereSQL += " And DiasterId IN @DiasterIds";
            }

            return whereSQL;
        }
    }
}