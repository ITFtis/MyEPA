using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Repositories
{
    public class UserLoginLogRepository : BaseEMISRepository<UserLoginLogModel>
    {
        public List<UserLoginLogModel> GetListByFilter(UserLoginLogFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter);
        }

        public UserLoginLogModel GetByFilter(UserLoginLogFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter).FirstOrDefault();
        }

        private static string GetWhereSQLByFilter(UserLoginLogFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (string.IsNullOrWhiteSpace(filter.UserName) == false)
            {
                whereSQL += " And UserName = @UserName";
            }

            return whereSQL;
        }
    }
}