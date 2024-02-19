using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Web.Configuration;
namespace MyEPA.Models
{
    public class ToiletplaceModel
    {    
        public SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());
        public string ToiletId { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string DiasterId { get; set; }
        public string DiasterName { get; set; }
        public string Xpos { get; set; }
        public string Ypos { get; set; }
        public string ToiletNumber { get; set; }
        public string ToiletType { get; set; }
        public string StartDay { get; set; }
        public string EndDay { get; set; }
        public string Manager { get; set; }
        public string ContactMethod { get; set; }
        public string Note { get; set; }

        public String Add(string TblName, string ToiletId, string City, string Town, string DiasterId, string DiasterName, string Xpos, string Ypos, string ToiletNumber, string ToiletType, string StartDay, string EndDay, string Manager, string ContactMethod, string Note)
        {
            try
            {
                X.Open();
                string G = "Insert into " + TblName + "(ToiletId, City, Town, DiasterId,  DiasterName, Xpos, Ypos,  ToiletNumber, ToiletType, StartDay, EndDay, Manager, ContactMethod, Note) Values(@ToiletId, @City, @Town, @DiasterId, @DiasterName, @Xpos, @Ypos,  @ToiletNumber, @ToiletType, @StartDay, @EndDay,@Manager,@ContactMethod,@Note)";
                System.Diagnostics.Debug.WriteLine(G);
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@ToiletId", ToiletId);
                Q.Parameters.AddWithValue("@City", City);
                Q.Parameters.AddWithValue("@Town", Town);
                Q.Parameters.AddWithValue("@DiasterId", DiasterId);
                Q.Parameters.AddWithValue("@DiasterName", DiasterName);
                Q.Parameters.AddWithValue("@Xpos", Xpos);
                Q.Parameters.AddWithValue("@Ypos", Ypos);
                Q.Parameters.AddWithValue("@ToiletNumber", ToiletNumber);
                Q.Parameters.AddWithValue("@ToiletType", ToiletType);
                Q.Parameters.AddWithValue("@StartDay", StartDay);
                Q.Parameters.AddWithValue("@EndDay", EndDay);
                Q.Parameters.AddWithValue("@Manager", Manager);
                Q.Parameters.AddWithValue("@ContactMethod", ContactMethod);
                Q.Parameters.AddWithValue("@Note", Note);
                Q.ExecuteNonQuery();
                X.Close();             
                return ("已新增廁所資料");
            }
            catch (Exception)
            {
                X.Close();
                return ("未新增資料");
            }
        }

        public LinkedList<ToiletplaceModel> Show(string City, string DiasterId)
        {
            ToiletplaceModel A = new ToiletplaceModel();
            LinkedList<ToiletplaceModel> C = new LinkedList<ToiletplaceModel>();
            string G;
           
            try
            {
                X.Open();
                if (City == "ALL")
                {
                    G = "Select * from Toiletplace"+DiasterId;
                }
                else
                {
                    G = "Select * from Toiletplace" + DiasterId+" where City=@City";
                }
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@City", City);
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                while (R.Read() == true)
                {
                    A.ToiletId = Convert.ToString(R["ToiletId"]);
                    A.City = Convert.ToString(R["City"]);
                    A.Town = Convert.ToString(R["Town"]);
                    A.DiasterId = Convert.ToString(R["DiasterId"]);
                    A.DiasterName = Convert.ToString(R["DiasterName"]);
                    A.Xpos = Convert.ToString(R["Xpos"]);
                    A.Ypos = Convert.ToString(R["Ypos"]);
                    A.ToiletNumber = Convert.ToString(R["ToiletNumber"]);
                    A.ToiletType = Convert.ToString(R["ToiletType"]);
                    A.StartDay = Convert.ToString(R["StartDay"]);
                    A.EndDay = Convert.ToString(R["EndDay"]);
                    A.Manager= Convert.ToString(R["Manager"]);
                    A.ContactMethod = Convert.ToString(R["ContactMethod"]);
                    A.Note = Convert.ToString(R["Note"]);
                    C.AddFirst(A);
                    A = null;
                    A = new ToiletplaceModel();
                }
            }
            catch (Exception)
            {
                A.ToiletId = "連通失敗";
                A.City = "連通失敗";
                C.AddFirst(A);
            }
            finally { X.Close(); }
            return C;
        }
    }
}