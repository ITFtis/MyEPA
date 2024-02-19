using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Models.BaseModels;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using MyEPA.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Repositories
{
    public class ApplySubsidyRepository : ApplyBaseRepositroy<ApplySubsidyModel>
    {

        public new List<ApplyBaseStatusCountModel> GetStatusCountByFilter(ApplyBaseFilterParameter filter)
        {
            string whereSql = GetWhereSQLByFilter(filter);
            string querySQL = $@"
SELECT 
	CityId,TownId,EPAConfirmStatus,EPBConfirmStatus,IsToEpa,SUM(asd.Amount) Count
FROM ApplySubsidy as [ap] WITH(NOLOCK)
JOIN 
	(
	SELECT ApplySubsidyId,
		CASE WHEN
			ApplySubsidyTypeId = 1
			or ApplySubsidyTypeId = 2
			or ApplySubsidyTypeId = 3
		THEN 
			Quantity * Price * NeedDays
		ELSE
			Quantity * Price
		END [Amount]
	FROM ApplySubsidyDetail 
	) asd  ON [ap].Id = asd.ApplySubsidyId
{whereSql}
GROUP BY CityId,TownId,EPAConfirmStatus,EPBConfirmStatus,IsToEpa";
            return GetListBySQL<ApplyBaseStatusCountModel>(querySQL, filter);
        }
        public List<ApplySubsidySumPriceModel> GetSumPriceByDiasterId(int diasterId)
        {
            string sql = $@"
SELECT [as].CityId,[as].TownId,SUM(asd.Quantity * asd.Price * asd.NeedDays) SumPrice
FROM ApplySubsidy [as]
JOIN ApplySubsidyDetail asd ON [as].Id = asd.ApplySubsidyId
WHERE [as].DiasterId = @DiasterId AND [as].EPAConfirmStatus = {ApplyStatusEnum.Confrim.ToInteger()}
GROUP BY [as].CityId,[as].TownId
";
            return GetListBySQL<ApplySubsidySumPriceModel>(sql, new { DiasterId = diasterId });
        }
        public int Create(ApplySubsidyModel model)
        {
            if (model == null)
            {
                throw new Exception($"{nameof(model)} 不得為空");
            }

            var sql = SQLUtility.GetInsertCommand<ApplySubsidyModel>("ApplySubsidy", new List<string>() { nameof(model.Id), nameof(model.Details) });
            sql = SQLUtility.ConcatReturnIdentityCommand(sql);
            model.Id = ExecuteSQL<int>(sql, model);

            CreateDetails(model);

            return model.Id;
        }

        public void DeleteDetails(int ApplySubsidyId) 
        {
            var sql = @"Delete ApplySubsidyDetail
                    Where ApplySubsidyId = @ApplySubsidyId";
            ExecuteSQL(sql, new { ApplySubsidyId });
        }

        private void CreateDetails(ApplySubsidyModel model)
        {
            //進行資料標準化
            model.Details.ForEach((detail) =>
            {
                //塞入 ref 用的 Id
                detail.ApplySubsidyId = model.Id;

                //type == 其他 強制 quantity 是 1
                if (detail.SubsidyType == Enums.ApplySubsidyTypeEnum.Other) 
                {
                    detail.Quantity = 1;
                }
            });

            if (model.Id > 0 && model.Details.Any(c => c.Quantity > 0))
            {
                //只篩出有數量的部分
                var details = model.Details
                                   .Where(c => c.Quantity > 0)
                                   .ToList();
                var sql = SQLUtility.GetInsertCommand<ApplySubsidyDetailModel>("ApplySubsidyDetail", new List<string>() { nameof(ApplySubsidyDetailModel.Id), nameof(ApplySubsidyDetailModel.SubsidyType) });
                ExecuteSQL(sql, details);
            }
        }

        public List<ApplySubsidyDetailModel> GetDetailsById(int id) 
        {
            string whereSql = "where ApplySubsidyId = @ApplySubsidyId";
            string querySQL = $@"SELECT * 
                                 FROM [ApplySubsidyDetail] ap WITH(NOLOCK)
                                 {whereSql}";

            return GetListBySQL<ApplySubsidyDetailModel>(querySQL, new { ApplySubsidyId = id });
        }

        public List<ApplySubsidyDetailModel> GetDetailsByIds(List<int> ids)
        {
            string whereSql = "where ApplySubsidyId in @ApplySubsidyId";
            string querySQL = $@"SELECT * 
                                 FROM [ApplySubsidyDetail] ap WITH(NOLOCK)
                                 {whereSql}";

            return GetListBySQL<ApplySubsidyDetailModel>(querySQL, new { ApplySubsidyId = ids });
        }

        public void Edit(ApplySubsidyModel model)
        {
            if (model == null) 
            {
                return;
            }
             //主 model update
             var editSql = @"UPDATE [dbo].[ApplySubsidy]
                            SET [RequireDate] = @RequireDate
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

            //更新 Detail
            DeleteDetails(model.Id);
            CreateDetails(model);
        }

        public List<ApplySubsidyTypeDetailModel> GetTypeDetails()
        {
            var sql = @"Select * from ApplySubsidyTypeDetail WITH(NOLOCK)";
            return GetListBySQL<ApplySubsidyTypeDetailModel>(sql);
        }

        public List<ApplySubsidyTypeDetailModel> GetTypeDetails(int typeId)
        {
            string whereSql = "where ApplySubsidyTypeId = @ApplySubsidyTypeId";
            string querySQL = $@"SELECT * 
                                 FROM [ApplySubsidyTypeDetail] ap WITH(NOLOCK)
                                 {whereSql}";

            return GetListBySQL<ApplySubsidyTypeDetailModel>(querySQL, new { ApplySubsidyTypeId = typeId });
            throw new NotImplementedException();
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
                        from [dbo].[ApplySubsidy] as ap
                        join city as ci on  ap.CityId = ci.Id
                        join town as tn on  ap.townId = tn.Id
                        join users as u on ap.CreateUser = u.userName
                    ";

            return GetApplySupportReportDetais(sql, filter);
        }
    }
}