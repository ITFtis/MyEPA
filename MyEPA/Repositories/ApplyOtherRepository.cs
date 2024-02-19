using MyEPA.Models;
using MyEPA.Repositories.BaseRepositories;
using MyEPA.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Repositories
{
    public class ApplyOtherRepository : ApplyBaseRepositroy<ApplyOtherModel>
    {
        public int Create(ApplyOtherModel model)
        {
            if (model == null)
            {
                throw new Exception($"{nameof(model)} 不得為空");
            }

            var sql = SQLUtility.GetInsertCommand<ApplyOtherModel>("ApplyOther", new List<string>() { nameof(model.Id), nameof(model.Details) });
            sql = SQLUtility.ConcatReturnIdentityCommand(sql);
            model.Id = ExecuteSQL<int>(sql, model);

            CreateDetails(model);

            return model.Id;
        }

        public void DeleteDetails(int ApplyOtherId) 
        {
            var sql = @"Delete ApplyOtherDetail
                    Where ApplyOtherId = @ApplyOtherId";
            ExecuteSQL(sql, new { ApplyOtherId });
        }

        private void CreateDetails(ApplyOtherModel model)
        {
            if (model.Id > 0 && model.Details.Any(c => c.Quantity > 0))
            {
                var details = model.Details
                                   .Where(c => c.Quantity > 0);

                foreach (var detail in details) 
                {
                    detail.ApplyOtherId = model.Id;
                }

                var sql = SQLUtility.GetInsertCommand<ApplyOtherDetailModel>("ApplyOtherDetail", new List<string>() { nameof(ApplyOtherDetailModel.Id) });
                ExecuteSQL(sql, details);
            }
        }

        public List<ApplyOtherDetailModel> GetDetailsById(int id) 
        {
            string whereSql = "where ApplyOtherId = @ApplyOtherId";
            string querySQL = $@"SELECT * 
                                 FROM [ApplyOtherDetail] ao WITH(NOLOCK)
                                 {whereSql}";

            return GetListBySQL<ApplyOtherDetailModel>(querySQL, new { ApplyOtherId = id });
        }

        public List<ApplyOtherDetailModel> GetDetailsByIds(List<int> ids)
        {
            string whereSql = "where ApplyOtherId in @ApplyOtherId";
            string querySQL = $@"SELECT * 
                                 FROM [ApplyOtherDetail] ao WITH(NOLOCK)
                                 {whereSql}";

            return GetListBySQL<ApplyOtherDetailModel>(querySQL, new { ApplyOtherId = ids });
        }

        public void Edit(ApplyOtherModel model)
        {
            if (model == null) 
            {
                return;
            }
             //主 model update
             var editSql = @"UPDATE [dbo].[ApplyOther]
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
    }
}