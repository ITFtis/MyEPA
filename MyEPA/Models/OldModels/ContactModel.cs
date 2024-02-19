using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Web.Configuration;
namespace MyEPA.Models
{
    public class ContactModel
    { 
        public SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());
        public String Id { get; set; }
        public String City { get; set; }
        public String Place1{ get; set; }
        public String Line1 { get; set; }
        public String Phone1 { get; set; }
        public String Fax1{ get; set; }
        public String Mail1{ get; set; }
        public String Place2 { get; set; }
        public String Line2 { get; set; }
        public String Phone2 { get; set; }
        public String Fax2 { get; set; }
        public String Mail2 { get; set; }
        public String Place3 { get; set; }
        public String Line3 { get; set; }
        public String Phone3 { get; set; }
        public String Fax3 { get; set; }
        public String Mail3 { get; set; }

        public String Update(string Place1, string Line1, string Phone1, string Fax1, string Mail1, string Place2, string Line2, string Phone2, string Fax2, string Mail2, string Place3, string Line3, string Phone3, string Fax3, string Mail3, string City)
        {
            try
            {
                X.Open();        
                MapModel Map= new MapModel();
                string CityCode = Map.FindCode(City);
                //where 後面限制的變數，一定要是key，所以不能寫成 where City='環保署'
                string G = "Update Contact Set Place1=@Place1, Line1=@Line1, Phone1=@Phone1, Fax1=@Fax1, Mail1=@Mail1,Place2=@Place2, Line2=@Line2, Phone2=@Phone2, Fax2=@Fax2, Mail2=@Mail2,Place3=@Place3, Line3=@Line3, Phone3=@Phone3, Fax3=@Fax3, Mail3=@Mail3 where Id=@CityCode";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@Place1",Place1);
                Q.Parameters.AddWithValue("@Line1", Line1);
                Q.Parameters.AddWithValue("@Phone1", Phone1);
                Q.Parameters.AddWithValue("@Fax1", Fax1);
                Q.Parameters.AddWithValue("@Mail1", Mail1);
                Q.Parameters.AddWithValue("@Place2", Place2);
                Q.Parameters.AddWithValue("@Line2", Line2);
                Q.Parameters.AddWithValue("@Phone2", Phone2);
                Q.Parameters.AddWithValue("@Fax2", Fax2);
                Q.Parameters.AddWithValue("@Mail2", Mail2);
                Q.Parameters.AddWithValue("@Place3", Place3);
                Q.Parameters.AddWithValue("@Line3", Line3);
                Q.Parameters.AddWithValue("@Phone3", Phone3);
                Q.Parameters.AddWithValue("@Fax3", Fax3);
                Q.Parameters.AddWithValue("@Mail3", Mail3);
                Q.Parameters.AddWithValue("@CityCode", CityCode);
                Q.ExecuteNonQuery();
                X.Close();
                return ("已更新單一聯繫窗口的資料");
            }
            catch (Exception)
            {
                X.Close();
                return ("抱歉，未更新單一聯繫窗口的資料");
            }
        }
               

        public LinkedList<ContactModel> Show(string City)
        {
            ContactModel A = new ContactModel();
            LinkedList<ContactModel> C = new LinkedList<ContactModel>();
            string G;
            try
            {
                X.Open();
                if (City == "ALL")
                {
                    G = "Select * from [Contact] ORDER BY Id DESC";
                }
                else
                {
                    G = "Select * from [Contact] where City=@City";
                }
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@City", City);
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                while (R.Read() == true)
                {
                    A.Id = Convert.ToString(R["Id"]);
                    A.City = Convert.ToString(R["City"]);
                    A.Place1= Convert.ToString(R["Place1"]);
                    A.Line1= Convert.ToString(R["Line1"]);
                    A.Phone1 = Convert.ToString(R["Phone1"]);
                    A.Fax1= Convert.ToString(R["Fax1"]);
                    A.Mail1= Convert.ToString(R["Mail1"]);
                    A.Place2 = Convert.ToString(R["Place2"]);
                    A.Line2 = Convert.ToString(R["Line2"]);
                    A.Phone2 = Convert.ToString(R["Phone2"]);
                    A.Fax2 = Convert.ToString(R["Fax2"]);
                    A.Mail2 = Convert.ToString(R["Mail2"]);
                    A.Place3 = Convert.ToString(R["Place3"]);
                    A.Line3 = Convert.ToString(R["Line3"]);
                    A.Phone3 = Convert.ToString(R["Phone3"]);
                    A.Fax3 = Convert.ToString(R["Fax3"]);
                    A.Mail3 = Convert.ToString(R["Mail3"]);
                    C.AddFirst(A);
                    A = null;
                    A = new ContactModel();
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
    }
}