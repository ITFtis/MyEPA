using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace MyEPA.Repositories
{
    public class UsersRepository : BaseEMISRepository<UsersModel>
    {
        public List<UsersModel> GetListByFilter(UsersFilterParameter usersFilter)
        {
            var whereSQL = GetWhereSQLByFilter(usersFilter);
            return base.GetListByWhereSQL(whereSQL, usersFilter);
        }

        public PagingResult<UsersModel> GetPageingByFilter(UsersFilterPaginationParameter usersFilter)
        {
            var whereSQL = GetWhereSQLByFilter(usersFilter);
            return base.GetPageingEntitiesByWhereSQL(whereSQL, usersFilter.Pagination, usersFilter);
        }

        /// <summary>
        /// 聯絡人未登入
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<UserLoginViewModel> GetUserLoginByFilter(UsersFilterParameter filter)
        {
            string whereSQL = GetWhereSQLByFilter(filter);
            string where2 = "";

            string sql = $@"

Select a.Name, a.UserName, Pwd, VoicePwd, Duty, City, Town, MobilePhone, HumanType, MainContacter, ReportPriority, DepartmentId, PositionId, OfficePhone, FaxNumber, Email, Remark, HomeNumber, UpdateDate, CityId, TownId, DutyId, ConfirmTime, isadmin, ContactManualDuty, ContactManualDepartmentId, ISEnvironmentalProtectionAdministration, ISEnvironmentalProtectionDepartment, ISBook,
	   b.logintime, b.loginrange
From  [Users] a
Left Join (
	Select Max(logintime) AS logintime, 
		   DateDiff(Day, Max(logintime), GetDate()) AS loginrange,
		   UserName
	From UserLoginLog
	--Where UserName = '顏蕉香'
	Group By UserName	
)b On b.UserName COLLATE Chinese_Taiwan_Stroke_CI_AS = a.UserName
    {whereSQL}
";

            sql += @"
Order By loginrange Desc
                ";

            return GetListBySQL<UserLoginViewModel>(sql, filter);
        }

        private string GetWhereSQLByFilter(UsersFilterParameter usersFilter)
        {
            string whereSQL = "Where 1=1";
            if (usersFilter.DepartmentIds.IsNotEmpty())
            {
                whereSQL += " AND DepartmentId IN @DepartmentIds";
            }
            if (usersFilter.ContactManualDepartmentIds.IsNotEmpty())
            {
                whereSQL += " AND ContactManualDepartmentId IN @ContactManualDepartmentIds";
            }
            if (usersFilter.DutyIds.IsNotEmpty())
            {
                whereSQL += " AND DutyId IN @DutyIds";
            }
            if (usersFilter.PositionIds.IsNotEmpty())
            {
                whereSQL += " AND PositionId IN @PositionIds";
            }
            if (usersFilter.TownIds.IsNotEmpty())
            {
                whereSQL += " AND TownId IN @TownIds";
            }
            if (usersFilter.UserIds.IsNotEmpty())
            {
                whereSQL += " AND Id IN @UserIds";
            }
            if (string.IsNullOrWhiteSpace(usersFilter.Name) == false)
            {
                usersFilter.Name = $"%{usersFilter.Name}%";
                whereSQL += " AND Name Like @Name";
            }
            if (usersFilter.ContactManualDutys.IsNotEmpty())
            {
                whereSQL += " AND ContactManualDuty IN @ContactManualDutys";
            }
            if (string.IsNullOrWhiteSpace(usersFilter.HumanType) == false)
            {
                usersFilter.HumanType = $"%{usersFilter.HumanType}%";
                whereSQL += " AND HumanType LIKE @HumanType";
            }
            if (string.IsNullOrWhiteSpace(usersFilter.MainContacter) == false)
            {
                whereSQL += " AND MainContacter = @MainContacter";
            }

            return whereSQL;
        }
        public List<UsersBriefModel> GetListBriefByFilter(UsersBriefFilterParameter filter)
        {
            string where = GetWhereSQLByFilter(filter);
            string sql = $@"
                            SELECT Id,Name,UserName,City,Town,MobilePhone,ContactManualDuty
                            FROM Users
                            {where}";
            return GetListBySQL<UsersBriefModel>(sql, filter);
        }
        public List<UsersModel> GetListByFilter(UsersBriefFilterParameter filter)
        {
            string where = GetWhereSQLByFilter(filter);
            string sql = $@"
                            SELECT *
                            FROM Users
                            {where}";
            return GetListBySQL<UsersModel>(sql, filter);
        }
        public List<UsersModel> GetUsersJoinPositionByFilter(UsersJoinPositionFilterParameter filter)
        {
            string where = GetWhereSQLByFilter(filter);
            string sql = $@"
                            SELECT U.*
                            FROM Users U
                            JOIN Position P ON U.PositionId = P.Id
                            {where}";
            
            return GetListBySQL<UsersModel>(sql, filter);
        }
        public UsersModel GetUserByUserName(string userName)
        {
            var whereSql = @"WHERE UserName = @userName";
            return GetByWhereSQL(whereSql, new { userName });
        }
        public bool IsExistsByMainContacter(int cityId, int townId, int? userId = null)
        {
            var whereSql = @"WHERE MainContacter = '是' 
                                AND CityId = @cityId
                                AND TownId = @townId";
            if(userId.HasValue)
            {
                whereSql += " AND Id != @userId";
            }
            return IsExistsByWhereSQL(whereSql, new { userId, cityId, townId });
        }
        public UsersModel GetUserByUserNameAndPwd(string userName, string pwd)
        {
            var whereSql = @"WHERE UserName = @userName AND 
                                   Pwd = @pwd";
            return GetByWhereSQL(whereSql, new { userName, pwd });
        }

        public UsersModel GetUserByUserNameAndemail(string userName, string email)
        {
            var whereSql = @"WHERE UserName = @userName AND 
                                   Email = @email";
            return GetByWhereSQL(whereSql, new { userName, email });
        }

        public void AddUserLoginLog(UsersModel user)
        {
            var now = DateTime.Now;
            SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());           
            X.Open();
            var G = @"Insert into UserLoginLog(Id,UserName,logintime) 
                                        Values(@Id,@UserName,@logintime)";
            SqlCommand Q = new SqlCommand(G, X);
            Q.Parameters.AddWithValue("@Id", user.Id);
            Q.Parameters.AddWithValue("@UserName", user.UserName);
            Q.Parameters.AddWithValue("@logintime", now);
           

            Q.ExecuteNonQuery();
            X.Close();
        }

        public void AddUserEmail(UsersModel user, string email)
        {          
            SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());
            X.Open();
            var G = @"UPDATE dbo.Users    SET Email = @Email WHERE Id=@Id";
            SqlCommand Q = new SqlCommand(G, X);
            Q.Parameters.AddWithValue("@Id", user.Id);
            Q.Parameters.AddWithValue("@Email", email);
            Q.ExecuteNonQuery();
            X.Close();
        }

        public List<UsersInfoModel> GetUsersInfoByFilter(UsersInfoFilterParameter filter)
        {
            string whereSql = GetWhereSQLByFilter(filter);
            string sql = $@"
SELECT U.*,D.Name DepartmentName,P.Name PositionName
FROM Users U
LEFT JOIN City C ON C.Id = U.CityId
LEFT JOIN Town T ON T.Id = U.TownId
LEFT JOIN Department D ON D.Id = U.DepartmentId
LEFT JOIN Position P ON P.Id = U.PositionId
{whereSql}
";
            return GetListBySQL<UsersInfoModel>(sql, filter);
        }
        private static string GetWhereSQLByFilter(UsersJoinPositionFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";

            if (filter.MinRank.HasValue)
            {
                whereSQL += " AND P.Rank > @MinRank";
            }

            if (filter.MaxRank.HasValue)
            {
                whereSQL += " AND P.Rank < @MaxRank";
            }

            if (filter.MinRankContain.HasValue)
            {
                whereSQL += " AND P.Rank >= @MinRankContain";
            }

            if (filter.MaxRankContain.HasValue)
            {
                whereSQL += " AND P.Rank <= @MaxRankContain";
            }

            if (filter.DepartmentIds.IsNotEmpty())
            {
                whereSQL += " AND U.DepartmentId IN @DepartmentIds";
            }
            return whereSQL;
        }
        private static string GetWhereSQLByFilter(UsersBriefFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";
            if (filter.UserIds.IsNotEmpty())
            {
                whereSQL += " AND Id IN @UserIds";
            }
            if (filter.DepartmentIds.IsNotEmpty())
            {
                whereSQL += " AND DepartmentId IN @DepartmentIds";
            }
            if (filter.CityIds.IsNotEmpty())
            {
                whereSQL += " AND CityId IN @CityIds";
            }
            if (filter.DutyIds.IsNotEmpty())
            {
                whereSQL += " AND DutyId IN @DutyIds";
            }
            if (filter.TownIds.IsNotEmpty())
            {
                whereSQL += " AND TownId IN @TownIds";
            }
            if (string.IsNullOrWhiteSpace(filter.MainContacter) == false)
            {
                whereSQL += " AND MainContacter = @MainContacter";
            }
            if (string.IsNullOrWhiteSpace(filter.HumanType) == false)
            {
                whereSQL += " AND HumanType = @HumanType";
            }
            if (string.IsNullOrWhiteSpace(filter.Duty) == false)
            {
                whereSQL += " AND Duty = @Duty";
            }
            if (filter.ContactManualDutys.IsNotEmpty())
            {
                whereSQL += " AND ContactManualDuty IN @ContactManualDutys";
            }
            if (filter.ISEnvironmentalProtectionAdministration.IsNotEmpty())
            {
                whereSQL += " AND ISEnvironmentalProtectionAdministration = @ISEnvironmentalProtectionAdministration";
            }
            if (filter.ISEnvironmentalProtectionDepartment.IsNotEmpty())
            {
                whereSQL += " AND ISEnvironmentalProtectionDepartment = @ISEnvironmentalProtectionDepartment";
            }
            if (filter.ISBook.IsNotEmpty())
            {
                whereSQL += " AND ISBook = @ISBook";
            }
            return whereSQL;
        }
        private static string GetWhereSQLByFilter(UsersInfoFilterParameter filter)
        {
            string whereSQL = "WHERE 1=1";
            if (filter.UserIds.IsNotEmpty())
            {
                whereSQL += " AND U.Id IN @UserIds";
            }
            if (filter.UserNames.IsNotEmpty())
            {
                whereSQL += " AND U.UserName IN @UserNames";
            }
            if (filter.DepartmentIds.IsNotEmpty())
            {
                whereSQL += " AND U.DepartmentId IN @DepartmentIds";
            }
            if (filter.CityIds.IsNotEmpty())
            {
                whereSQL += " AND C.Id IN @CityIds";
            }
            if (filter.TownIds.IsNotEmpty())
            {
                whereSQL += " AND T.Id IN @TownIds";
            }
            if (filter.DutyIds.IsNotEmpty())
            {
                whereSQL += " AND U.DutyId IN @DutyIds";
            }

            return whereSQL;
        }

        public UsersModel GetByUserName(string userName)
        {
            var whereSql = "where userName = @userName";
            return GetByWhereSQL(whereSql, new { userName });
        }

    }
}