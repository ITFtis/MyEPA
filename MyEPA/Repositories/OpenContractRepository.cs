using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Repositories.BaseRepositories;
using MyEPA.Services;
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

        public List<OpenContractCountModel> GetCountByFilter(OpenContractFilterParameter filter)
        {
            string whereSql = GetWhereSQLByFilter(filter);
            string querySQL = $@"SELECT oc.*, 
                                        b.DetailCount, rt.Name AS ResourceTypeName,
                                        c.City AS CityName, t.Name AS TownName
                                    FROM OpenContract oc
                                    Left Join
                                    (
	                                    SELECT OpenContractId, Count(1) AS DetailCount
	                                    FROM OpenContractDetail
	                                    Group By OpenContractId
                                    )b On Id = b.OpenContractId
                                    Left Join ResourceType rt On oc.ResourceTypeId = rt.Id
                                    Left JOIN City c ON oc.CityId = c.Id
                                    Left JOIN Town t ON oc.TownId = t.Id
                                 {whereSql}";

            return GetListBySQL<OpenContractCountModel>(querySQL, filter);
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
            if (filter.YearRange.HasValue)
            {
                whereSQL += @" AND 
                            (
                                @YearRange >= YEAR(oc.OContractDateBegin)
                                AND 
                                @YearRange <= YEAR(oc.OContractDateEnd)
                            )";
            }
            if(filter.IsEffective.HasValue && filter.IsEffective.Value)
            {
                whereSQL += $" AND oc.OContractDateBegin <= '{now.ToString("yyyy/MM/dd HH:mm:ss")}'";
                whereSQL += $" AND oc.OContractDateEnd > '{now.ToString("yyyy/MM/dd HH:mm:ss")}'";
            }
            if (filter.IsEPB.HasValue && filter.IsEPB.Value)
            {
                whereSQL += " AND t.Name = '環保局'";
            }
            return whereSQL;
        }

        /// <summary>
        /// 複製來源主約Id
        /// </summary>
        /// <param name="user"></param>
        /// <param name="copyId"></param>
        /// <returns></returns>
        public int CopyOpenContractById(UserBriefModel user, int copyId)
        {
            //主約
            var oc = Get(copyId);
            if (oc == null)
            {
                return -1;
            }

            oc.Name = "(複製)_oooo_" + oc.Name;
            oc.CreateDate = DateTimeHelper.GetCurrentTime();
            oc.UpdateDate = DateTimeHelper.GetCurrentTime();
            oc.CreateUser = user.UserName;
            oc.UpdateUser = user.UserName;
            oc.CityId = user.CityId;
            oc.TownId = user.TownId;

            oc.Status = -1;
            var id = CreateAndResultIdentity<int>(oc);

            return id;
        }
    }
}