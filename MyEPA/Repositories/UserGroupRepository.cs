using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Repositories
{
    public class UserGroupRepository : BaseEMISRepository<UserGroupModel>
    {
        public List<UserGroupModel> GetListByFilter(UserGroupFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter);
        }

        public UserGroupModel GetByFilter(UserGroupFilterParameter filter)
        {
            return GetListByFilter(filter).FirstOrDefault();
        }

        private static string GetWhereSQLByFilter(UserGroupFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (filter.Types.IsNotEmpty())
            {
                whereSQL += " And Type IN @Types";
            }

            return whereSQL;
        }
    }
}