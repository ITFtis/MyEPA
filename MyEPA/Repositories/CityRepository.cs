using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Repositories
{
    public class CityRepository : BaseEMISRepository<CityModel>
    {
        public CityModel GetByCityName(string city)
        {
            string whereSQL = "Where City=@City";
            return GetByWhereSQL(whereSQL, new { City = city});
        }

        public List<CityModel> GetListByFilter(CityFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter);
        }

        public CityModel GetByFilter(CityFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter).FirstOrDefault();
        }

        private static string GetWhereSQLByFilter(CityFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";
            
            if(filter.CityIds.IsNotEmpty())
            {
                whereSQL += " And Id IN @CityIds";
            }
            if (filter.AreaIds.IsNotEmpty())
            {
                whereSQL += " And AreaId IN @AreaIds";
            }
            if (filter.IsCounty.HasValue)
            {
                whereSQL += " And IsCounty = @IsCounty";
            }
            if (filter.IsCounty.HasValue)
            {
                whereSQL += " And IsCounty = @IsCounty";
            }
            if (filter.WaterDivisionIds.IsNotEmpty())
            {
                whereSQL += " And WaterDivisionId IN @WaterDivisionIds";
            }
            if (filter.Types.IsNotEmpty())
            {
                whereSQL += " And Type IN @Types";
            }
            if (string.IsNullOrWhiteSpace(filter.City) == false)
            {
                whereSQL += " And City = @City";
            }
            if (filter.IsCity23.HasValue)
            {
                whereSQL += @" And 
                (
	                (IsCounty = 1)
	                Or
	                (IsCounty = 0 And Id = 23)
                )
";
            }

            return whereSQL;
        }

        public List<CityWaterDivisionJoinModel> GetWaterDivisions(string waterDivisionName = null)
        {
            string sql = @"
SELECT C.Id CityId,C.City,C.Sort,C.WaterDivisionId ,WD.Name WaterDivision
FROM City C
JOIN WaterDivision WD ON C.WaterDivisionId = WD.Id";
            if (string.IsNullOrWhiteSpace(waterDivisionName) == false)
            {
                sql += " Where WD.Name =@waterDivisionName";
            }
            return GetListBySQL<CityWaterDivisionJoinModel>(sql, new { waterDivisionName });
        }
    }
}