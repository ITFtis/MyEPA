using MyEPA.Models;
using MyEPA.Repositories.BaseRepositories;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class SendTextLogDetailRepository : BaseEMISRepository<SendTextLogDetailModel>
    {
        public List<SendTextLogDetailModel> GetBySendTextLogId(int sendTextLogId)
        {
            string sql = @"
Select *
From SendTextLogDetail
WHERE SendTextLogId = @sendTextLogId";
            return GetListBySQL<SendTextLogDetailModel>(sql, new { sendTextLogId });
        }
    }
}