using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Web.Configuration;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class IncineratorModel
    {
        //提醒：請確定目前資料庫用主機上的或Local的，以及在專案版本更新後，
        //Web.Config的連接字串是否有更新
        //以免造成資料庫內容更多，但是程式抓到的並非該資料庫內容的現象

        public SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());
        [AutoKey]
        public int Id { get; set; }
        [DisplayName("職繫單位名稱")]
        public String ContactUnit { get; set; }
        public decimal Xpos { get; set; }
        public decimal Ypos { get; set; }
        [DisplayName("縣市")]
        public String City { get; set; }
        [DisplayName("鄉鎮市")]
        public String Town { get; set; }
        public DateTime UpdateTime { get; set; }
        [DisplayName("地址")]
        public String Address { get; set; }
        public String ContactPerson { get; set; }
        public String ContactPersonTitle { get; set; }
        public String ContactPhone { get; set; }
        public String DesignCapacity { get; set; }
        public DateTime ConfirmTime { get; set; }

        public String Add(string ContactUnit, string DesignCapacity,string Xpos, string Ypos, string City,string Town,string Address, string ContactPerson, string ContactPersonTitle, string ContactPhone)
        {
            try {
                X.Open();

                string G = "Insert into Incinerator( ContactUnit, DesignCapacity, Xpos, Ypos,City,Town, Address,ContactPerson, ContactPersonTitle, ContactPhone,UpdateTime) Values(@ContactUnit,@DesignCapacity,@Xpos,@Ypos,@City,@Town,@Address,@ContactPerson, @ContactPersonTitle, @ContactPhone,@UpdateTime)";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@ContactUnit", ContactUnit);
                Q.Parameters.AddWithValue("@DesignCapacity",DesignCapacity);
                Q.Parameters.AddWithValue("@Xpos", Xpos);
                Q.Parameters.AddWithValue("@Ypos", Ypos);
                Q.Parameters.AddWithValue("@City", City);
                Q.Parameters.AddWithValue("@Town", Town);
                Q.Parameters.AddWithValue("@Address", Address);
                Q.Parameters.AddWithValue("@ContactPerson", ContactPerson);
                Q.Parameters.AddWithValue("@ContactPersonTitle", ContactPersonTitle);
                Q.Parameters.AddWithValue("@ContactPhone", ContactPhone);
                var TaipeiTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
                DateTime TaipeiLocalTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TaipeiTimeZoneInfo);
                Q.Parameters.AddWithValue("@UpdateTime", TaipeiLocalTime.ToString("yyyy/MM/dd HH: mm:ss"));
                Q.ExecuteNonQuery();
                StatisticsModel Statistics = new StatisticsModel();
                string Msg2 = Statistics.StoreNewestUpdateTime("Incinerator", "環境部");

                X.Close();

                return ("已新增資料"); }
            catch (Exception ex) {
                X.Close();
                return("未新增資料");}
        }

       

        public String Delete(string Id)
        {
            try {  X.Open();
                string G = "Delete Incinerator where Id = @Id";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@Id", Id);
                Q.ExecuteNonQuery();
                X.Close();
                StatisticsModel Statistics = new StatisticsModel();
                string Msg2 = Statistics.StoreNewestUpdateTime("Incinerator", "環境部");
                return ("已刪除資料");}
            catch (Exception) {
                X.Close();
                return ("資料未被刪除"); }
        }

        public IncineratorModel GetItem(int Id)
        {
            try
            {
                X.Open();
                string G = "Select * from Incinerator where Id = @Id";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@Id", Id);
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                IncineratorModel A = new IncineratorModel();
                if (R.Read() == true)
                {
                    A.Id = Convert.ToInt32(R["Id"]);
                    A.ContactUnit = Convert.ToString(R["ContactUnit"]);
                    A.Xpos = Convert.ToDecimal(R["Xpos"]);
                    A.Ypos = Convert.ToDecimal(R["Ypos"]);
                    A.City = Convert.ToString(R["City"]);
                    A.Town = Convert.ToString(R["Town"]);
                    A.UpdateTime = Convert.ToDateTime(R["UpdateTime"]);
                    A.Address = Convert.ToString(R["Address"]);
                    A.DesignCapacity= Convert.ToString(R["DesignCapacity"]);
                    A.ContactPerson= Convert.ToString(R["ContactPerson"]);
                    A.ContactPersonTitle = Convert.ToString(R["ContactPersonTitle"]);
                    A.ContactPhone = Convert.ToString(R["ContactPhone"]);
                    X.Close(); return A;

                }
                else { X.Close(); return null; }
            }
            catch (Exception ex)
            {
                X.Close();
                return null;
            }
        }


        public LinkedList<IncineratorModel> Show(string city)
        {
            IncineratorModel A = new IncineratorModel();
            LinkedList<IncineratorModel> C = new LinkedList<IncineratorModel>();
            string G;
            try
            {
                X.Open();
                if (city == "ALL")
                {
                    G = "Select * from Incinerator";
                }
                else
                {
                    G = "Select * from Incinerator where City=@City";
                }
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@City", city);
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                while (R.Read() == true)
                {
                    A.Id = Convert.ToInt32(R["Id"]);
                    A.ContactUnit= Convert.ToString(R["ContactUnit"]);
                    A.Xpos= Convert.ToDecimal(R["Xpos"]);
                    A.Ypos = Convert.ToDecimal(R["Ypos"]);
                    A.City = Convert.ToString(R["City"]);
                    A.Town= Convert.ToString(R["Town"]);
                    A.UpdateTime = Convert.ToDateTime(R["UpdateTime"]);
                    A.Address = Convert.ToString(R["Address"]);
                    A.DesignCapacity= Convert.ToString(R["DesignCapacity"]);

                    A.ContactPerson = Convert.ToString(R["ContactPerson"]);
                    A.ContactPersonTitle = Convert.ToString(R["ContactPersonTitle"]);
                    A.ContactPhone = Convert.ToString(R["ContactPhone"]);
                    C.AddFirst(A);
                    A = null;
                    A = new IncineratorModel();
                }

            }
            catch (Exception)
            {
                A.ContactUnit = "連通失敗";
                A.City = "連通失敗";
                C.AddFirst(A);
            }
            finally { X.Close(); }

            return C;
        }
    }
}