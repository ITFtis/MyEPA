using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class DamageRepository : BaseEMISRepository<DamageModel>
    {
		public List<DamageYearReportModel> GetYearReport(int year)
        {
			string sql = @"
SELECT 
	DiasterId
	,SUM(FloodArea)FloodArea
	,SUM(CLE_MUD)CLE_MUD
	,SUM(CLE_Garbage)CLE_Garbage
	,SUM(CleaningMemberQuantity)CleaningMemberQuantity
	,SUM(NationalArmyQuantity)NationalArmyQuantity
	,SUM(CLE_Disinfect)CLE_Disinfect
FROM [dbo].[Damage]
WHERE Datepart(year,ReportDay) = @year
GROUP BY DiasterId
";
			return GetListBySQL<DamageYearReportModel>(sql, new { year });
        }
		public List<DamageJoinModel> GetListByFilter(DamageFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);

            string sql = $@"
Select D.*,C.City CityName,T.Name TownName,T.IsTown,T.CityId
From Damage D
JOIN City C ON C.Id = D.CityId
Left JOIN Town T ON T.Id = D.TownId
{whereSQL}
";


            return GetListBySQL<DamageJoinModel>(sql, filter);
        }

        public List<DamageJoinModel> GetReportListByFilter(DamageFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);

            string sql = $@"
Select D.*,C.City CityName, C.Sort AS CitySort, T.Id AS TownId, T.Name TownName
From Damage D
JOIN City C ON C.Id = D.CityId
Left JOIN Town T ON T.Id = D.TownId
{whereSQL}
And D.ReportDay Is Not Null  --災情通報資料
";


            return GetListBySQL<DamageJoinModel>(sql, filter);
        }

        /// <summary>
        /// 環境清理資料
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<DamageJoinModel> GetCleanListByFilter(DamageFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);

            string sql = $@"
Select D.*,C.City CityName, C.Sort AS CitySort, T.Name TownName
From Damage D
JOIN City C ON C.Id = D.CityId
Left JOIN Town T ON T.Id = D.TownId
{whereSQL}
And D.CleanDay Is Not Null  --環境清理資料
";


            return GetListBySQL<DamageJoinModel>(sql, filter);
        }

        public List<DamageReportModel> GetReport(DamageReportFilterModel input)
        {
			string whereCity = string.Empty;
			if (input.CityId.HasValue)
			{
				whereCity = " AND C.Id = @CityId";
			}
			string sql = $@"
SELECT 
	C.Id CityId
	,C.City CityName
	,A.Name AreaName
	,CD2.TeamWaitingCount
	,CD2.TeamConfirmCount
	,CD.WaitingCount
	,CD.ConfirmCount
	,U.Name UserName
	,U.OfficePhone
	,D.CreateDate
	,D.UpdateDate
	,D.ConfirmTime
	,D.TeamConfirmTime
	,ISNULL(DM.IsDone,0)IsDone
FROM City C 
JOIN Area A ON C.AreaId = A.Id
JOIN 
(
    --鄉鎮數量資料
	SELECT p.CityId,p.City,ISNULL([1],0) WaitingCount,ISNULL([2],0) ConfirmCount
	FROM
	(
		SELECT C.Id CityId,ISNULL(D.Status,0) Status, C.City,COUNT(D.Id) Count
		FROM City C 
		LEFT JOIN [Damage] D ON C.Id = D.CityId AND DiasterId = @DiasterId --AND D.DutyId = {DutyEnum.Cleaning.ToInteger()}
		GROUP BY C.City,C.Id,D.Status,D.DutyId
	) T
	PIVOT (
		-- 設定彙總欄位及方式
		MAX(T.Count) 
		-- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
		FOR T.Status IN ([1],[2],[3])
	) p
) AS CD ON C.Id = CD.CityId 
JOIN 
(
    --鄉鎮數量資料
	SELECT p.CityId,p.City,ISNULL([1],0) TeamWaitingCount,ISNULL([2],0) TeamConfirmCount
	FROM
	(
		SELECT C.Id CityId,ISNULL(D.Status,0) Status, C.City,COUNT(D.Id) Count
		FROM City C 
		LEFT JOIN [Damage] D ON C.Id = D.CityId AND DiasterId = @DiasterId --AND D.DutyId = {DutyEnum.EPB.ToInteger()}
		GROUP BY C.City,C.Id,D.Status,D.DutyId
	) T
	PIVOT (
		-- 設定彙總欄位及方式
		MAX(T.Count) 
		-- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
		FOR T.Status IN ([1],[2],[3])
	) p
) AS CD2 ON C.Id = CD2.CityId 
LEFT JOIN 
(
	SELECT CityId, MAX(CreateDate)CreateDate,MAX(UpdateDate)UpdateDate, MAX(ConfirmTime)ConfirmTime,MAX(TeamConfirmTime)TeamConfirmTime
	FROM [Damage]
	WHERE DiasterId = @DiasterId
	GROUP BY CityId
) AS D ON C.Id = D.CityId
LEFT JOIN Users U ON C.Id = U.CityId AND U.MainContacter = '是' AND U.DutyId = {DutyEnum.EPB.ToInteger()}
LEFT JOIN [DamageMain] DM ON C.Id = DM.CityId AND DM.DiasterId = @DiasterId
Where 1 = 1 AND C.[IsCounty] = 1 {whereCity}
Order By C.Sort
";
            

            return GetListBySQL<DamageReportModel>(sql, input);
        }
        public List<DamageStatisticsModel> GetStatistics(DamageReportFilterModel filter)
        {
			string whereSQL = string.Empty;
			string damageWhereSQL = "WHERE d.DiasterId = @DiasterId";
			if (filter.CityId.HasValue)
			{
				whereSQL += " AND C.Id = @CityId";
			}
            if (filter.AreaId.HasValue)
            {
                whereSQL += " AND C.AreaId = @AreaId";
            }
            if (filter.Date.HasValue)
			{
				damageWhereSQL += " AND d.CleanDay = @Date";
			}            

            string sql = $@"
SELECT
	D.FloodArea
	,D.CLE_MUD
	,D.PR_Garbage
	,D.CLE_Trash
,D.CLE_Garbage
	,D.CleaningMemberQuantity
	,D.NationalArmyQuantity
	,D.CLE_DisinfectorL
	,D.CLE_DisinfectorW
	,D.CLE_EquipmentNum
	,D.CLE_CarNum
	,D.DisinfectArea
	,D.CleanUpdateDate
	,C.Id CityId
	,C.City CityName
	,Town.Id TownId
	,Town.Name TownName
	,ISNULL(Dfectant.SolidCount,0) DisinfectantSolidAmount	    --消毒藥品固體
	,ISNULL(Dfectant.LiquidCount,0) DisinfectantLiquidAmount    --消毒藥品液體
	,admip.Area CityArea
	,admit.Area TownArea
	,DM.IsDone
FROM Town
JOIN City C ON Town.CityId = C.Id AND C.[IsCounty] = 1
LEFT JOIN 
(
		SELECT p.City, p.Town,p.固體 SolidCount,p.液體 LiquidCount
		FROM (
			SELECT  City,Town,DrugState,SUM(Amount) Amount
			FROM Disinfectant
			GROUP BY City, Town,DrugState
		) t 
		PIVOT (
			-- 設定彙總欄位及方式
			MAX(Amount) 
			-- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
			FOR DrugState IN ([固體],[液體])
		) p
) AS Dfectant ON C.City = Dfectant.City AND  Town.Name = Dfectant.Town
LEFT JOIN
(
	SELECT  
		d.CityId
		,d.TownId
		,SUM(FloodArea)FloodArea										--居家環境淹水面積(公頃)
		,SUM(CLE_MUD)CLE_MUD											--已清除污泥(公噸)
		,SUM(PR_Garbage)PR_Garbage										--預估廢棄物量(公噸)
		,SUM(CLE_Trash)CLE_Trash										--已清除廢棄物(公噸)
		,SUM(ISNULL(CleaningMemberQuantity,0))CleaningMemberQuantity	--已動用清除人力(人次)
		,SUM(ISNULL(NationalArmyQuantity,0))NationalArmyQuantity		--已動用國軍人力(人次)
		,SUM(ISNULL(DisinfectArea,0))DisinfectArea						--已消毒面積(公頃)
        ,SUM(ISNULL(CLE_Garbage,0))CLE_Garbage						    --已清除廢棄物(公噸)
		,SUM(ISNULL(CLE_DisinfectorL,0))CLE_DisinfectorL				--已使用藥劑數量(公升
		,SUM(ISNULL(CLE_DisinfectorW,0))CLE_DisinfectorW				--已使用藥劑數量(公斤)
		,SUM(ISNULL(CLE_EquipmentNum,0))CLE_EquipmentNum				--已使用機具(總數量)
		,SUM(ISNULL(CLE_CarNum,0))CLE_CarNum							--已使用車輛(總數量)
		,MAX(d.CleanUpdateDate)CleanUpdateDate
	FROM Damage d
	{damageWhereSQL}
	And D.CleanDay Is Not Null  --環境清理資料
	GROUP BY d.CityId,d.TownId
) AS D ON D.CityId = C.Id AND D.TownId = Town.ID
LEFT JOIN admip ON C.Id = admip.CityId
LEFT JOIN ADMIT ON Town.Id = admit.TownId
LEFT JOIN DamageMain DM ON DM.DiasterId =  @DiasterId AND DM.IsDone = 1 AND DM.CityId = C.Id
Where 1=1
{whereSQL}
Order By C.Sort
";
			return GetListBySQL<DamageStatisticsModel>(sql, filter);
        }

        public List<DamageTownReportModel> GetTownReport(DamageTownReportFilterModel input)
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
LEFT JOIN Damage D ON T.Id = D.TownId AND D.DiasterId = @DiasterId AND D.DutyId = {DutyEnum.Cleaning.ToInteger()}
LEFT JOIN Users U ON T.Id = U.TownId AND U.MainContacter = '是'
Where 1 = 1
";
            if (input.CityId.HasValue)
            {
                sql += " AND T.CityId = @CityId";
            }

            return GetListBySQL<DamageTownReportModel>(sql, input);
        }
        public bool IsExistsByFilter(DamageFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);

            string sql = $@"
Select TOP 1 *
From Damage D
{whereSQL}";

            return IsExistsBySQL(sql);
        }
        private string GetWhereSQLByFilter(DamageFilterParameter filter)
        {
            string whereSQL = "where 1 = 1";
			if (filter.Ids.IsNotEmpty())
			{
				whereSQL += " AND D.Id IN @Ids";
			}
			if (filter.DiasterIds.IsNotEmpty())
            {
                whereSQL += " AND D.DiasterId IN @DiasterIds";
            }
            if (filter.AreaId.HasValue)
            {
                whereSQL += " AND C.AreaId = @AreaId";
            }
            if (filter.CityIds.IsNotEmpty())
            {
                whereSQL += " AND D.CityId IN @CityIds";
            }
            if (filter.TownIds.IsNotEmpty())
            {
                whereSQL += " AND D.TownId IN @TownIds";
            }
			if (filter.TownId.HasValue)
			{               
                whereSQL += " AND D.TownId = @TownId";
            }
            if (filter.ReportDay.HasValue)
            {
                whereSQL += " AND D.ReportDay = @ReportDay";
            }
            if (filter.CleanDay.HasValue)
            {
                whereSQL += " AND D.CleanDay = @CleanDay";
            }
            if (filter.Type.HasValue)
			{
                if (filter.Type == FacilityDamageTypeEnum.ALL) 
				{
					whereSQL += $" AND( D.{FacilityDamageTypeEnum.DumpSiteDesc.ToString()} <> '' or D.{FacilityDamageTypeEnum.IncinerationPlantDesc.ToString()} <> '' or D.{FacilityDamageTypeEnum.Other.ToString()} <> '')";

				}
                else
                {
					whereSQL += $" AND D.{filter.Type.ToString()} <> ''";
				}
			
			}
			if (filter.StartTime.HasValue)
			{
				whereSQL += " AND DateDiff(Day, @StartTime, [CreateDate]) >= 0";
			}
			if (filter.EndTime.HasValue)
			{
				whereSQL += " AND DateDiff(Day, @EndTime, [CreateDate]) <= 0";
			}

            if (filter.CleanStartTime.HasValue)
            {
                whereSQL += " AND DateDiff(Day, @CleanStartTime, CleanCreateDate) >= 0";
            }
            if (filter.CleanEndTime.HasValue)
            {
                whereSQL += " AND DateDiff(Day, @CleanEndTime, CleanCreateDate) <= 0";
            }

            if (filter.HType.HasValue)
            {
				if (filter.HType == 1)
				{
                    //災情通報
                    whereSQL += " AND D.ReportDay Is Not Null";
				}
				else if (filter.HType == 2)
                {
                    //環境清理
                    whereSQL += " AND D.CleanDay Is Not Null";
                }
            }
            return whereSQL;
        }
    }
}