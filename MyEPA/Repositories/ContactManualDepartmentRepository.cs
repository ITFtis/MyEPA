using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class ContactManualDepartmentRepository :  BaseEMISBaseModelRepository<ContactManualDepartmentModel>
    {
        public bool IsExistsByFilter(ContactManualDepartmentParameter filter)
        {
            string whereSql = GetWhereSQLByFilter(filter);

            return IsExistsByWhereSQL(whereSql, filter);
        }
        public ContactManualDepartmentModel GetByFilter(ContactManualDepartmentParameter filter)
        {
            string whereSql = GetWhereSQLByFilter(filter);

            return GetByWhereSQL(whereSql, filter);
        }
        public List<ContactManualDepartmentModel> GetListByFilter(ContactManualDepartmentParameter filter)
        {
            string whereSql = GetWhereSQLByFilter(filter);

            return GetListByWhereSQL(whereSql, filter);
        }

        public List<ContactManualDepartmentModel> GetByType(ContactManualDepartmentTypeEnum type)
        {
            string whereSql = "Where Type = @type";
            return GetListByWhereSQL(whereSql, new { type });
        }

        private string GetWhereSQLByFilter(ContactManualDepartmentParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (filter.Ids.IsNotEmpty())
            {
                whereSQL += " AND Id IN @Ids";
            }

            if (filter.Types.IsNotEmpty())
            {
                whereSQL += " AND Type = @Types";
            }

            if (string.IsNullOrWhiteSpace(filter.Name) == false)
            {
                whereSQL += " AND Name = @Name";
            }

            return whereSQL;
        }
    }
}