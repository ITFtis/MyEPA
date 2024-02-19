using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class ContactManualRoleRepository : BaseEMISBaseModelRepository<ContactManualRoleModel>
    {
        public List<ContactManualRoleModel> GetByFilter(ContactManualRoleFilterParameter filter)
        {
            string whereSql = GetWhereSQLByFilter(filter);

            return GetListByWhereSQL(whereSql, filter);
        }
        private string GetWhereSQLByFilter(ContactManualRoleFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (filter.Ids.IsNotEmpty())
            {
                whereSQL += " AND Id IN @Ids";
            }

            if (string.IsNullOrWhiteSpace(filter.Name) == false)
            {
                whereSQL += " AND Name = @Name";
            }

            return whereSQL;
        }
    }
}