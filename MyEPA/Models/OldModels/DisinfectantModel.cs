using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Web.Configuration;
using System.ComponentModel;
using MyEPA.Helper;

namespace MyEPA.Models
{
    public class DisinfectantModel
    {
        //提醒：請確定目前資料庫用主機上的或Local的，以及在專案版本更新後，
        //Web.Config的連接字串是否有更新
        //以免造成資料庫內容更多，但是程式抓到的並非該資料庫內容的現象

        public SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());
        [AutoKey]
        public int Id { get; set; }
        [DisplayName("縣市")]
        public string City { get; set; }
        [DisplayName("鄉鎮名")]
        public string Town { get; set; }
        [DisplayName("部門")]
        public string ContactUnit { get; set; }
        [DisplayName("藥品名稱")]
        public string DrugName{ get; set; }
        [DisplayName("類別")]
        public string DrugType { get; set; }
        [DisplayName("狀態	")]
        public string DrugState { get; set; }
        [DisplayName("數量")]
        public decimal Amount{ get; set; }
        [DisplayName("濃度")]
        public string Density { get; set; }
        [DisplayName("可消毒面積")]
        public decimal Area { get; set; }
        [DisplayName("使用年限")]
        public DateTime ServiceLife { get; set; }
        [DisplayName("資料更新日期")]
        public DateTime UpdateTime { get; set; }
        public DateTime? ConfirmTime { get; set; }
        /// <summary>
        /// 參考 DisinfectantTypeEnum.cs
        /// </summary>
        [DisplayName("用途")]
        public int UseType { get; set; }
        public string ActiveIngredients1 { get; set; }
        public string ActiveIngredients2 { get; set; }

        public String Update(string Id, string City, string Town, string ContactUnit,string DrugName, string DrugType, string DrugState, string Amount, string Density, string Area, string ServiceLife,int UseType, string ActiveIngredients1, string ActiveIngredients2)
        {
            try
            {
                X.Open();
                string G = "Update Disinfectant Set ContactUnit=@ContactUnit, DrugName=@DrugName, DrugType=@DrugType,DrugState=@DrugState, Amount=@Amount, Density=@Density, Area =@Area, ServiceLife=@ServiceLife,UseType=@UseType,UpdateTime=@UpdateTime,ActiveIngredients1=@ActiveIngredients1,ActiveIngredients2=@ActiveIngredients2 where Id=@Id";

                //string G = "Update Disinfectant Set ContactUnit=@ContactUnit, DrugName=@DrugName, DrugType=@DrugType,DrugState=@DrugState, Amount=@Amount, Density=@Density, Area =@Area, ServiceLife=@ServiceLife, UpdateTime=@UpdateTime where Id=@Id";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@Id", Id);
                Q.Parameters.AddWithValue("@City", City);
                Q.Parameters.AddWithValue("@Town", Town);
                Q.Parameters.AddWithValue("@ContactUnit", ContactUnit);
                Q.Parameters.AddWithValue("@DrugName", DrugName);
                Q.Parameters.AddWithValue("@DrugType", DrugType);
                Q.Parameters.AddWithValue("@DrugState", DrugState);
                Q.Parameters.AddWithValue("@Amount", Amount);
                Q.Parameters.AddWithValue("@Density", Density);
                Q.Parameters.AddWithValue("@Area", Area);
                Q.Parameters.AddWithValue("@ServiceLife", ServiceLife);
                Q.Parameters.AddWithValue("@UseType", UseType);
                Q.Parameters.AddWithValue("@ActiveIngredients1", ActiveIngredients1);
                Q.Parameters.AddWithValue("@ActiveIngredients2", ActiveIngredients2);
                // HH表示24小時，hh表示12小時
                // 之前網站上傳到 gear.host之後，會呈現主機的美國時間
                // 以下方法讓網站上傳到哪，都呈現臺北時間

                var TaipeiTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
                DateTime TaipeiLocalTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TaipeiTimeZoneInfo);
                Q.Parameters.AddWithValue("@UpdateTime", TaipeiLocalTime.ToString("yyyy/MM/dd HH: mm:ss"));

                Q.ExecuteNonQuery();
                X.Close();
                StatisticsModel Statistics = new StatisticsModel();
                string Msg2 = Statistics.StoreNewestUpdateTime("Disinfectant", City);
                return ("已更新資料");
            }
            catch (Exception ex)
            {
                X.Close();
                return ("未更新資料");
            }
        }
 
        public String Delete(string Id, String City)
        {
            try {  X.Open();
                string G = "Delete Disinfectant where Id = @Id and City=@City";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@Id", Id);
                Q.Parameters.AddWithValue("@City", City);
                Q.ExecuteNonQuery();
                X.Close();
                StatisticsModel Statistics = new StatisticsModel();
                string Msg2 = Statistics.StoreNewestUpdateTime("Disinfectant", City);
                return ("已刪除資料");}
            catch (Exception) {
                X.Close();
                return ("資料未被刪除"); }
        }


        public LinkedList<DisinfectantModel> Search(string City,string Town)
        {
            DisinfectantModel A = new DisinfectantModel();
            LinkedList<DisinfectantModel> C = new LinkedList<DisinfectantModel>();
            string G;
            try
            {
                X.Open();
                if (City == "ALL" && Town == "ALL")
                { G = "Select * from [Disinfectant]"; }
                else if (City != "ALL" && Town == "ALL")
                { G = "Select * from [Disinfectant] where City=@City"; }
                else if (City == "ALL" && Town != "ALL")
                { G = "Select * from [Disinfectant] where Town=@Town"; }
                else
                { G = "Select * from [Disinfectant] where City=@City and Town=@Town"; }

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
                    A.DrugName = Convert.ToString(R["DrugName"]);
                    A.DrugType = Convert.ToString(R["DrugType"]);
                    A.DrugState = Convert.ToString(R["DrugState"]);

                    A.Amount = Convert.ToDecimal(R["Amount"]);
                    A.Density = Convert.ToString(R["Density"]);
                    A.Area = Convert.ToDecimal(R["Area"]);
                    A.ServiceLife = Convert.ToDateTime(R["ServiceLife"]);
                    A.UpdateTime = Convert.ToDateTime(R["UpdateTime"]);
                    A.UseType = Convert.ToInt32(R["UseType"]);
                    C.AddFirst(A);
                    A = null;
                    A = new DisinfectantModel();
                }
            }
            catch (Exception ex)
            {
                C.AddFirst(A);

            }
            finally { X.Close(); }
            return C;
        }


        public LinkedList<DisinfectantModel> Show(string City)
        {
            DisinfectantModel A = new DisinfectantModel();
            LinkedList<DisinfectantModel> C = new LinkedList<DisinfectantModel>();
            string G;
            try
            {
                X.Open();
                if (City == "ALL")
                {
                    G = "Select * from [Disinfectant]";
                }
                else
                {
                    G = "Select * from [Disinfectant] where City=@City";
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
                    A.DrugName = Convert.ToString(R["DrugName"]);
                    A.DrugType= Convert.ToString(R["DrugType"]);
                    A.DrugState=Convert.ToString(R["DrugState"]);

                    A.Amount = Convert.ToDecimal(R["Amount"]);
                    A.Density = Convert.ToString(R["Density"]);
                    A.Area = Convert.ToDecimal(R["Area"]);
                    A.ServiceLife= Convert.ToDateTime(R["ServiceLife"]);
                    A.UpdateTime = DateTimeHelper.GetCurrentTime();
                    C.AddFirst(A);
                    A = null;
                    A = new DisinfectantModel();
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