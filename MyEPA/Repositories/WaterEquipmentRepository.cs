using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class WaterEquipmentRepository : BaseEMISRepository<WaterEquipmentModel>
    {
        public List<WaterEquipmentViewModel> GetByFilter(WaterEquipmentFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);

            string sql = $@"
SELECT*
FROM
(
	SELECT WE.*,T.Name TownName,C.City CityName
	FROM [dbo].[WaterEquipment] WE
	JOIN City C ON WE.CityId = C.Id
	JOIN Town T ON WE.TownId = T.Id
) AS T
{whereSQL}";


            return GetListBySQL<WaterEquipmentViewModel>(sql, filter);
        }

        private static string GetWhereSQLByFilter(WaterEquipmentFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (filter.DiasterIds.IsNotEmpty())
            {
                whereSQL += " And DiasterId IN @DiasterIds";
            }
            if (filter.Ids.IsNotEmpty())
            {
                whereSQL += " And Id IN @Ids";
            }

            return whereSQL;
        }
    }
}