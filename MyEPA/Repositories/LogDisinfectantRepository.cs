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

        /// <summary>
        /// (閥值)消毒藥品，當下數量比較
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<LogDisinfectantViewModel> GetLogDisinfectantCurrentByFilter(LogDisinfectantFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);

            string sql = $@"

Select a.DiasterId, 
       a.City, a.Town, a.ContactUnit, a.DrugName, 
       a.Amount, a.CtPoint, a.LogBDate, a.LogBUser,
	   b.CurAmount,
	   c.Sort
From
(
	Select DiasterId, City, Town, ContactUnit, DrugName, Sum(Amount) AS Amount, Sum(CtPoint) AS CtPoint,
			MAX(LogBDate) AS LogBDate, MAX(LogBUser) AS LogBUser
	From LogDisinfectant
    {whereSQL}
	Group By DiasterId, City, Town, ContactUnit, DrugName
)a
Left Join
(
	Select City, Town, ContactUnit, DrugName, Sum(Amount) AS CurAmount
	From Disinfectant
	Group By City, Town, ContactUnit, DrugName
)b On a.City = b.City And a.Town = b.Town
And ISNULL(a.ContactUnit, '') = ISNULL(b.ContactUnit, '')
And a.DrugName = b.DrugName 
Left Join City c On a.City = c.City
Order By c.Sort

";


            return GetListBySQL<LogDisinfectantViewModel>(sql, filter);
        }

        public bool Delete(int DiasterId)
        {
            string whereSql = "WHERE DiasterId = @DiasterId";
            return DeleteByWhereSQL(whereSql, new { DiasterId });
        }
    }
}