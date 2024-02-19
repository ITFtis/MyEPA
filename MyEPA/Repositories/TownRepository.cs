using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Repositories
{
    public class TownRepository : BaseEMISRepository<TownModel>
    {
        public TownModel GetByTownName(int cityId, string town)
        {
            string whereSQL = "Where CityId = @cityId AND  Name=@Town";
            return GetByWhereSQL(whereSQL, new { Town = town, CityId = @cityId });
        }


        public List<TownModel> GetByCityId(int cityId,bool? isTown = null)
        {
            string whereSQL = "Where CityId = @cityId";

            if(isTown.HasValue)
            {
                whereSQL += " And IsTown = @IsTown";
            }

            return GetListByWhereSQL(whereSQL, new { CityId = @cityId, IsTown = isTown });
        }
        public TownModel GetByFilter(TownFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter).FirstOrDefault();
        }
        public List<TownModel> GetListByFilter(TownFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter);
        }
        private static string GetWhereSQLByFilter(TownFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (filter.CityIds.IsNotEmpty())
            {
                whereSQL += " And CityId IN @CityIds";
            }
            if (filter.Ids.IsNotEmpty())
            {
                whereSQL += " And Id IN @Ids";
            }
            if (filter.IsTown.HasValue)
            {
                whereSQL += " And IsTown = @IsTown";
            }
            if (string.IsNullOrWhiteSpace(filter.TownName) == false)
            {
                whereSQL += " And Name = @TownName";
            }
            return whereSQL;
        }
    }
}