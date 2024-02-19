using MyEPA.Models;
using MyEPA.Repositories.BaseRepositories;
using MyEPA.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Repositories
{
    public class ApplyCarRepository : ApplyBaseRepositroy<ApplyCarModel>
    {
        public int Create(ApplyCarModel model)
        {
            if (model == null)
            {
                throw new Exception($"{nameof(model)} 不得為空");
            }

            var sql = SQLUtility.GetInsertCommand<ApplyCarModel>("ApplyCar", new List<string>() { nameof(model.Id), nameof(model.Details) });
            sql = SQLUtility.ConcatReturnIdentityCommand(sql);
            model.Id = ExecuteSQL<int>(sql, model);

            CreateDetails(model);

            return model.Id;
        }

        public void DeleteDetails(int applyCarId) 
        {
            var sql = @"Delete ApplyCarDetail
                        Where ApplyCarId = @ApplyCarId";
            ExecuteSQL(sql, new { applyCarId });
        }

        private void CreateDetails(ApplyCarModel model)
        {
            if (model.Id > 0 && model.Details.Any(c => c.Quantity > 0))
            {
                foreach (var detail in model.Details) 
                {
                    detail.ApplyCarId = model.Id;
                }

                var sql = SQLUtility.GetInsertCommand<ApplyCarDetailModel>("ApplyCarDetail", new List<string>() { nameof(ApplyCarDetailModel.Id) });
                ExecuteSQL(sql, model.Details);
            }
        }

        public List<ApplyCarDetailModel> GetDetailsById(int id) 
        {
            string whereSql = "where ApplyCarId = @ApplyCarId";
            string querySQL = $@"SELECT * 
                                 FROM [ApplyCarDetail] ap WITH(NOLOCK)
                                 {whereSql}";

            return GetListBySQL<ApplyCarDetailModel>(querySQL, new { ApplyCarId = id });
        }

        public List<ApplyCarDetailModel> GetDetailsByIds(List<int> ids)
        {
            string whereSql = "where ApplyCarId in @ApplyCarId";
            string querySQL = $@"SELECT * 
                                 FROM [ApplyCarDetail] ap WITH(NOLOCK)
                                 {whereSql}";

            return GetListBySQL<ApplyCarDetailModel>(querySQL, new { ApplyCarId = ids });
        }

        public void Edit(ApplyCarModel model)
        {
            if (model == null) 
            {
                return;
            }
             //主 model update
             var editSql = @"UPDATE [dbo].[ApplyCar]
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

        public List<ApplyCarTypeModel> GetCarTypes()
        {
            var sql = "Select * from ApplyCarType";
            return GetListBySQL<ApplyCarTypeModel>(sql);
        }
    }
}