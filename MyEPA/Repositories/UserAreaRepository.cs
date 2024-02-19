using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class UserAreaRepository : BaseEMISRepository<UserAreaModel>
    {
        public UserAreaModel Get(int userId)
        {
            string whereSql = "WHERE UserId = @userId";
            return GetByWhereSQL(whereSql, new { userId });
        }

        public bool IsExistsByUserId(int userId)
        {
            string whereSql = "WHERE UserId = @userId";
            return IsExistsByWhereSQL(whereSql, new { userId });
        }

        public bool Delete(int userId)
        {
            string whereSql = "WHERE UserId = @userId";
            return DeleteByWhereSQL(whereSql, new { userId });
        }
    }
}