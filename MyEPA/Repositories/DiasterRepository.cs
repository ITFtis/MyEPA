using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class DiasterRepository : BaseEMISRepository<DiasterModel>
    {
        public List<int> GetDiasterYears()
        {
            string sql = @"
SELECT year
FROM
(
	SELECT Datepart(year,[StartTime])year
	FROM [dbo].[Diaster]
) AS T
GROUP BY year
ORDER BY year DESC";

            return GetListBySQL<int>(sql);
        }
        public bool IsExistsRuning()
        {
            string whereSQL = GetWhereRuningSQL();
            string sql = $@"
                            SELECT TOP 1 Id
                            FROM Diaster
                            {whereSQL}";

            return IsExistsBySQL(sql);
        }

        public List<DiasterModel> GetByFilter(DiasterFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter);
        }
        private static string GetWhereSQLByFilter(DiasterFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";
            if (filter.StartYears.IsNotEmpty())
            {
                whereSQL += " And Datepart(year,[StartTime]) IN @StartYears";
            }
            if (filter.Ids.IsNotEmpty())
            {
                whereSQL += " And Id IN @Ids";
            }
            if (filter.IsRunning == true)
            {
                whereSQL += $" And Status = {NormalActiveStatusEnum.Active.ToInteger()}";
            }
            return whereSQL;
        }

        public DiasterModel GetByRuning()
        {
            string whereSQL = GetWhereRuningSQL();
            return GetByWhereSQL(whereSQL);
        }

        private string GetWhereRuningSQL()
        {
            return $"WHERE Status = {NormalActiveStatusEnum.Active.ToInteger()}";
        }
    }
}