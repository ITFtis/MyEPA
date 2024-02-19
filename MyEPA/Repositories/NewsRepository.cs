using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class NewsRepository : BaseEMISBaseModelRepository<NewsModel>
    {
        
        public List<NewsModel> GetByFilter(NewsFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter);
        }

        private static string GetWhereSQLByFilter(NewsFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (filter.Ids.IsNotEmpty())
            {
                whereSQL += " And Id IN @Ids";
            }
            if (filter.DiasterIds.IsNotEmpty())
            {
                whereSQL += " And DiasterId IN @DiasterIds";
            }
            if (filter.StartDate.HasValue)
            {
                whereSQL += " And UpdateDate >= @StartDate";
            }
            if (filter.EndDate.HasValue)
            {
                whereSQL += " And UpdateDate <= @EndDate";
            }
            if (string.IsNullOrWhiteSpace(filter.Keyword) == false)
            {
                filter.Keyword = $"%{filter.Keyword}%";
                whereSQL += " And Title LIKE @Keyword";
            }
            return whereSQL;
        }
    }
}