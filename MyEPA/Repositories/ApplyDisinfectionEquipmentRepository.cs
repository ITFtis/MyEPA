using MyEPA.Models;
using MyEPA.Repositories.BaseRepositories;
using MyEPA.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Repositories
{
    public class ApplyDisinfectionEquipmentRepository : ApplyBaseRepositroy<ApplyDisinfectionEquipmentModel>
    {
        public int Create(ApplyDisinfectionEquipmentModel model)
        {
            if (model == null)
            {
                throw new Exception($"{nameof(model)} 不得為空");
            }

            var sql = SQLUtility.GetInsertCommand<ApplyDisinfectionEquipmentModel>("ApplyDisinfectionEquipment", new List<string>() { nameof(model.Id), nameof(model.Details) });
            sql = SQLUtility.ConcatReturnIdentityCommand(sql);
            model.Id = ExecuteSQL<int>(sql, model);

            CreateDetails(model);

            return model.Id;
        }

        public void DeleteDetails(int ApplyDisinfectionEquipmentId) 
        {
            var sql = @"Delete ApplyDisinfectionEquipmentDetail
                    Where ApplyDisinfectionEquipmentId = @ApplyDisinfectionEquipmentId";
            ExecuteSQL(sql, new { ApplyDisinfectionEquipmentId });
        }


        private void CreateDetails(ApplyDisinfectionEquipmentModel model)
        {
            if (model.Id > 0 && model.Details.Any(c => c.Quantity > 0 && c.Days > 0))
            {
                var details = model.Details
                                   .Where(c => c.Quantity > 0 && c.Days > 0);

                foreach (var detail in details) 
                {
                    detail.ApplyDisinfectionEquipmentId = model.Id;
                }

                var sql = SQLUtility.GetInsertCommand<ApplyDisinfectionEquipmentDetailModel>("ApplyDisinfectionEquipmentDetail", new List<string>() { nameof(ApplyDisinfectionEquipmentDetailModel.Id) });
                ExecuteSQL(sql, details);
            }
        }

        public List<ApplyDisinfectionEquipmentDetailModel> GetDetailsById(int id) 
        {
            string whereSql = "where ApplyDisinfectionEquipmentId = @ApplyDisinfectionEquipmentId";
            string querySQL = $@"SELECT * 
                                 FROM [ApplyDisinfectionEquipmentDetail] ao WITH(NOLOCK)
                                 {whereSql}";

            return GetListBySQL<ApplyDisinfectionEquipmentDetailModel>(querySQL, new { ApplyDisinfectionEquipmentId = id });
        }

        public List<ApplyDisinfectionEquipmentDetailModel> GetDetailsByIds(List<int> ids)
        {
            string whereSql = "where ApplyDisinfectionEquipmentId in @ApplyDisinfectionEquipmentId";
            string querySQL = $@"SELECT * 
                                 FROM [ApplyDisinfectionEquipmentDetail] ao WITH(NOLOCK)
                                 {whereSql}";

            return GetListBySQL<ApplyDisinfectionEquipmentDetailModel>(querySQL, new { ApplyDisinfectionEquipmentId = ids });
        }


        public void Edit(ApplyDisinfectionEquipmentModel model)
        {
            if (model == null) 
            {
                return;
            }
             //主 model update
             var editSql = @"UPDATE [dbo].[ApplyDisinfectionEquipment]
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