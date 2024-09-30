using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Repositories
{
    public class DisinfectorRepository : BaseEMISRepository<DisinfectorModel>
    {
        public List<DisinfectorModel> GetByFilter(DisinfectorFilterParameter filter)
        {
            string sql = GetSQLByFilter(filter);

            return GetListBySQL<DisinfectorModel>(sql, filter);
        }

        public List<DisinfectorReportModel> GetReportByFilter(DisinfectorFilterParameter filter)
        {
            string querySql = GetSQLByFilter(filter);

            string sql = $@"
SELECT DisinfectInstrument,SUM(Amount) Quantity
FROM
(
	{querySql}
) AS T
GROUP BY T.DisinfectInstrument";

            return GetListBySQL<DisinfectorReportModel>(sql, filter);
        }
        public List<int> GetROCYears()
        {
            string sql = @"
SELECT ROCyear
FROM Disinfector
WHERE ROCyear IS NOT NULL
GROUP BY ROCyear";
            return GetListBySQL<string>(sql).Select(e => e.TryToInt()).Where(e => e.HasValue).Select(e => e.Value).ToList();
        }
        public List<DisinfectorCityReportModel> GetCityReport(DisinfectorFilterCityReportParameter filter)
        {
            string whereSQL = "Where 1=1";

            if(filter.CityIds.IsNotEmpty())
            {
                whereSQL += " AND C.Id IN @CityIds";
            }

            if (filter.TownIds.IsNotEmpty())
            {
                whereSQL += " AND T.Id IN @TownIds";
            }

            if (filter.UseTypes.IsNotEmpty())
            {
                whereSQL += " AND D.UseType IN @UseTypes";
            }

            if (filter.Year.HasValue)
            {
                whereSQL += " AND D.ROCyear = @Year";
            }

            if (string.IsNullOrWhiteSpace(filter.Name) == false)
            {
                whereSQL += " AND D.DisinfectInstrument = @Name";
            }

            string sql = $@"
SELECT C.Id CityId,C.City,UseType,DisinfectInstrument Name,SUM(ISNULL(D.Amount,0)) Amount ,MAX(D.UpdateTime)UpdateTime
FROM Disinfector D
JOIN  City C ON D.City = C.City
JOIN Town T ON T.Name = D.Town AND T.CityId = C.Id
{whereSQL}
GROUP BY C.Id,C.City,D.UseType,D.DisinfectInstrument
                        ";

            return GetListBySQL<DisinfectorCityReportModel>(sql, filter);
        }

        public List<DisinfectorCityReportModel> GetTownReport(DisinfectorFilterCityReportParameter filter)
        {
            string whereSQL = "Where 1=1";

            if (filter.CityIds.IsNotEmpty())
            {
                whereSQL += " AND C.Id IN @CityIds";
            }

            if (filter.TownIds.IsNotEmpty())
            {
                whereSQL += " AND T.Id IN @TownIds";
            }

            if (filter.UseTypes.IsNotEmpty())
            {
                whereSQL += " AND D.UseType IN @UseTypes";
            }

            if (filter.Year.HasValue)
            {
                whereSQL += " AND D.ROCyear = @Year";
            }

            if (string.IsNullOrWhiteSpace(filter.Name) == false)
            {
                whereSQL += " AND D.DisinfectInstrument = @Name";
            }

            string sql = $@"
SELECT T.Id TownId,D.Town,C.Id CityId,C.City,UseType,DisinfectInstrument Name,SUM(ISNULL(D.Amount,0)) Amount ,MAX(D.UpdateTime)UpdateTime
FROM Disinfector D
JOIN  City C ON D.City = C.City
JOIN Town T ON T.Name = D.Town AND T.CityId = C.Id
{whereSQL}
GROUP BY T.Id,D.Town,C.Id,C.City,D.UseType,D.DisinfectInstrument
                        ";

            return GetListBySQL<DisinfectorCityReportModel>(sql, filter);
        }
        public List<DisinfectorSummaryTownReportModel> GetSummaryTownReport()
        {
            string sql = GetSummaryTownReportSQL();

            return GetListBySQL<DisinfectorSummaryTownReportModel>(sql);
        }
        private string GetSummaryTownReportSQL()
        {
            string sql = @"
SELECT
	T.City
    ,T.Town
	,ISNULL(SUM(T.SprayerCount),0) SprayerCount
	,ISNULL(SUM(T.DisinfectorCount),0) DisinfectorCount
	,ISNULL(SUM(T.HotSmokeSachineCount),0) HotSmokeSachineCount
	,ISNULL(SUM(T.DisinfectionCarCount),0) DisinfectionCarCount
	,ISNULL(SUM(T.PressureWasherCount),0) PressureWasherCount

	,ISNULL(SUM(T.SprayerCAR),0) SprayerCAR
	,ISNULL(SUM(T.SprayeSrHI),0) SprayeSrHI
	,ISNULL(SUM(T.SprayeSrLO),0) SprayeSrLO
	,ISNULL(SUM(T.SMOK),0) SMOK

	,ISNULL(SUM(T.OtherCount),0) OtherCount
	,MAX(UpdateTime)UpdateTime
	,MAX(ConfirmTime) ConfirmTime
FROM
(
	SELECT 
		p.City
		, p.Town
		,ISNULL(SUM(p.[噴霧機]),0) SprayerCount
		,ISNULL(SUM(p.[消毒機(器)]),0) DisinfectorCount
		,ISNULL(SUM(p.[熱煙霧機]),0) HotSmokeSachineCount
		,ISNULL(SUM(p.[消毒車]),0) DisinfectionCarCount
		,ISNULL(SUM(p.[高壓清洗機]),0) PressureWasherCount


		,ISNULL(SUM(p.[車載式高壓噴霧機]),0) SprayerCAR
		,ISNULL(SUM(p.[高壓噴霧機]),0) SprayeSrHI
		,ISNULL(SUM(p.[超低容量噴霧機]),0) SprayeSrLO
		,ISNULL(SUM(p.[車載式煙霧機]),0) SMOK



		,ISNULL(SUM(p.[其他]),0) OtherCount
		,MAX(UpdateTime)UpdateTime
		,MAX(ConfirmTime) ConfirmTime
	FROM (
		SELECT City,Town,DisinfectInstrument,SUM(Amount)Amount,MAX(UpdateTime)UpdateTime,MAX(ConfirmTime)ConfirmTime
        From
        (
	        SELECT City,Town,DisinfectInstrument,Amount,UpdateTime,ConfirmTime
	        FROM Disinfector
	        WHERE DisinfectInstrument IN(N'噴霧機',N'消毒機(器)',N'熱煙霧機',N'消毒車',N'高壓清洗機',N'車載式高壓噴霧機',N'高壓噴霧機',N'超低容量噴霧機',N'車載式煙霧機')
	        UNION ALL
	        SELECT City,Town,'其他'DisinfectInstrument,Amount,UpdateTime,ConfirmTime
	        FROM Disinfector
	        WHERE DisinfectInstrument NOT IN(N'噴霧機',N'消毒機(器)',N'熱煙霧機',N'消毒車',N'高壓清洗機',N'車載式高壓噴霧機',N'高壓噴霧機',N'超低容量噴霧機',N'車載式煙霧機')
        ) as Dftort
        GROUP BY City,Town, DisinfectInstrument
	) t 
	PIVOT (
		-- 設定彙總欄位及方式
		MAX(Amount) 
		-- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
		FOR DisinfectInstrument IN ([噴霧機],[消毒機(器)],[熱煙霧機],[消毒車],[高壓清洗機],[車載式高壓噴霧機],[高壓噴霧機],[超低容量噴霧機],[車載式煙霧機],[其他])
	) p
	GROUP BY p.City,p.Town
) T
GROUP BY T.City,T.Town
";
            return sql;
        }

        public List<DisinfectorSummaryCityReportModel> GetSummaryCityReport()
        {
            string townSql = GetSummaryTownReportSQL();
            string sql = $@"
SELECT
	T.City
	,ISNULL(SUM(T.SprayerCount),0) SprayerCount
	,ISNULL(SUM(T.DisinfectorCount),0) DisinfectorCount
	,ISNULL(SUM(T.HotSmokeSachineCount),0) HotSmokeSachineCount
	,ISNULL(SUM(T.DisinfectionCarCount),0) DisinfectionCarCount
	,ISNULL(SUM(T.PressureWasherCount),0) PressureWasherCount

	,ISNULL(SUM(T.SprayerCAR),0) SprayerCAR
	,ISNULL(SUM(T.SprayeSrHI),0) SprayeSrHI
	,ISNULL(SUM(T.SprayeSrLO),0) SprayeSrLO
	,ISNULL(SUM(T.SMOK),0) SMOK


	,ISNULL(SUM(T.OtherCount),0) OtherCount
	,MAX(UpdateTime)UpdateTime
	,MAX(ConfirmTime) ConfirmTime
FROM
(
	{townSql}
) T
GROUP BY T.City";
            return GetListBySQL<DisinfectorSummaryCityReportModel>(sql);
        }

        //跨縣市調度
        private string GetSupportCityTownReportSQL()
        {
            string sql = @"
SELECT
	T.City
    ,T.Town
	,ISNULL(SUM(T.SprayerCount),0) SprayerCount
	,ISNULL(SUM(T.DisinfectorCount),0) DisinfectorCount
	,ISNULL(SUM(T.HotSmokeSachineCount),0) HotSmokeSachineCount
	,ISNULL(SUM(T.DisinfectionCarCount),0) DisinfectionCarCount
	,ISNULL(SUM(T.PressureWasherCount),0) PressureWasherCount

	,ISNULL(SUM(T.SprayerCAR),0) SprayerCAR
	,ISNULL(SUM(T.SprayeSrHI),0) SprayeSrHI
	,ISNULL(SUM(T.SprayeSrLO),0) SprayeSrLO
	,ISNULL(SUM(T.SMOK),0) SMOK

	,ISNULL(SUM(T.OtherCount),0) OtherCount
	,MAX(UpdateTime)UpdateTime
	,MAX(ConfirmTime) ConfirmTime
FROM
(
	SELECT 
		p.City
		, p.Town
		,ISNULL(SUM(p.[噴霧機]),0) SprayerCount
		,ISNULL(SUM(p.[消毒機(器)]),0) DisinfectorCount
		,ISNULL(SUM(p.[熱煙霧機]),0) HotSmokeSachineCount
		,ISNULL(SUM(p.[消毒車]),0) DisinfectionCarCount
		,ISNULL(SUM(p.[高壓清洗機]),0) PressureWasherCount


		,ISNULL(SUM(p.[車載式高壓噴霧機]),0) SprayerCAR
		,ISNULL(SUM(p.[高壓噴霧機]),0) SprayeSrHI
		,ISNULL(SUM(p.[超低容量噴霧機]),0) SprayeSrLO
		,ISNULL(SUM(p.[車載式煙霧機]),0) SMOK



		,ISNULL(SUM(p.[其他]),0) OtherCount
		,MAX(UpdateTime)UpdateTime
		,MAX(ConfirmTime) ConfirmTime
	FROM (
		SELECT City,Town,DisinfectInstrument,SUM(SupportCityNum)SupportCityNum,MAX(UpdateTime)UpdateTime,MAX(ConfirmTime)ConfirmTime
        From
        (
	        SELECT City,Town,DisinfectInstrument,UpdateTime,ConfirmTime,
                   Case When ISNULL(IsSupportCity, '') = 1 Then ISNULL(SupportCityNum, 0) Else 0 End AS SupportCityNum
	        FROM Disinfector
	        WHERE DisinfectInstrument IN(N'噴霧機',N'消毒機(器)',N'熱煙霧機',N'消毒車',N'高壓清洗機',N'車載式高壓噴霧機',N'高壓噴霧機',N'超低容量噴霧機',N'車載式煙霧機')
	        UNION ALL
	        SELECT City,Town,'其他'DisinfectInstrument,UpdateTime,ConfirmTime,
                   Case When ISNULL(IsSupportCity, '') = 1 Then ISNULL(SupportCityNum, 0) Else 0 End AS SupportCityNum
	        FROM Disinfector
	        WHERE DisinfectInstrument NOT IN(N'噴霧機',N'消毒機(器)',N'熱煙霧機',N'消毒車',N'高壓清洗機',N'車載式高壓噴霧機',N'高壓噴霧機',N'超低容量噴霧機',N'車載式煙霧機')
        ) as Dftort
        GROUP BY City,Town, DisinfectInstrument
	) t 
	PIVOT (
		-- 設定彙總欄位及方式
		MAX(SupportCityNum) 
		-- 設定轉置欄位，並指定轉置欄位中需彙總的條件值作為新欄位
		FOR DisinfectInstrument IN ([噴霧機],[消毒機(器)],[熱煙霧機],[消毒車],[高壓清洗機],[車載式高壓噴霧機],[高壓噴霧機],[超低容量噴霧機],[車載式煙霧機],[其他])
	) p
	GROUP BY p.City,p.Town
) T
GROUP BY T.City,T.Town
";
            return sql;
        }

        //跨縣市調度
        public List<DisinfectorSummaryCityReportModel> GetSupportCityReport()
        {
            string townSql = GetSupportCityTownReportSQL();
            string sql = $@"
SELECT
	T.City
	,ISNULL(SUM(T.SprayerCount),0) SprayerCount
	,ISNULL(SUM(T.DisinfectorCount),0) DisinfectorCount
	,ISNULL(SUM(T.HotSmokeSachineCount),0) HotSmokeSachineCount
	,ISNULL(SUM(T.DisinfectionCarCount),0) DisinfectionCarCount
	,ISNULL(SUM(T.PressureWasherCount),0) PressureWasherCount

	,ISNULL(SUM(T.SprayerCAR),0) SprayerCAR
	,ISNULL(SUM(T.SprayeSrHI),0) SprayeSrHI
	,ISNULL(SUM(T.SprayeSrLO),0) SprayeSrLO
	,ISNULL(SUM(T.SMOK),0) SMOK


	,ISNULL(SUM(T.OtherCount),0) OtherCount
	,MAX(UpdateTime)UpdateTime
	,MAX(ConfirmTime) ConfirmTime
FROM
(
	{townSql}
) T
GROUP BY T.City";
            return GetListBySQL<DisinfectorSummaryCityReportModel>(sql);
        }

        private static string GetSQLByFilter(DisinfectorFilterParameter filter)
        {
            string where = GetWhereSQLByFilter(filter);
            string sql = $@"
SELECT D.*
FROM [dbo].[Disinfector] D
JOIN City C ON D.City = C.City
JOIN Town T ON D.Town = T.Name AND T.CityId = C.Id
{where}";
            return sql;
        }
        private static string GetWhereSQLByFilter(DisinfectorFilterParameter filter)
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