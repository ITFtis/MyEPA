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
    public class LandfillModel
    {
        public SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());
        [AutoKey]
        public int Id { get; set; }      
        [DisplayName("職繫單位名稱")]
        public string ContactUnit { get; set; }
        public decimal Xpos { get; set; }
        public decimal Ypos { get; set; }
        [DisplayName("縣市")]
        public string City { get; set; }
        [DisplayName("鄉鎮市")]
        public string Town{ get; set; }
        public string DesignCapacity { get; set; }
        public string ResidualCapacity { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonTitle { get; set; }
        public string ContactPhone{ get; set; }
        public DateTime UpdateTime { get; set; }
        [DisplayName("地址")]
        public string Address { get; set; }
        [DisplayName("臨時堆置場")]
        public string IsDump { get; set; }
        public DateTime? ConfirmTime { get; set; }

        public String Add(string Id, string ContactUnit, string Xpos, string Ypos, string City, string Town,string ContactPerson, string ContactPersonTitle, string ContactPhone, string DesignCapacity, string ResidualCapacity, string Address, string IsDump)
        {
            string G = ""; string msg = "";
            try{
                X.Open();
                G = "Insert into Landfill(Id, ContactUnit, Xpos, Ypos, City, Town,ContactPerson, ContactPersonTitle,ContactPhone, DesignCapacity, ResidualCapacity, Address, IsDump,UpdateTime) Values(@Id,@ContactUnit,@Xpos,@Ypos,@City,@Town, @ContactPerson, @ContactPersonTitle,@ContactPhone, @DesignCapacity,@ResidualCapacity, @Address, @IsDump, @UpdateTime)";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@Id", Id);
                Q.Parameters.AddWithValue("@ContactUnit", ContactUnit);
                Q.Parameters.AddWithValue("@Xpos", Xpos);
                Q.Parameters.AddWithValue("@Ypos", Ypos);
                Q.Parameters.AddWithValue("@City", City);
                Q.Parameters.AddWithValue("@Town", Town);
                Q.Parameters.AddWithValue("@ContactPerson", ContactPerson);
                Q.Parameters.AddWithValue("@ContactPersonTitle", ContactPersonTitle);
                Q.Parameters.AddWithValue("@ContactPhone", ContactPhone);
                Q.Parameters.AddWithValue("@DesignCapacity", DesignCapacity);
                Q.Parameters.AddWithValue("@ResidualCapacity", ResidualCapacity);
                Q.Parameters.AddWithValue("@Address", Address);
                Q.Parameters.AddWithValue("@IsDump", IsDump);
                var TaipeiTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
                DateTime TaipeiLocalTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TaipeiTimeZoneInfo);
                Q.Parameters.AddWithValue("@UpdateTime", TaipeiLocalTime.ToString("yyyy/MM/dd HH: mm:ss"));
                Q.ExecuteNonQuery();
                msg = "已成功新增資料";
                StatisticsModel Statistics = new StatisticsModel();
                string Msg2 = Statistics.StoreNewestUpdateTime("Landfill", "環保署");

            }
            catch (Exception) { msg = G; }
            finally { X.Close(); }
            return msg;
        }

        public String Delete(string Id)
        {
            string G = string.Empty; string msg = string.Empty;
            try{
                X.Open();
                G = "Delete Landfill where Id = @Id ";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@Id", Id);
                Q.ExecuteNonQuery();
                msg = "已刪除資料";
                StatisticsModel Statistics = new StatisticsModel();
                string Msg2 = Statistics.StoreNewestUpdateTime("Landfill", "環保署");

            }
            catch (Exception) { msg = G; }
            finally { X.Close(); }
            return msg;
        }

        public LandfillModel GetItem(string Id)
        {
            try{
                X.Open();
                string G = "Select * from Landfill where Id = @Id";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@Id", Id);
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                LandfillModel A = new LandfillModel();
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
                    A.DesignCapacity = Convert.ToString(R["DesignCapacity"]);
                    A.ResidualCapacity = Convert.ToString(R["ResidualCapacity"]);
                    A.ContactPerson = Convert.ToString(R["ContactPerson"]);
                    A.ContactPersonTitle = Convert.ToString(R["ContactPersonTitle"]);
                    A.ContactPhone = Convert.ToString(R["ContactPhone"]);
                    A.IsDump=Convert.ToString(R["IsDump"]);
                    X.Close(); return A;
                }
                else { X.Close(); return null; }}
            catch (Exception) {X.Close();return null;}
        }

        public LinkedList<LandfillModel> Show(string City)
        {
            LandfillModel A = new LandfillModel();
            LinkedList<LandfillModel> C = new LinkedList<LandfillModel>();
            string G;
            try
            {
                X.Open();
                if (City == "ALL")
                {
                    System.Diagnostics.Debug.WriteLine("測試2");
                    G = "Select * from Landfill";
                }
                else
                {

                    System.Diagnostics.Debug.WriteLine("測試3");
                    G = "Select * from Landfill where City = @City";
                }
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@City", City);
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                while (R.Read() == true)
                {
                    A.Id = Convert.ToInt32(R["Id"]);
                    A.ContactUnit = Convert.ToString(R["ContactUnit"]);
                    A.Xpos = Convert.ToDecimal(R["Xpos"]);
                    A.Ypos = Convert.ToDecimal(R["Ypos"]);
                    A.City = Convert.ToString(R["City"]);
                    A.Town = Convert.ToString(R["Town"]);
                    A.DesignCapacity = Convert.ToString(R["DesignCapacity"]);
                    A.ResidualCapacity = Convert.ToString(R["ResidualCapacity"]);
                    A.ContactPerson = Convert.ToString(R["ContactPerson"]);
                    A.ContactPersonTitle = Convert.ToString(R["ContactPersonTitle"]);
                    A.ContactPhone = Convert.ToString(R["ContactPhone"]);
                    A.Address = Convert.ToString(R["Address"]);
                    A.IsDump = Convert.ToString(R["IsDump"]);
                    A.UpdateTime = Convert.ToDateTime(R["UpdateTime"]);
                    C.AddFirst(A);
                    A = null; A = new LandfillModel();
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

            public LinkedList<LandfillModel> GetAll()
            {
            LandfillModel A = new LandfillModel();
            LinkedList<LandfillModel> C = new LinkedList<LandfillModel>();
            string G;
            try
            {
                X.Open();
                G = "Select * from Landfill";
               SqlCommand Q = new SqlCommand(G, X);
               Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                while (R.Read() == true)
                {
                    A.Id = Convert.ToInt32(R["Id"]);
                    A.ContactUnit = Convert.ToString(R["ContactUnit"]);
                    A.Xpos = Convert.ToDecimal(R["Xpos"]);
                    A.Ypos = Convert.ToDecimal(R["Ypos"]);
                    A.City = Convert.ToString(R["City"]);
                    A.Town = Convert.ToString(R["Town"]);
                    A.DesignCapacity = Convert.ToString(R["DesignCapacity"]);
                    A.ResidualCapacity = Convert.ToString(R["ResidualCapacity"]);
                    A.ContactPerson = Convert.ToString(R["ContactPerson"]);
                    A.ContactPersonTitle = Convert.ToString(R["ContactPersonTitle"]);
                    A.ContactPhone = Convert.ToString(R["ContactPhone"]);
                    A.Address = Convert.ToString(R["Address"]);
                    A.UpdateTime = Convert.ToDateTime(R["UpdateTime"]);
                    A.IsDump = Convert.ToString(R["IsDump"]);
                    C.AddFirst(A);
                    A = null;A = new LandfillModel();
                }
            }
            catch (Exception ex)
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