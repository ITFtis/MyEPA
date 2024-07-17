using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Repositories
{
    public class VehicleRepository : BaseEMISRepository<VehicleModel>
    {
        public void UpdateConfirmTimeByFilter(VehicleFilterParameter filter)
        {
            string where = GetWhereSQLByFilter(filter);

            string sql = $@"
                Update [dbo].[Vehicle]
                SET ConfirmTime = '{DateTimeHelper.GetCurrentTime().ToString("yyyy/MM/dd HH:mm:ss.fff")}'
                From [dbo].[Vehicle] V
                JOIN City C ON V.City = C.City
                Left JOIN Town T ON V.Town = T.Name AND T.CityId = C.Id
                {where}
            ";
            ExecuteSQL(sql, filter);
        }
        public List<VehicleModel> GetByFilter(VehicleFilterParameter filter)
        {
            string sql = GetSQLByFilter(filter);
            
            return GetListBySQL<VehicleModel>(sql, filter);
        }

        public List<VehicleReportModel> GetReportByFilter(VehicleFilterParameter filter)
        {
            string querySql = GetSQLByFilter(filter);

            string sql = $@"
Select aaa.Type, aaa.Name AS VehicleType, IsNull(VehicleQuantity, 0) AS VehicleQuantity
From VehicleType aaa
Left Join
(
    SELECT VTType, COUNT(1) VehicleQuantity
    FROM
    (
	    {querySql}
    ) AS VReport
    GROUP BY VTType
)bbb On aaa.Type = bbb.VTType
Order By aaa.Type
";

            return GetListBySQL<VehicleReportModel>(sql, filter);
        }

        public List<VehicleCountModel> GetCarsCountByCity()
        {
            string sql = @"SELECT City CityName, VT.Name VehicleName,COUNT(1)COUNT
FROM Vehicle V
JOIN  VehicleType VT ON ISNULL(v.VehicleType, 'nullvalue') = VT.Type

GROUP BY VT.Name,City
";
            return GetListBySQL<VehicleCountModel>(sql,new {});
        }

        private static string GetSQLByFilter(VehicleFilterParameter filter)
        {
            string where = GetWhereSQLByFilter(filter);
            string sql = $@"
                            SELECT V.*,VT.Name VTName,VT.Type AS VTType
                            FROM [dbo].[Vehicle] V
                            JOIN City C ON V.City = C.City
                            Left JOIN Town T ON V.Town = T.Name AND T.CityId = C.Id
                            JOIN VehicleType VT ON ISNULL(v.VehicleType, 'nullvalue') = VT.Type
                            {where}";
            return sql;
        }
        private static string GetWhereSQLByFilter(VehicleFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (filter.CityIds.IsNotEmpty())
            {
                whereSQL += "  AND C.Id IN @CityIds";
            }
            if (filter.TownIds.IsNotEmpty())
            {
                whereSQL += " AND T.Id IN @TownIds";
            }

            return whereSQL;
        }
    }
}