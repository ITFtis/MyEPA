using DocumentFormat.OpenXml.Office2010.Excel;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories.BaseRepositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MyEPA.Repositories
{
    public class UserLoginLogRepository : BaseEMISRepository<UserLoginLogModel>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

            if (filter.Type.HasValue)
            {
                whereSQL += " And Type = @Type";
            }

            if (filter.IsOver.HasValue)
            {
                whereSQL += " AND IsOver = @IsOver";
            }

            return whereSQL;
        }

        /// <summary>
        /// 清除密碼輸入錯誤的Log
        /// </summary>
        /// <param name="UserName">帳號</param>
        /// <param name="lockTime">15(分)</param>
        /// <returns></returns>
        public bool UpdateIsOver(string userName, int lockTime = 0)
        {
            bool result = false;

            SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());

            try
            {
                X.Open();
                string G = @"
Update UserLoginLog
Set IsOver = 0
Where (Type = 2 And IsOver = 1)
And UserName = @UserName
";

                if (lockTime != 0)
                {
                    G += @"
                            And  DateDiff(Minute, logintime, GetDate()) > @lockTime
                        ";
                }

                SqlCommand Q = new SqlCommand(G, X);                
                Q.Parameters.AddWithValue("@UserName", userName);
                Q.Parameters.AddWithValue("@lockTime", lockTime);
                Q.ExecuteNonQuery();

                result = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
            finally
            {
                X.Close();
            }

            return result;
        }
    }
}