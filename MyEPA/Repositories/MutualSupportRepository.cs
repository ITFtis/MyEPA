using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class MutualSupportRepository : BaseEMISRepository<MutualSupportModel>
    {
        public List<MutualSupportModel> GetByFilter(MutualSupportFilterParameter filter)
        {
            string whereSql = GetWhereSQLByFilter(filter);
            string querySQL = $@"SELECT * 
                                 FROM [MutualSupport] ms WITH(NOLOCK)
                                 {whereSql}";

            return GetListBySQL<MutualSupportModel>(querySQL, filter);
        }
        private string GetWhereSQLByFilter(MutualSupportFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (filter.ResourceTypeIds.IsNotEmpty())
            {
                whereSQL += " AND ms.ResourceTypeId IN @ResourceTypeIds";
            }
            if (filter.Year.HasValue)
            {
                whereSQL += " AND (YEAR(ms.StartDate) = @Year OR YEAR(ms.EndDate) = @Year)";
            }
            if(string.IsNullOrWhiteSpace(filter.AcceptSection) == false)
            {
                filter.AcceptSection = $"%{filter.AcceptSection}%";
                whereSQL += " AND ms.AcceptSection LIKE @AcceptSection";
            }
            if (string.IsNullOrWhiteSpace(filter.Section) == false)
            {
                filter.Section = $"%{filter.Section}%";
                whereSQL += " AND ms.Section LIKE @Section";
            }
            if (filter.SupportType.IsNotEmpty())
            {
                whereSQL += " AND ms.SupportType IN @SupportType";
            }
            return whereSQL;
        }
    }
}