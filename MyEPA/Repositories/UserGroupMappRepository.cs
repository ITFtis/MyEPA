using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System.Collections.Generic;

namespace MyEPA.Repositories
{
    public class UserGroupMappRepository : BaseEMISRepository<UserGroupMappModel>
    {
        public UserGroupMappModel Get(int userId,int groupId)
        {
            string whereSQL = "where userId=@userId AND groupId=@groupId";
            return GetByWhereSQL(whereSQL, new { userId, groupId });
        }
        public List<UserGroupMappBriefModel> GetBriefAll()
        {
            string sql = @"
SELECT [GroupId]
      ,[UserId]
  FROM [UserGroupMapp]";
            return GetListBySQL<UserGroupMappBriefModel>(sql);
        }
        public List<UserGroupMappModel> GetBriefListByFilter(UserGroupMappFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            string sql = $@"
SELECT [GroupId]
      ,[UserId]
FROM [UserGroupMapp]
{whereSQL}";
            return GetListBySQL<UserGroupMappModel>(sql, filter);
        }
        public List<UserGroupMappModel> GetListByFilter(UserGroupMappFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            return GetListByWhereSQL(whereSQL, filter);
        }

        private static string GetWhereSQLByFilter(UserGroupMappFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (filter.GroupIds.IsNotEmpty())
            {
                whereSQL += " And GroupId IN @GroupIds";
            }
            if (filter.UserIds.IsNotEmpty())
            {
                whereSQL += " And UserId IN @UserIds";
            }

            return whereSQL;
        }
    }
}