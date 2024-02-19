using MyEPA.Models;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class PhoneWorkRepository : BaseEMISRepository<PhoneWorkModel>
    {
        public List<PhoneWorkModel> GetByDiasterId(int diasterId)
        {
            string whereSql = "Where DiasterId = @DiasterId";
            return GetListByWhereSQL(whereSql, new { DiasterId = diasterId });
        }
    }
}