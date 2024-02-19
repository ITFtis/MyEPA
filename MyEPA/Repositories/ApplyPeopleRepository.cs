using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.BaseModels;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class ApplyPeopleRepository : ApplyBaseRepositroy<ApplyPeopleModel>
    {
        public List<ApplyBaseStatusCountModel> GetStatusCountByFilter(ApplyBaseFilterParameter filter)
        {
            string whereSql = GetWhereSQLByFilter(filter);
            string querySQL = $@"
SELECT 
    CityId  
    ,TownId
    ,EPAConfirmStatus
    ,EPBConfirmStatus
    ,IsToEpa
    ,SUM( CleaningMemberQuantity * CleaningMemberDays + NationalArmyQuantity * NationalArmyDays ) Count
FROM [ApplyPeople] as [ap] WITH(NOLOCK)
{whereSql}
GROUP BY CityId,TownId,EPAConfirmStatus,EPBConfirmStatus,IsToEpa";
            return GetListBySQL<ApplyBaseStatusCountModel>(querySQL, filter);
        }
        public void Edit(ApplyPeopleModel model)
        {
            var editSql = @"UPDATE [dbo].[ApplyPeople]
                            SET [RequireDate] = @RequireDate
                               ,[CleaningMemberQuantity] = @CleaningMemberQuantity
                               ,[CleaningMemberDays] = @CleaningMemberDays
                               ,[NationalArmyQuantity] = @NationalArmyQuantity
                               ,[NationalArmyDays] = @NationalArmyDays
                               ,[EstimationMethodDescribe] = @EstimationMethodDescribe
                               ,[PhotoDescribe] = @PhotoDescribe
                               ,[ContactPerson] = @ContactPerson
                               ,[ContactPhone] = @ContactPhone
                               ,[ContactMobilePhone] = @ContactMobilePhone
                               ,[UpdateDate] = @UpdateDate
                               ,[UpdateUser] = @UpdateUser
                               WHERE Id = @Id
                           ";
            ExecuteSQL(editSql, model);
        }


        public List<ApplySupportReportDetailModel> GetApplySupportReportDetais(ApplyBaseFilterParameter filter) 
        {
            var sql = @"
                        select 
                        ap.Id,
                        ap.DiasterId,
                        ap.CityId,
                        ci.city CityName, 
                        ap.TownId,
                        tn.Name TownName, 
                        ap.CreateDate,
                        ap.RequireDate,
                        (ap.NationalArmyQuantity * NationalArmyDays) + (CleaningMemberQuantity*CleaningMemberDays)  Quantity,
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
                        ap.Status
                        from applypeople as ap
                        join city as ci on  ap.CityId = ci.Id
                        join town as tn on  ap.townId = tn.Id
                        join users as u on ap.CreateUser = u.userName
                    ";

            return GetApplySupportReportDetais(sql, filter);
        }
    }
}