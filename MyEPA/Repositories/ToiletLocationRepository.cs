using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class ToiletLocationRepository : BaseEMISRepository<ToiletLocationModel>
    {
        public List<ToiletLocationManagementTownModel> GetManagementTownByFilter(ToiletLocationFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            string sql = $@"
SELECT TL.*,c.City+t.Name ManagementTownDisplay
FROM
(
	SELECT ManagementTownId,CityId
	FROM ToiletLocation
    {whereSQL}
	GROUP BY ManagementTownId,CityId
) TL
JOIN City c ON c.Id = TL.CityId
JOIN Town t ON T.Id = TL.ManagementTownId";
            return GetListBySQL<ToiletLocationManagementTownModel>(sql, filter);
        }

        public List<int> GetDiasterIds(ToiletLocationFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            string sql = $@"
SELECT DiasterId
FROM ToiletLocation
{whereSQL}
GROUP BY DiasterId";
            return GetListBySQL<int>(sql, filter);
        }
        public List<ToiletLocationStatisticsModel> GetStatisticsByFilter(ToiletLocationFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            string sql = $@"
SELECT 
	TL.Id
	,c.City CityName
	,t.Name TownName
	,TL.Address
	,TL.ToiletQuantity
	,TL.ToiletType
	,TL.StartDate
	,TL.EndDate
	,TL.ContactPerson
	,TL.ContactMethod
	,TL.Note
	,TCL.LastCleanDate
FROM 
(
	SELECT *
	FROM ToiletLocation
    {whereSQL}
) TL
LEFT JOIN 
(
	SELECT ToiletLocationId,MAX(Date) LastCleanDate
	FROM ToiletCleaningLog
	GROUP BY ToiletLocationId
) AS TCL ON TL.Id = TCL.ToiletLocationId
JOIN City c ON TL.CityId = c.Id
JOIN Town t ON TL.TownId = t.Id";
            return GetListBySQL<ToiletLocationStatisticsModel>(sql, filter);
        }

        public List<ToiletLocationModel> GetByFilter(ToiletLocationFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter);
        }
        private static string GetWhereSQLByFilter(ToiletLocationFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (filter.CityIds.IsNotEmpty())
            {
                whereSQL += " And CityId IN @CityIds";
            }
            if (filter.TownIds.IsNotEmpty())
            {
                whereSQL += " And TownId IN @TownIds";
            }
            if (filter.ManagementTownIds.IsNotEmpty())
            {
                whereSQL += " And ManagementTownId IN @ManagementTownIds";
            }
            if (filter.DiasterIds.IsNotEmpty())
            {
                whereSQL += " And DiasterId IN @DiasterIds";
            }
            return whereSQL;
        }
    }
}