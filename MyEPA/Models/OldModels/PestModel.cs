using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Web.Configuration;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class PestModel
    { 
        public SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());
        public string Id { get; set; }
        [DisplayName("公司名稱")]
        public string CompanyName { get; set; }
        [DisplayName("縣市")]
        public string City { get; set; }
        [DisplayName("鄉鎮名")]
        public string Town { get; set; }
        [DisplayName("公司地址")]
        public string Address { get; set; }
        [DisplayName("公司電話")]
        public string Phone { get; set; }
        [DisplayName("傳真")]
        public string Fax { get; set; }
        [DisplayName("許可執照號碼")]
        public string License { get; set; }
        [DisplayName("負責人")]
        public string ContactPerson { get; set; }
        [DisplayName("資料更新日期")]
        public string UpdateTime { get; set; }

        public LinkedList<PestModel> Show(string City)
        {
            PestModel A = new PestModel();
            LinkedList<PestModel> C = new LinkedList<PestModel>();
            string G;
            try
            {
                X.Open();
                if (City == "ALL")
                {G = "Select * from Pest";}
                else
                {G = "Select * from Pest where City=@City";}
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@City", City);
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                while (R.Read() == true)
                {
                    A.Id = Convert.ToString(R["Id"]);
                    A.CompanyName = Convert.ToString(R["CompanyName"]);
                    A.City= Convert.ToString(R["City"]);
                    A.Town = Convert.ToString(R["Town"]);
                    A.Address= Convert.ToString(R["Address"]);
                    A.Phone = Convert.ToString(R["Phone"]);
                    A.Fax = Convert.ToString(R["Fax"]);
                    A.License= Convert.ToString(R["License"]);
                    A.ContactPerson = Convert.ToString(R["ContactPerson"]);
                    A.UpdateTime = Convert.ToString(R["UpdateTime"]);
                    C.AddFirst(A);
                    A = null;
                    A = new PestModel();
                }
            }
            catch (Exception)
            {
                A.Id = "1";
                A.CompanyName = "連通失敗";
                A.City = "連通失敗";
                A.Town = "連通失敗";
                A.Address = "連通失敗";
                A.Phone = "連通失敗";
                A.Fax = "連通失敗";
                A.License = "連通失敗";
                A.ContactPerson = "連通失敗";
                A.UpdateTime = "連通失敗";
                C.AddFirst(A);
            }
            finally { X.Close(); }
            return C;
        }
    }
}