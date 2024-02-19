using MyEPA.Models;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Repositories
{
    public class OpenContractDetailRepository : BaseEMISRepository<OpenContractDetailModel>
    {
        public List<OpenContractDetailModel> GetListByOpenContractId(int openContractId)
        {
            string whereSql = "Where OpenContractId = @OpenContractId";
            return GetListByWhereSQL(whereSql, new { OpenContractId = openContractId });
        }

        public void DeleteByOpenContractId(int id)
        {
            string sql = @"DELETE
FROM OpenContractDetail
WHERE OpenContractId = @id";

            ExecuteSQL(sql, new { id });
        }
    }
}