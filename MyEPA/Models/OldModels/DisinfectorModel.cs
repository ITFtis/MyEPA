using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Web.Configuration;
using System.ComponentModel;
using MyEPA.Extensions;
using MyEPA.Helper;

namespace MyEPA.Models
{
    public class DisinfectorModel
    {
        //提醒：請確定目前資料庫用主機上的或Local的，以及在專案版本更新後，
        //Web.Config的連接字串是否有更新
        //以免造成資料庫內容更多，但是程式抓到的並非該資料庫內容的現象

        public SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());
        [AutoKey]
        public int Id { get; set; }
        [DisplayName("縣市")]
        public String City { get; set; }
        [DisplayName("鄉鎮名")]
        public String Town { get; set; }
        [DisplayName("部門")]
        public String ContactUnit { get; set; }
        [DisplayName("消毒設備名稱")]
        public String DisinfectInstrument { get; set; }
        [DisplayName("規格")]
        public String Standard { get; set; }
        [DisplayName("數量")]
        public String Amount{ get; set; }
        [DisplayName("購置年份")]
        public String ROCyear { get; set; }
        [DisplayName("是否跨縣市調度")]
        public bool? IsSupportCity { get; set; }
        [DisplayName("跨縣市調度數量")]
        public int? SupportCityNum { get; set; }
        [DisplayName("資料更新日期")]
        public DateTime? UpdateTime { get; set; }
        [DisplayName("資料更新者")]
        public string UpdateUser { get; set; }
        public DateTime? ConfirmTime { get; set; }
        public int? UseType { get; set; }

        public String Update(string Id, string City, string Town, string ContactUnit, string DisinfectInstrument, string Standard, string Amount, string ROCyear,int? UseType, string UserName, bool? IsSupportCity, int? SupportCityNum)
        {
            try
            {
                X.Open();
                string G = @"
Update Disinfector 
Set ContactUnit = @ContactUnit
, DisinfectInstrument = @DisinfectInstrument
, Standard = @Standard
, Amount = @Amount
, ROCyear = @ROCyear 
, UpdateTime = @UpdateTime
, ConfirmTime = @ConfirmTime
, UpdateUser = @UpdateUser
, UseType = @UseType
, IsSupportCity = @IsSupportCity
, SupportCityNum = @SupportCityNum
where Id=@Id";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@Id", Id);
                Q.Parameters.AddWithValue("@City", City);
                Q.Parameters.AddWithValue("@Town", Town);
                Q.Parameters.AddWithValue("@ContactUnit", ContactUnit);
                Q.Parameters.AddWithValue("@DisinfectInstrument", DisinfectInstrument);
                Q.Parameters.AddWithValue("@Standard", Standard);
                Q.Parameters.AddWithValue("@Amount", Amount);
                Q.Parameters.AddWithValue("@ROCyear", ROCyear);
                Q.Parameters.AddWithValue("@UpdateTime", DateTimeHelper.GetCurrentTime());
                Q.Parameters.AddWithValue("@ConfirmTime", DateTimeHelper.GetCurrentTime());
                Q.Parameters.AddWithValue("@UseType", UseType);
                Q.Parameters.AddWithValue("@UpdateUser", UserName);
                Q.Parameters.AddWithValue("@IsSupportCity", IsSupportCity);
                if (SupportCityNum == null)
                {
                    Q.Parameters.AddWithValue("@SupportCityNum", DBNull.Value);
                }
                else
                {
                    Q.Parameters.AddWithValue("@SupportCityNum", SupportCityNum);
                }             
                Q.ExecuteNonQuery();
                X.Close();
                StatisticsModel Statistics = new StatisticsModel();
                string Msg2 = Statistics.StoreNewestUpdateTime("Disinfector", City);
                return ("已更新資料");
            }
            catch (Exception)
            {
                X.Close();
                return ("未更新資料");
            }
        }

        public String Add(string Id, string City, string Town, string ContactUnit, string DisinfectInstrument,string Standard, string Amount,string ROCyear,int? UseType)
        {
            try {
                X.Open();
                string G = @"
                            Insert into Disinfector(Id, City,Town,ContactUnit,DisinfectInstrument, Standard,Amount,ROCyear,UpdateTime,UseType) 
                            Values(@Id, @City,@Town,@ContactUnit,@DisinfectInstrument, @Standard,@Amount,@ROCyear,@UpdateTime,@UseType)";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@Id", Id);
                Q.Parameters.AddWithValue("@City", City);
                Q.Parameters.AddWithValue("@Town", Town);
                Q.Parameters.AddWithValue("@ContactUnit", ContactUnit);
                Q.Parameters.AddWithValue("@DisinfectInstrument", DisinfectInstrument);
                Q.Parameters.AddWithValue("@Standard", Standard);
                Q.Parameters.AddWithValue("@Amount", Amount);
                Q.Parameters.AddWithValue("@ROCyear",ROCyear);
                Q.Parameters.AddWithValue("@UseType", UseType);
                // HH表示24小時，hh表示12小時
                // 之前網站上傳到 gear.host之後，會呈現主機的美國時間
                // 以下方法讓網站上傳到哪，都呈現臺北時間

                var TaipeiTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
                DateTime TaipeiLocalTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TaipeiTimeZoneInfo);
                Q.Parameters.AddWithValue("@UpdateTime", TaipeiLocalTime.ToString("yyyy/MM/dd HH: mm:ss"));
                Q.ExecuteNonQuery();
                X.Close();

                StatisticsModel Statistics = new StatisticsModel();
                string Msg2 = Statistics.StoreNewestUpdateTime("Disinfector", City);
                return ("已新增資料"); }
            catch (Exception) {
                X.Close();
                return("未新增資料");}
        }

      

        //以下方法刪除特定ID並且城市的垃圾堆置場
        //因為ID是唯一的，所以應該不需要City的參數，
        //等到找到呼叫它的程式後，再將此參數刪除

        public String Delete(string Id, String City)
        {
            try {  X.Open();
                string G = "Delete Disinfector where Id = @Id and City=@City";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@Id", Id);
                Q.Parameters.AddWithValue("@City", City);
                Q.ExecuteNonQuery();
                X.Close();
                StatisticsModel Statistics = new StatisticsModel();
                string Msg2 = Statistics.StoreNewestUpdateTime("Disinfector", City);
                return ("已刪除資料");}
            catch (Exception) {
                X.Close();
                return ("資料未被刪除"); }
        }



        public DisinfectorModel GetItem(string Id)
        {
            try
            {
                X.Open();
                string G = "Select * from Disinfector where Id = @Id";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@Id", Id);
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                DisinfectorModel A = new DisinfectorModel();
                if (R.Read() == true)
                {
                    A.Id = Convert.ToInt32(R["Id"]);
                    A.City = Convert.ToString(R["City"]);
                    A.Town= Convert.ToString(R["Town"]);
                    A.ContactUnit = Convert.ToString(R["ContactUnit"]);
                    A.DisinfectInstrument = Convert.ToString(R["DisinfectInstrument"]);
                    A.Standard = Convert.ToString(R["Standard"]);
                    A.Amount= Convert.ToString(R["Amount"]);
                    A.ROCyear = Convert.ToString(R["ROCyear"]);
                    A.UpdateTime = R["UpdateTime"] == null ? (DateTime?)null : R["UpdateTime"].ToString().TryToDateTime();
                    X.Close();              
                    return A;
                }
                else { X.Close(); return null; }
            }
            catch (Exception)
            {
                X.Close();
                return null;
            }
        }

        public LinkedList<DisinfectorModel> Search (string City, string Town)
        {
            DisinfectorModel A = new DisinfectorModel();
            LinkedList<DisinfectorModel> C = new LinkedList<DisinfectorModel>();
            string G;
            try
            {
                X.Open();
                if (City == "ALL" && Town=="ALL")
                {  G = "Select * from [Disinfector]"; }
                else if (City != "ALL" && Town=="ALL")
                { G = "Select * from [Disinfector] where City=@City";}
                else if (City == "ALL" && Town != "ALL")
                { G = "Select * from [Disinfector] where Town=@Town"; }
                else 
                { G = "Select * from [Disinfector] where City=@City and Town=@Town"; }
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@City", City);
                Q.Parameters.AddWithValue("@Town", Town);
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                while (R.Read() == true)
                {
                    A.Id = Convert.ToInt32(R["Id"]);
                    A.ContactUnit = Convert.ToString(R["ContactUnit"]);
                    A.City = Convert.ToString(R["City"]);
                    A.Town = Convert.ToString(R["Town"]);
                    A.DisinfectInstrument = Convert.ToString(R["DisinfectInstrument"]);
                    A.Standard = Convert.ToString(R["Standard"]);
                    A.Amount = Convert.ToString(R["Amount"]);
                    A.ROCyear = Convert.ToString(R["ROCyear"]);
                    A.UpdateTime = R["UpdateTime"] == null ? (DateTime?)null : R["UpdateTime"].ToString().TryToDateTime();
                    C.AddFirst(A);
                    A = null;
                    A = new DisinfectorModel();
                }
            }
            catch (Exception)
            {
                C.AddFirst(A);

            }
            finally { X.Close(); }
            return C;
        }


        public LinkedList<DisinfectorModel> Show(string City)
        {
            DisinfectorModel A = new DisinfectorModel();
            LinkedList<DisinfectorModel> C = new LinkedList<DisinfectorModel>();
            string G;
            try
            {
                X.Open();
                if (City == "ALL")
                {
                    G = "Select * from [Disinfector]";
                }
                else
                {
                    G = "Select * from [Disinfector] where City=@City";
                }
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@City", City);
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                while (R.Read() == true)
                {
                    A.Id = Convert.ToInt32(R["Id"]);
                    A.ContactUnit= Convert.ToString(R["ContactUnit"]);
                    A.City= Convert.ToString(R["City"]);
                    A.Town = Convert.ToString(R["Town"]);
                    A.DisinfectInstrument = Convert.ToString(R["DisinfectInstrument"]);
                    A.Standard = Convert.ToString(R["Standard"]);
                    A.Amount = Convert.ToString(R["Amount"]);
                    A.ROCyear = Convert.ToString(R["ROCyear"]);
                    A.UpdateTime = R["UpdateTime"] == null ? (DateTime?)null : R["UpdateTime"].ToString().TryToDateTime();
                    C.AddFirst(A);
                    A = null;
                    A = new DisinfectorModel();
                }
            }
            catch (Exception)
            {
                C.AddFirst(A);
 
            }
            finally { X.Close(); }
            return C;
        }
    }
}