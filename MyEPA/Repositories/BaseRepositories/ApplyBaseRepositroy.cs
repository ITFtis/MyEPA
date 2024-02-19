using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.BaseModels;
using MyEPA.Models.FilterParameter;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Repositories.BaseRepositories
{
    public class ApplyBaseRepositroy<T> : BaseEMISRepository<T> where T : ApplyBaseModel,new()
    {
        public List<ApplyBaseEPBStatusCountModel> GetEPBStatusCountByFilter(ApplyBaseFilterParameter filter)
        {
            string whereSql = GetWhereSQLByFilter(filter);
            string querySQL = $@"
SELECT CityId,TownId,EPBConfirmStatus,COUNT(Id) COUNT
FROM  [{_tableName}] ap
{whereSql}
GROUP BY CityId,TownId,EPBConfirmStatus";
            return GetListBySQL<ApplyBaseEPBStatusCountModel>(querySQL, filter);
        }
        public List<ApplyBaseStatusCountModel> GetStatusCountByFilter(ApplyBaseFilterParameter filter)
        {
            string whereSql = GetWhereSQLByFilter(filter);
            string querySQL = $@"
SELECT CityId,TownId,EPAConfirmStatus,EPBConfirmStatus,IsToEpa,SUM(apd.Quantity) Count
FROM [{_tableName}] as [ap] WITH(NOLOCK)
JOIN [{_tableName}Detail] as [apd] WITH(NOLOCK) ON ap.Id = apd.{_tableName}Id
{whereSql}
GROUP BY CityId,TownId,EPAConfirmStatus,EPBConfirmStatus,IsToEpa";
            return GetListBySQL<ApplyBaseStatusCountModel>(querySQL, filter);
        }

        public List<T> GetByFilter(ApplyBaseFilterParameter filter)
        {
            string whereSql = GetWhereSQLByFilter(filter);
            string querySQL = $@"SELECT * 
                                 FROM [{_tableName}] ap WITH(NOLOCK)
                                 {whereSql}";

            return GetListBySQL<T>(querySQL, filter);
        }

        protected static string GetWhereSQLByFilter(ApplyBaseFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (filter.Id.HasValue)
            {
                whereSQL += " AND ap.Id = @Id";
            }

            if (filter.DiasterIds.IsNotEmpty())
            {
                whereSQL += " AND ap.DiasterId IN @DiasterIds";
            }

            if (filter.CityIds.IsNotEmpty())
            {
                whereSQL += " AND ap.CityId IN @CityIds";
            }

            if (filter.TownIds.IsNotEmpty())
            {
                whereSQL += " AND ap.TownId IN @TownIds";
            }

            if (!string.IsNullOrWhiteSpace(filter.UserName))
            {
                whereSQL += " AND ap.CreateUser = @UserName";
            }

            if (filter.Status.HasValue)
            {
                whereSQL += $" AND ap.Status = {filter.Status.ToInteger()}";
            }

            if (filter.NotStatus.HasValue)
            {
                whereSQL += $" AND ap.Status != {filter.NotStatus.ToInteger()}";
            }

            if (filter.EPAConfirmStatus.IsNotEmpty()) 
            {
                whereSQL += $" AND ap.EPAConfirmStatus in @EPAConfirmStatus";
            }

            if (filter.EPBConfirmStatus.IsNotEmpty())
            {
                whereSQL += $" AND ap.EPBConfirmStatus in @EPBConfirmStatus";
            }

            if (filter.IsToEpa.HasValue) 
            {
                whereSQL += $" AND ap.IsToEpa = @IsToEpa";
            }

            return whereSQL;
        }

        public List<ApplySupportReportDetailModel> GetApplySupportReportDetais(string selectSql, ApplyBaseFilterParameter filter)
        {
            selectSql += GetWhereSQLByFilter(filter);
            selectSql += " ORDER BY ci.Sort";
            return GetListBySQL<ApplySupportReportDetailModel>(selectSql, filter);
        }

        public List<ApplySupportReportDetailModel> GetApplySupportReportDetais(ApplyBaseFilterParameter filter)
        {
            var sql = $@"
                        select 
                        ap.Id,
                        ap.DiasterId,
                        ap.CityId,
                        ci.city CityName, 
                        ap.TownId,
                        tn.Name TownName, 
                        ap.CreateDate,
                        ap.RequireDate,
                        ap.EstimationMethodDescribe,
                        u.Name CreateUser,
                        ap.ContactPerson,
                        ap.ContactPhone,
                        ap.[EPBConfirmStatus],
                        ap.[EPBConfirmUpdateTime],
                        ap.[EPBConfirmDescribe],
                        ap.[EPAConfirmStatus],
                        ap.[EPAConfirmUpdateTime],
                        ap.[EPAConfirmDescribe],
                        ap.[IsToEpa],
                        Status
                        from [dbo].[{_tableName}] as ap
                        join city as ci on  ap.CityId = ci.Id
                        join town as tn on  ap.townId = tn.Id
                        join users as u on ap.CreateUser = u.userName
                    ";

            return GetApplySupportReportDetais(sql, filter);
        }

        public List<ApplySupportProcessReport> GetProcessReportByFilter(string tableName, ApplyBaseFilterParameter applyBaseFilterParameter)
        {
            var sql = $@"select ap.id,ap.createUser , u.name , ap.RequireDate, c.city City, t.name Town, ap.createdate, ap.status, ap.EPAConfirmStatus, ap.EstimationMethodDescribe
                         from {tableName} as ap 
                         join city as c on ap.cityId = c.id
                         join town as t on ap.townId = t.id
                         join users as u on ap.createuser = u.username
                         {GetWhereSQLByFilter(applyBaseFilterParameter)}";
            return GetListBySQL<ApplySupportProcessReport>(sql, applyBaseFilterParameter);
        }

        public List<ApplySupportSubsidyReportCountingViewModel> GetEPBSubsidyReportCounting(ApplyBaseFilterParameter filter)
        {
            var whereSql = GetWhereSQLByFilter(filter);
            var sql = $@"
                        select 'ApplyPeople' ApplyType, [EPBConfirmStatus] ConfirmStatus, count(*) Count  from applypeople as ap
                        {whereSql}
                        group by [EPBConfirmStatus]
                        union 
                        select 'ApplyMedicine' ApplyType, [EPBConfirmStatus] ConfirmStatus, count(*) Count  from ApplyMedicine as ap
                        {whereSql}
                        group by [EPBConfirmStatus]
                        union
                        select 'ApplyCar' ApplyType, [EPBConfirmStatus] ConfirmStatus, count(*) Count  from ApplyCar as ap
                        {whereSql}
                        group by [EPBConfirmStatus]
                        union
                        select 'ApplySubsidy' ApplyType, [EPBConfirmStatus] ConfirmStatus, count(*) Count  from ApplySubsidy as ap
                        {whereSql}
                        group by [EPBConfirmStatus]
                        union
                        select 'ApplyOther' ApplyType, [EPBConfirmStatus] ConfirmStatus, count(*) Count  from ApplyOther as ap
                        {whereSql}
                        group by [EPBConfirmStatus]
                        union
                        select 'ApplyDisinfectionEquipment' ApplyTpye,  [EPBConfirmStatus] ConfirmStatus, count(*) Count from ApplyDisinfectionEquipment as ap
                        {whereSql}
                        group by [EPBConfirmStatus]
                     ";
            return GetListBySQL<ApplySupportSubsidyReportCountingViewModel>(sql,  filter);
        }

        public List<ApplySupportSubsidyReportCountingViewModel> GetEPASubsidyReportCounting(ApplyBaseFilterParameter filter)
        {
            var whereSql = GetWhereSQLByFilter(filter);
            var sql = $@"
                        select 'ApplyPeople' ApplyType, [EPAConfirmStatus] ConfirmStatus, count(*) Count  from applypeople as ap
                        {whereSql}
                        group by [EPAConfirmStatus]
                        union 
                        select 'ApplyMedicine' ApplyType, [EPAConfirmStatus] ConfirmStatus, count(*) Count  from ApplyMedicine as ap
                        {whereSql}
                        group by [EPAConfirmStatus]
                        union
                        select 'ApplyCar' ApplyType, [EPAConfirmStatus] ConfirmStatus, count(*) Count  from ApplyCar as ap
                        {whereSql}
                        group by [EPAConfirmStatus]
                        union
                        select 'ApplySubsidy' ApplyType, [EPAConfirmStatus] ConfirmStatus, count(*) Count  from ApplySubsidy as ap
                        {whereSql}
                        group by [EPAConfirmStatus]
                        union
                        select 'ApplyOther' ApplyType, [EPAConfirmStatus] ConfirmStatus, count(*) Count  from ApplyOther as ap
                        {whereSql}
                        group by [EPAConfirmStatus]
                        union
                        select 'ApplyDisinfectionEquipment' ApplyTpye,  [EPAConfirmStatus] ConfirmStatus, count(*) Count from ApplyDisinfectionEquipment as ap
                        {whereSql}
                        group by [EPAConfirmStatus]
                     ";
            return GetListBySQL<ApplySupportSubsidyReportCountingViewModel>(sql, filter);
        }

        /// <summary>
        /// 從 apply 系列取得 general 的 data 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<ApplyBaseModel> GetUnionApplySupportDatas(ApplyBaseFilterParameter filter)
        {

            var needFieldsSql = "TownId, Status, EPBConfirmStatus";
            var whereSql = GetWhereSQLByFilter(filter);
            var sql = $@"
                        select {needFieldsSql} from applypeople as ap
                        {whereSql}
                        union all
                        select {needFieldsSql} from ApplyCar as ap
                        {whereSql}
                        union all
                        select {needFieldsSql} from ApplyMedicine as ap
                        {whereSql}
                        union all
                        select {needFieldsSql} from ApplySubsidy as ap
                        {whereSql}
                        union all
                        select {needFieldsSql} from ApplyOther as ap
                        {whereSql}
                        union all
                        select {needFieldsSql} from ApplyDisinfectionEquipment as ap
                        {whereSql}
                     ";
            return GetListBySQL<ApplyBaseModel>(sql, filter);

        }

        public List<ApplySupportSubsidyReportCountingViewModel> GetEPBSubsidyReportCounting(int diasterId)
        {
            var filter = new ApplyBaseFilterParameter
            {
                DiasterIds = new List<int>() { diasterId },
                IsToEpa = false
            };


            return GetEPBSubsidyReportCounting(filter);
        }

        public List<ApplySupportSubsidyReportCountingViewModel> GetEPASubsidyReportCounting(int diasterId)
        {
            var filter = new ApplyBaseFilterParameter()
            {
                DiasterIds = new List<int>() { diasterId },
                IsToEpa = true
            };

            return GetEPASubsidyReportCounting(filter);
        }

        public void UpdateEpbConfrimStatusToProcess(int id) 
        {
            var updateSql = $@"update [{_tableName}] 
                               set EPBConfirmStatus = {(int)ApplyStatusEnum.Processing},
                                   EPBConfirmUpdateTime = GetDate()
                               where Id = @id 
                              ";
            ExecuteSQL(updateSql, new { id } );
        }

        public void UpdateEpaConfrimStatusToProcess(int id)
        {
            var updateSql = $@"update [{_tableName}] 
                               set EPAConfirmStatus = {(int)ApplyStatusEnum.Processing},
                                   EPAConfirmUpdateTime = GetDate()
                               where Id = @id 
                              ";
            ExecuteSQL(updateSql, new { id });
        }

        public void UpdateEpbConfrimStatus(int id, ApplyStatusEnum status, string description)
        {
            var updateSql = $@"update [{_tableName}] 
                               set EPBConfirmStatus = {(int)status},
                                   EPBConfirmUpdateTime = GetDate(),
                                   EPBConfirmDescribe = @description
                               where Id = @id 
                              ";
            ExecuteSQL(updateSql, new { id, description });
        }
    }
}