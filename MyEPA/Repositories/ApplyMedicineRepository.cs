using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using MyEPA.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Repositories
{
    public class ApplyMedicineRepository : ApplyBaseRepositroy<ApplyMedicineModel>
    {
        public new int Create(ApplyMedicineModel model)
        {
            if (model == null)
            {
                throw new Exception($"{nameof(model)} 不得為空");
            }

            var sql = SQLUtility.GetInsertCommand<ApplyMedicineModel>("ApplyMedicine", new List<string>() { nameof(model.Id), nameof(model.Details) });
            sql = SQLUtility.ConcatReturnIdentityCommand(sql);
            model.Id = ExecuteSQL<int>(sql, model);

            CreateDetails(model);

            return model.Id;
        }

        public void DeleteDetails(int applyMedicineId) 
        {
            var sql = @"Delete ApplyMedicineDetail
                    Where ApplyMedicineId = @applyMedicineId";
            ExecuteSQL(sql, new { applyMedicineId });
        }

        private void CreateDetails(ApplyMedicineModel model)
        {
            if (model.Id > 0 && model.Details.Any(c => c.Quantity > 0))
            {
                var details = model.Details
                                   .Where(c => c.Quantity > 0)
                                   .GroupBy(c => c.MedicineType)
                                   .Select(c => new ApplyMedicineDetailModel()
                                   {
                                       ApplyMedicineId = model.Id,
                                       MedicineType = c.Key,
                                       Quantity = c.Sum(k => k.Quantity)
                                   });

                var sql = SQLUtility.GetInsertCommand<ApplyMedicineDetailModel>("ApplyMedicineDetail", new List<string>() { nameof(ApplyMedicineDetailModel.Id) });
                ExecuteSQL(sql, details);
            }
        }

        public List<ApplyMedicineDetailModel> GetDetailsById(int id) 
        {
            string whereSql = "where ApplyMedicineId = @ApplyMedicineId";
            string querySQL = $@"SELECT * 
                                 FROM [ApplyMedicineDetail] ap WITH(NOLOCK)
                                 {whereSql}";

            return GetListBySQL<ApplyMedicineDetailModel>(querySQL, new { ApplyMedicineId = id });
        }

        public List<ApplyMedicineDetailModel> GetDetailsByIds(List<int> ids )
        {
            string whereSql = "where ApplyMedicineId in @ApplyMedicineId";
            string querySQL = $@"SELECT * 
                                 FROM [ApplyMedicineDetail] ap WITH(NOLOCK)
                                 {whereSql}";

            return GetListBySQL<ApplyMedicineDetailModel>(querySQL, new { ApplyMedicineId = ids });
        }

        public void Edit(ApplyMedicineModel model)
        {
            if (model == null) 
            {
                return;
            }
             //主 model update
             var editSql = @"UPDATE [dbo].[ApplyMedicine]
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