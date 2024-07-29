using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Web.Configuration;
using System.ComponentModel.DataAnnotations.Schema;
using MyEPA.Extensions;

namespace MyEPA.Models
{
    [Table("Users")]
    public class UserModel
    {
        public SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());
        [AutoKey]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Pwd { get; set; }
        public string VoicePwd { get; set; }
        public string Duty { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string MobilePhone { get; set; }

        public string HumanType { get; set; }
        public string MainContacter { get; set; }
        public string ReportPriority { get; set; }
        public int DutyId { get; set; }
        public int? CityId { get; set; }
        public int? TownId { get; set; }
        public int? PositionId { get; set; }
        public String Update(string UserName, string Name, string Pwd, string VoicePwd, string Duty, string City, string Town, string MobilePhone, string HumanType, string MainContacter, string ReportPriority,int PositionId)
        {
            try
            {
                X.Open();
               string G = "Update Users Set Name = @Name, Pwd=@Pwd, PwdUpdateDate = @PwdUpdateDate, VoicePwd=@VoicePwd, Duty=@Duty, City=@City,Town=@Town,MobilePhone=@MobilePhone, HumanType=@HumanType, MainContacter=@MainContacter, ReportPriority=@ReportPriority,PositionId=@PositionId where UserName=@UserName";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@UserName", UserName);
                Q.Parameters.AddWithValue("@Name", Name);
                Q.Parameters.AddWithValue("@Pwd", Pwd);
                Q.Parameters.AddWithValue("@PwdUpdateDate", DateTime.Now.AddDays(90));
                Q.Parameters.AddWithValue("@VoicePwd", VoicePwd);
                Q.Parameters.AddWithValue("@Duty", Duty);
                Q.Parameters.AddWithValue("@City", City);
                Q.Parameters.AddWithValue("@Town", Town);
                Q.Parameters.AddWithValue("@MobilePhone", MobilePhone);
                Q.Parameters.AddWithValue("@HumanType", HumanType);
                Q.Parameters.AddWithValue("@MainContacter", MainContacter);
                Q.Parameters.AddWithValue("@ReportPriority", ReportPriority);
                Q.Parameters.AddWithValue("@PositionId", PositionId);
                
                Q.ExecuteNonQuery();
                //在表格NewestUpdateTime 登入聯絡人(users)資訊的最新更新時間
                //User是SQL關鍵字，所以改為Users 

                if ((Duty == "環保局") || (Duty == "清潔隊"))
                {
                    StatisticsModel Statistics = new StatisticsModel();
                    string Msg2 = Statistics.StoreNewestUpdateTime("Users", City);
                    System.Diagnostics.Debug.WriteLine(Msg2);
                }

                X.Close(); return ("用戶資料已更新");
            }
            catch (Exception) { X.Close(); return ("抱歉，用戶資料未更新"); }

        }

        public string Add(string userName, string name, string pwd, string voicePwd, string mobilePhone, string duty, string city,string town, string humanType,string mainContacter,string reportPriority, int? cityId, int? townId, int? dutyId, int departmentId = 0, int positionId = 0)
        {
            try{
                X.Open();
                string G = @"Insert into Users(UserName, Name, Pwd, VoicePwd, MobilePhone, Duty, City, Town, HumanType, MainContacter, ReportPriority, CityId, TownId, DutyId, DepartmentId, PositionId) 
                             Values(@UserName,@Name,@Pwd,@VoicePwd,@MobilePhone,@Duty,@City,@Town,@HumanType, @MainContacter,@ReportPriority,@CityId,@TownId,@DutyId,@DepartmentId,@PositionId)";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@UserName", userName);
                Q.Parameters.AddWithValue("@Name", name);
                Q.Parameters.AddWithValue("@Pwd", pwd);
                Q.Parameters.AddWithValue("@VoicePwd", voicePwd);
                Q.Parameters.AddWithValue("@mobilePhone", mobilePhone);
                Q.Parameters.AddWithValue("@Duty", duty);
                Q.Parameters.AddWithValue("@City", city);
                Q.Parameters.AddWithValue("@Town", town);
                Q.Parameters.AddWithValue("@HumanType", humanType);
                Q.Parameters.AddWithValue("@MainContacter", mainContacter);
                Q.Parameters.AddWithValue("@ReportPriority", reportPriority);
                Q.Parameters.AddWithValue("@CityId", cityId);
                Q.Parameters.AddWithValue("@TownId", townId);
                Q.Parameters.AddWithValue("@DutyId", dutyId);
                Q.Parameters.AddWithValue("@DepartmentId", departmentId);
                Q.Parameters.AddWithValue("@PositionId", positionId);
                Q.ExecuteNonQuery();
                X.Close();

                //在表格NewestUpdateTime 登入聯絡人(users)資訊的最新更新時間
                //User是SQL關鍵字，所以改為Users 

                if ((duty=="環保局") || (duty =="清潔隊"))
                {
                    StatisticsModel Statistics = new StatisticsModel();
                    string Msg2 = Statistics.StoreNewestUpdateTime("Users", city);                
                    System.Diagnostics.Debug.WriteLine(Msg2);
                }

                return ("、成為正式用戶");
            }
            catch (Exception ex)
            {
                X.Close();return ("，但未加入用戶名單"); }
     
        }

        public String Delete(string UserName)
        {
            try
            {
                X.Open();
                string G = "Delete Users where UserName = @UserName";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@UserName", UserName);
                Q.ExecuteNonQuery();

                //在表格NewestUpdateTime 登入聯絡人(users)資訊的最新更新時間
                //User是SQL關鍵字，所以改為Users 
                //刪除時沒有Duty值，因此內部程式沒被執行

                if ((Duty == "環保局") || (Duty == "清潔隊"))
                {
                    StatisticsModel Statistics = new StatisticsModel();
                    string Msg2 = Statistics.StoreNewestUpdateTime("Users", City);
                    System.Diagnostics.Debug.WriteLine(Msg2);
                }

                X.Close();
                return ("已成功刪除資料");
            }
            catch (Exception) { X.Close(); return ("抱歉，資料未刪除"); }
        }


        public String Delete(string UserName, string Duty, string City)
        {
            try
            {   X.Open();
                string G = "Delete Users where UserName = @UserName";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@UserName", UserName);
                Q.ExecuteNonQuery();

                //在表格NewestUpdateTime 登入聯絡人(users)資訊的最新更新時間
                //User是SQL關鍵字，所以改為Users 
                //刪除時沒有Duty值，因此內部程式沒被執行

                if ((Duty == "環保局") || (Duty == "清潔隊"))
                {
                    StatisticsModel Statistics = new StatisticsModel();
                    string Msg2 = Statistics.StoreNewestUpdateTime("Users", City);
                    System.Diagnostics.Debug.WriteLine(Msg2);
                }

                X.Close();
                return("已成功刪除資料"); }
            catch (Exception) { X.Close();return("抱歉，資料未刪除"); }
        }

        public bool SearchId(string username)
        {
            try {
                X.Open();
                string G = "Select * from [Users] where UserName= @UserName";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@UserName", username);
                Q.ExecuteNonQuery();
                X.Close();
                SqlDataReader R = Q.ExecuteReader();
                if (R.Read() == true)
                { return true;}
                else { return false; }
            }
            catch (Exception) { X.Close(); return false; }
        }

        public string ChangeVoicePwd(string username, string NewVoicePwd)
        {
            try
            {
                X.Open();
                string G = "Update [Users] Set VoicePwd=@NewVoicePwd where username=@UserName";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@UserName", username);
                Q.Parameters.AddWithValue("@NewVoicePwd", NewVoicePwd);
                Q.ExecuteNonQuery();
                X.Close();
              
                return ("已更換語音密碼");
            }
            catch (Exception)
            {
                X.Close();
                return ("抱歉，未更換語音密碼");
            }
        }

        public string ChangePwd(string username, string NewPwd)
        {
            try
            {
                X.Open();
                string G = "Update [Users] Set Pwd=@NewPwd, PwdUpdateDate=@PwdUpdateDate where UserName= @UserName";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@UserName", username);
                Q.Parameters.AddWithValue("@NewPwd", NewPwd);
                Q.Parameters.AddWithValue("@PwdUpdateDate", DateTime.Now.AddDays(90));
                Q.ExecuteNonQuery();
                
                X.Close();
                return ("已更換網頁密碼");
            }
            catch (Exception)
            {
                X.Close();
                return ("抱歉，未更換網頁密碼");
            }
        }

        public LinkedList<UserModel> Search(string Duty,string City,string Town, string HumanType, string MainContacter)
        {       
            UserModel A = new UserModel();
            LinkedList<UserModel> C = new LinkedList<UserModel>();
            string G="Select * from [Users] where ";
            try
            {
                X.Open();
                SqlCommand Q;
                if (Duty == "ALL") { G = G + "(1=1)"; }
                else { G = G + "(Duty =N'" + Duty + "')"; }

                if (City == "ALL") { G = G + " And (1=1)"; }
                else { G = G + " And (City  =N'" + City + "')"; }

                if (Town== "ALL") { G = G + " And (1=1)"; }
                else { G = G + " And (Town  =N'" + Town+ "')"; }

                if (HumanType == "ALL") { G = G + " And (1=1)"; }
                else { G = G + " And (HumanType  =N'" + HumanType + "')"; }

                if (MainContacter== "ALL") { G = G + " And (1=1)"; }
                else { G = G + " And (MainContacter  =N'" + MainContacter + "')"; }
                Q = new SqlCommand(G, X);
                System.Diagnostics.Debug.WriteLine("******************************");
                System.Diagnostics.Debug.WriteLine(G);

                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                
                while (R.Read() == true)
                {
                    A.UserName = Convert.ToString(R["UserName"]);
                    A.Name = Convert.ToString(R["Name"]);
                    A.Pwd = Convert.ToString(R["Pwd"]);
                    A.VoicePwd = Convert.ToString(R["VoicePwd"]);
                    A.Duty = Convert.ToString(R["Duty"]);
                    A.City = Convert.ToString(R["City"]);
                    A.Town = Convert.ToString(R["Town"]);
                    A.MobilePhone = Convert.ToString(R["MobilePhone"]);
                    A.HumanType = Convert.ToString(R["HumanType"]);
                    A.MainContacter = Convert.ToString(R["MainContacter"]);
                    A.ReportPriority = Convert.ToString(R["ReportPriority"]);
                    A.DutyId = R["DutyId"].TryToInt().GetValueOrDefault();
                    A.CityId = R["CityId"].TryToInt();
                    A.TownId = R["TownId"].TryToInt();
                    A.PositionId = R["PositionId"].TryToInt();
                    C.AddFirst(A);
                    A = null;
                    A = new UserModel();
                }
            }
            catch (Exception)
            {
                A.UserName = "連通失敗";
                C.AddFirst(A);
            }
            finally { X.Close(); }
            return C;
        }

        public LinkedList<UserModel> Show(string City)
        {
            UserModel A = new UserModel();
            LinkedList<UserModel> C = new LinkedList<UserModel>();
            string G;
            try
            {
                X.Open();
                SqlCommand Q;
                if (City == "ALL")
                {
                    G = "Select * from [Users]";
                    Q = new SqlCommand(G, X);
                }
                else
                {
                    G = "Select * from [Users] where City=@City";
                    Q = new SqlCommand(G, X);
                    Q.Parameters.AddWithValue("@City", City);
                }
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                while (R.Read() == true)
                {
                    A.UserName = Convert.ToString(R["UserName"]);
                    A.Name= Convert.ToString(R["Name"]);
                    A.Pwd = Convert.ToString(R["Pwd"]);
                    A.VoicePwd = Convert.ToString(R["VoicePwd"]);
                    A.Duty = Convert.ToString(R["Duty"]);
                    A.City = Convert.ToString(R["City"]);
                    A.Town = Convert.ToString(R["Town"]);
                    A.MobilePhone= Convert.ToString(R["MobilePhone"]);
                    A.HumanType = Convert.ToString(R["HumanType"]);
                    A.MainContacter = Convert.ToString(R["MainContacter"]);
                    A.ReportPriority= Convert.ToString(R["ReportPriority"]);
                    A.DutyId = R["DutyId"].TryToInt().GetValueOrDefault();
                    A.CityId = R["CityId"].TryToInt();
                    A.TownId = R["TownId"].TryToInt();
                    A.PositionId = R["PositionId"].TryToInt();
                    C.AddFirst(A);
                    A = null;
                    A = new UserModel();
                }
            }
            catch (Exception)
            {
                A.UserName = "連通失敗";
                C.AddFirst(A);
            }
            finally { X.Close(); }
            return C;
        }

    }
}