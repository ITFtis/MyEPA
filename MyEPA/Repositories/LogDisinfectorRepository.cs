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
    public class LogDisinfectorRepository : BaseEMISRepository<LogDisinfectorModel>
    {
        public List<LogDisinfectorModel> GetByFilter(LogDisinfectorFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter);
        }

        private string GetWhereSQLByFilter(LogDisinfectorFilterParameter filter)
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
        /// (閾值)消毒設備，當下數量比較
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<LogDisinfectorViewModel> GetLogDisinfectorCurrentByFilter(LogDisinfectorFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            string where2 = "";

            string sql = $@"

Select a.DiasterId, 
       a.City, a.Town, a.ContactUnit, a.DisinfectInstrument, 
       a.Amount, a.CtPoint, a.LogBDate, a.LogBUser,
	   b.CurAmount,
	   c.Sort
From
(
	Select DiasterId, City, Town, ContactUnit, DisinfectInstrument, Sum(Amount) AS Amount, Sum(CtPoint) AS CtPoint,
	       MAX(LogBDate) AS LogBDate, MAX(LogBUser) AS LogBUser
	From LogDisinfector
    {whereSQL}
	Group By DiasterId, City, Town, ContactUnit, DisinfectInstrument
)a
Left Join
(
	Select City, Town, ContactUnit, DisinfectInstrument, Sum(Amount) AS CurAmount
	From Disinfector
	Group By City, Town, ContactUnit, DisinfectInstrument
)b On a.City = b.City And a.Town = b.Town
And a.ContactUnit = b.ContactUnit
And a.DisinfectInstrument = b.DisinfectInstrument 
Left Join City c On a.City = c.City
Where 1=1
";

            if (filter.Ct.HasValue)
            {
                if (filter.Ct == 1)
                {
                    //低於閾值
                    sql += " AND (a.CtPoint > b.CurAmount Or a.CtPoint Is Null Or b.CurAmount Is Null)";
                }
                else if (filter.Ct == 2)
                {
                    //高於閾值(正常)
                    sql += " AND a.CtPoint <= b.CurAmount";
                }                
            }

            sql += @"
                        Order By c.Sort
                ";

            return GetListBySQL<LogDisinfectorViewModel>(sql, filter);
        }

        public bool Delete(int DiasterId)
        {
            string whereSql = "WHERE DiasterId = @DiasterId";
            return DeleteByWhereSQL(whereSql, new { DiasterId });
        }
    }
}