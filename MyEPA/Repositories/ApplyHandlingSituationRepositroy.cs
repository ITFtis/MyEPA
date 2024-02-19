using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Repositories.BaseRepositories;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class ApplyHandlingSituationRepositroy : BaseEMISRepository<ApplyHandlingSituationModel>
    {
        public void DeleteByApply(ApplyTypeEnum applyType, int applyId)
        {
            string whereSql = "where ApplyType = @applyType and ApplyId = @applyId";
            DeleteByWhereSQL(whereSql, new { applyType = applyType.ToInteger(), applyId });
        }

        public List<ApplyHandlingSituationModel> GetByApply(ApplyTypeEnum applyType, int applyId)
        {
            string whereSql = "where ApplyType = @applyType and ApplyId = @applyId";
            return GetListByWhereSQL(whereSql, new { applyType = applyType.ToInteger(), applyId });
        }
    }
}