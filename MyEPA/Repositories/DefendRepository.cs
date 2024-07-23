using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Models.QueryModel;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Repositories
{
    public class DefendRepository : BaseEMISRepository<DefendModel>
    {
        public List<DefendCityCountQueryModel> GetCityCountByDiasterId(int diasterId)
        {
            string sql = @"
                            SELECT C.City,T.Name Town,Count(D.Id)Count
                            FROM Defend D
                            JOIN City C ON C.Id = D.CityId
                            JOIN Town T ON T.Id = D.TownId
                            WHERE DiasterId = @diasterId
                            GROUP BY C.City,T.Name";
            return GetListBySQL<DefendCityCountQueryModel>(sql, new { DiasterId = diasterId});
        }

        //鄉鎮未通報
        public List<UnNotificationJoinDefendModel> GetUnNotifications(int diasterId, int cityId)
        {
            string sql = @"
                            SELECT T.CityId,T.Name Town,U.Name,U.OfficePhone,D.Status
                            FROM Town T
                            LEFT JOIN Defend D ON T.Id = D.TownId AND D.DiasterId = @DiasterId
                            LEFT JOIN Users U ON T.CityId = U.CityId AND T.Id = U.TownId AND U.MainContacter = '是'
                            WHERE IsTown = 1 --鄉鎮通報
                            And (D.Id IS NULL Or D.Status = 1)  --0未通報 1未確認(鄉鎮已通報)
                            AND T.CityId = @CityId";
            return GetListBySQL<UnNotificationJoinDefendModel>(sql, new { DiasterId = diasterId, CityId = cityId });
        }
        public DefendModel Get(int dutyId, int diasterId, int cityId, int townId)
        {
            var result = GetByFilter(new DefendParameter
            {
                DutyIds = dutyId.ToListCollection(),
                CityIds = cityId.ToListCollection(),
                TownIds = townId.ToListCollection(),
                DiasterIds = diasterId.ToListCollection(),
            }).FirstOrDefault();

            return result;
        }
        public List<DefendModel> GetByFilter(DefendParameter filter)
        {
            string whereSql = GetWhereSQLByFilter(filter);

            return GetListByWhereSQL(whereSql, filter);
        }
        private string GetWhereSQLByFilter(DefendParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (filter.DutyIds.IsNotEmpty())
            {
                whereSQL += " AND DutyId IN @DutyIds";
            }

            if (filter.CityIds.IsNotEmpty())
            {
                whereSQL += " AND CityId IN @CityIds";
            }

            if (filter.TownIds.IsNotEmpty())
            {
                whereSQL += " AND TownId IN @TownIds";
            }

            if (filter.DiasterIds.IsNotEmpty())
            {
                whereSQL += " AND DiasterId IN @DiasterIds";
            }

            if (filter.Status.IsNotEmpty())
            {
                whereSQL += " AND Status IN @Status";
            }

            return whereSQL;
        }

        public List<DefendReportModel> GetReport(DefendReportFilterModel input)
        {
            string whereCity = string.Empty;
            if (input.CityId.HasValue)
            {
                whereCity = " AND C.Id = @CityId";
            }

            string sql = $@"
SELECT 
	C.Id CityId
    ,D.TownId
	,C.City CityName
	,A.Name AreaName
	,ISNULL(D.Status,0) Status
	,CD.UnNotificationCount
	,CD.WaitingCount
	,CD.ConfirmCount
	,U.Name UserName
	,U.OfficePhone
	,D.CreateDate
	,D.UpdateDate
	,D.ConfirmTime
FROM City C 
JOIN Area A ON C.AreaId = A.Id
JOIN 
(
    --鄉鎮數量資料
	SELECT p.CityId,p.City,ISNULL([0],0) UnNotificationCount,ISNULL([1],0) WaitingCount,ISNULL([2],0) ConfirmCount
	FROM
	(
		SELECT C.Id CityId,ISNULL(D.Status,0) Status, C.City,COUNT(D.Id) Count
		FROM City C 
		LEFT JOIN Defend D ON C.Id = D.CityId AND DiasterId = @DiasterId
		GROUP BY C.City,C.Id,D.Status
	) T
	PIVOT (
		-- 設定彙總欄位及方式
		MAX(T.Count) 
		-- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
		FOR T.Status IN ([0],[1],[2])
	) p
) AS CD ON C.Id = CD.CityId 
LEFT JOIN Defend D ON C.Id = D.CityId AND D.DiasterId = @DiasterId AND D.DutyId = {DutyEnum.EPB.ToInteger()}
LEFT JOIN Users U ON C.Id = U.CityId AND U.MainContacter = '是' AND U.DutyId = {DutyEnum.EPB.ToInteger()}
Where C.IsCounty = 1 {whereCity}
Order By C.Sort
";
            

            return GetListBySQL<DefendReportModel>(sql, input);
        }

        public List<DefendTownReportModel> GetTownReport(DefendTownReportFilterModel input)
        {
            string sql = $@"
SELECT 
	T.Id TownId
	,T.Name TownName
	,ISNULL(D.Status,0) Status
	,U.Name UserName
	,U.OfficePhone
	,D.CreateDate
	,D.UpdateDate
	,D.ConfirmTime
FROM Town T
LEFT JOIN Defend D ON T.Id = D.TownId AND D.DiasterId = @DiasterId
LEFT JOIN Users U ON T.Id = U.TownId AND U.MainContacter = '是'
Where 1 = 1
";
            if (input.CityId.HasValue)
            {
                sql += " AND T.CityId = @CityId";
            }

            return GetListBySQL<DefendTownReportModel>(sql, input);
        }

        public List<DefendTownQuestionModel> GetDefendTownQuestions(int diasterId, int? townId, int? cityId)
        {
            string sql = @"
SELECT 
	T.Name TownName
	,Q.Title QuestionTitle
	,Q.Note
	,DQ.Ans
	,DQ.Remark
	,DDQ.Sort
    ,DQ.UpdateDate
FROM Defend D
JOIN DefendQuestion DQ ON D.Id = DQ.DefendId
JOIN Question Q ON DQ.QuestionId = Q.Id
JOIN DefendDutyQuestion DDQ ON DDQ.QuestionId = Q.Id AND D.DutyId = DDQ.DutyId
JOIN Town T ON T.Id = D.TownId
WHERE DiasterId = @DiasterId";
            if(townId.HasValue)
            {
                sql += " AND TownId = @TownId";
            }
            if (cityId.HasValue)
            {
                sql += " AND T.CityId = @CityId";
            }
            return GetListBySQL<DefendTownQuestionModel>(sql, new { DiasterId = diasterId, TownId = townId, CityId = cityId });
        }
    }
}