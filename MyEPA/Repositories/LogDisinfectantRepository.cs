using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Repositories
{
    public class LogDisinfectantRepository : BaseEMISRepository<LogDisinfectantModel>
    {
        public List<LogDisinfectantModel> GetByFilter(LogDisinfectantFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter);
        }

        private string GetWhereSQLByFilter(LogDisinfectantFilterParameter filter)
        {
            string wherwSQL = "Where 1=1";

            if (filter.CityIds.IsNotEmpty())
            {
                wherwSQL += " AND CityId IN @CityIds";
            }

            if (filter.DiasterIds.IsNotEmpty())
            {
                wherwSQL += " AND DiasterId IN @DiasterIds";
            }

            ////if (filter.Types.IsNotEmpty())
            ////{
            ////    wherwSQL += " AND Type IN @Types";
            ////}

            ////if (filter.CheckDate.HasValue)
            ////{
            ////    wherwSQL += " AND CheckDate = @CheckDate";
            ////}

            return wherwSQL;
        }

        public bool Delete(int DiasterId)
        {
            string whereSql = "WHERE DiasterId = @DiasterId";
            return DeleteByWhereSQL(whereSql, new { DiasterId });
        }
    }
}