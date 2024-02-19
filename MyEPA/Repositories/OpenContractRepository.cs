using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Repositories
{
    public class OpenContractRepository : BaseEMISRepository<OpenContractModel>
    {
        public List<OpenContractModel> GetByCity(int city)
        {
            string whereSql = "Where City = @City";
            return GetListByWhereSQL(whereSql, new { City = city });
        }
        public List<OpenContractModel> GetByFilter(OpenContractFilterParameter filter)
        {
            string whereSql = GetWhereSQLByFilter(filter);
            string querySQL = $@"SELECT * 
                                 FROM [OpenContract] oc WITH(NOLOCK)
                                 {whereSql}";
            
            return GetListBySQL<OpenContractModel>(querySQL, filter);
        }
        public List<OpenContractJoinDetailSearchModel> GetJoinDetailsByFilter(OpenContractFilterParameter filter)
        {
            string whereSql = GetWhereSQLByFilter(filter);
            string querySQL = $@"SELECT 
	                                oc.Id
	                                ,c.City CityName
	                                ,T.Name TownName
	                                ,oct.Name TypeName
	                                ,oc.Name OpenContractName
	                                ,ocd.Items
	                                ,ocd.Unit
	                                ,ocd.Count
	                                ,ocd.Price
	                                ,ocd.Budge
                                FROM OpenContract oc WITH(NOLOCK)
                                Left JOIN OpenContractDetail ocd WITH(NOLOCK) ON oc.Id = ocd.OpenContractId
                                JOIN ResourceType oct ON oc.ResourceTypeId = oct.Id
                                Left JOIN City c ON oc.CityId = c.Id
                                Left JOIN Town t ON oc.TownId = T.Id
                                {whereSql}
                            ";
            
            return GetListBySQL<OpenContractJoinDetailSearchModel>(querySQL, filter);
        }
        private string GetWhereSQLByFilter(OpenContractFilterParameter filter)
        {
            DateTime now = DateTimeHelper.GetCurrentTime();
            string whereSQL = "WHERE 1=1";
            if(filter.CityIds.IsNotEmpty())
            {
                whereSQL += " AND oc.CityId IN @CityIds";
            }
            if (filter.ResourceTypeIds.IsNotEmpty())
            {
                whereSQL += " AND oc.ResourceTypeId IN @ResourceTypeIds";
            }
            if (filter.TownIds.IsNotEmpty())
            {
                whereSQL += " AND oc.TownId IN @TownIds";
            }
            if (filter.Year.HasValue)
            {
                whereSQL += " AND (YEAR(oc.OContractDateBegin) = @Year OR YEAR(oc.OContractDateEnd) = @Year)";
            }
            if(filter.IsEffective.HasValue && filter.IsEffective.Value)
            {
                whereSQL += $" AND oc.OContractDateBegin <= '{now.ToString("yyyy/MM/dd HH:mm:ss")}'";
                whereSQL += $" AND oc.OContractDateEnd > '{now.ToString("yyyy/MM/dd HH:mm:ss")}'";
            }
            return whereSQL;
        }
    }
}