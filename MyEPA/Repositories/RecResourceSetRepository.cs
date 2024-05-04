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
    public class RecResourceSetRepository : BaseEMISRepository<RecResourceSetModel>
    {
        public List<RecResourceSetModel> GetByFilter(RecResourceSetFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter);
        }
        ////public int GetCountByFilter(RecResourceFilterParameter filter)
        ////{
        ////    string whereSQL = GetWhereSQLByFilter(filter);
        ////    return GetCountByWhereSQL(whereSQL, filter);
        ////}
        private string GetWhereSQLByFilter(RecResourceSetFilterParameter filter)
        {
            string wherwSQL = "Where 1=1";

            if (filter.CityIds.IsNotEmpty())
            {
                wherwSQL += " AND CityId IN @CityIds";
            }

            if (filter.RecResourceIdNeeds.IsNotEmpty())
            {
                wherwSQL += " AND RecResourceIdNeed IN @RecResourceIdNeeds";
            }

            if (filter.RecResourceIdHelps.IsNotEmpty())
            {
                wherwSQL += " AND RecResourceIdHelp IN @RecResourceIdHelps";
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
    }
}