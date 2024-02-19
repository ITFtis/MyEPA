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
    public class VolunteerModel
    {
        public SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());
        public String Id { get; set; }
        [DisplayName("縣市")]
        public String City { get; set; }
        [DisplayName("聯絡人姓名")]
        public String ContactPerson { get; set; }
        [DisplayName("電話")]
        public String Phone { get; set; }
        [DisplayName("行動電話")]
        public String MobilePhone { get; set; }
        [DisplayName("傳真")]
        public String Fax{ get; set; }
        [DisplayName("電子郵件")]
        public String Mail{ get; set; }
        [DisplayName("提供服務項目")]
        public String Service { get; set; }
        [DisplayName("鄉鎮名")]
        public String Town { get; set; }
        [DisplayName("資料更新日期")]
        public DateTime? UpdateTime { get; set; }
        public LinkedList<VolunteerModel> Show(string City)
        {
            VolunteerModel A = new VolunteerModel();
            LinkedList<VolunteerModel> C = new LinkedList<VolunteerModel>();
            string G;
            try
            {
                X.Open();
                if (City == "ALL")
                {
                    G = "Select * from [Volunteer]";
                }
                else
                {
                    G = "Select * from [Volunteer] where City=@City";
                }
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@City", City);
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                while (R.Read() == true)
                {
                    A.Id = Convert.ToString(R["Id"]);
                    A.City= Convert.ToString(R["City"]);
                    A.ContactPerson= Convert.ToString(R["ContactPerson"]);
                    A.Phone= Convert.ToString(R["Phone"]);
                    A.MobilePhone = Convert.ToString(R["MobilePhone"]);
                    A.Fax = Convert.ToString(R["Fax"]);
                    A.Mail= Convert.ToString(R["Mail"]);
                    A.Service = Convert.ToString(R["Service"]);
                    C.AddFirst(A);
                    A = null;
                    A = new VolunteerModel();
                }
            }
            catch (Exception)
            {
                A.Id = "連通失敗";
                C.AddFirst(A);
            }
            finally { X.Close(); }
            return C;
        }

        public String Delete(string Id, String City)
        {
            try
            {
                X.Open();
                string G = "Delete Volunteer where Id = @Id and City=@City";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@Id", Id);
                Q.Parameters.AddWithValue("@City", City);
                Q.ExecuteNonQuery();
                X.Close();

                var TaipeiTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
                DateTime TaipeiLocalTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TaipeiTimeZoneInfo);
                StatisticsModel Statistics = new StatisticsModel();
                string Msg2 = Statistics.StoreNewestUpdateTime("Volunteer", City);

                return ("已刪除資料");
            }
            catch (Exception)
            {
                X.Close();
                return ("資料未被刪除");
            }
        }

        public String Add(string Id, string City, string ContactPerson, string Phone, string MobilePhone, string Fax, string Mail, string Service)
        {
            try
            {
                X.Open();
                string G = "Insert into Volunteer(Id, City,ContactPerson,Phone, MobilePhone, Fax,Mail,Service) Values(@Id, @City,@ContactPerson,@Phone, @MobilePhone, @Fax,@Mail, @Service)";

                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@Id", Id);
                Q.Parameters.AddWithValue("@City", City);
                Q.Parameters.AddWithValue("@ContactPerson", ContactPerson);
                Q.Parameters.AddWithValue("@Phone", Phone);
                Q.Parameters.AddWithValue("@MobilePhone", MobilePhone);    
                Q.Parameters.AddWithValue("@Fax", Fax);
                Q.Parameters.AddWithValue("@Mail", Mail);
                Q.Parameters.AddWithValue("@Service", Service);       
                Q.ExecuteNonQuery();
                X.Close();
                var TaipeiTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
                DateTime TaipeiLocalTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TaipeiTimeZoneInfo);
                StatisticsModel Statistics = new StatisticsModel();
                string Msg2 = Statistics.StoreNewestUpdateTime("Volunteer", City);
                return ("已新增資料");
            }
            catch (Exception)
            {
                X.Close();
                return ("未新增資料");
            }
        }
    }
}