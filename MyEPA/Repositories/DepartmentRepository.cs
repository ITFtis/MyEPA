using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Repositories
{
    public class DepartmentRepository : BaseEMISRepository<DepartmentModel>
    {
        public List<DepartmentModel> GetByFilter(DepartmentParameter filter)
        {
            string whereSql = GetWhereSQLByFilter(filter);

            return GetListByWhereSQL(whereSql, filter);
        }
        private string GetWhereSQLByFilter(DepartmentParameter filter)
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