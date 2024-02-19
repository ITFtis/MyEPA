using MyEPA.Models;
using MyEPA.Repositories.BaseRepositories;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class SendTextLogRepository : BaseEMISRepository<SendTextLogModel>
    {
        public List<SendTextLogModel> GetByTop(int top)
        {
            string sql = @"
Select TOP (@top) *
From SendTextLog
Order By CreateDate Desc";
            return GetListBySQL<SendTextLogModel>(sql,new { top });
        }
    }
}