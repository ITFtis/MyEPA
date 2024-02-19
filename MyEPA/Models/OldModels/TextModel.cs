using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Web.Configuration;
using System.ComponentModel.DataAnnotations.Schema;
using MyEPA.Helper;

namespace MyEPA.Models
{

    public class TextModel
    { 
        public SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());
        [AutoKey]
        public int Id { get; set; }
        public String Topic { get; set; }
        public String PhoneNumber { get; set; }
        public String Content { get; set; }
        public DateTime SendTime { get; set; }
        public String SendResult { get; set; }

        public String Add(string Topic, string PhoneNumber, string Content, string SendResult)
        {
            try
            {
                X.Open();
                string G = "Insert into Text(Topic, PhoneNumber, Content, SendTime, SendResult) Values(@Topic, @PhoneNumber, @Content, @SendTime,@SendResult)";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@Topic", Topic);
                Q.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                Q.Parameters.AddWithValue("@Content", Content);
                Q.Parameters.AddWithValue("@SendResult", SendResult);
                var TaipeiTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
                
                Q.Parameters.AddWithValue("@SendTime", DateTimeHelper.GetCurrentTime());
                Q.ExecuteNonQuery();
                X.Close();
                return ("簡訊已存檔。");
            }
            catch (Exception)
            {
                X.Close();
                return ("簡訊未存檔。");
            }
        }

        public LinkedList<TextModel> SearchByDays(DateTime BeginDay, DateTime EndDay)
        {
            TextModel A = new TextModel();
            LinkedList<TextModel> C = new LinkedList<TextModel>();
            string G;
            try
            {
                X.Open();
                G = "Select * from  Text where SendTime>=@BeginDay and SendTime<=@EndDay";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@BeginDay", BeginDay);
                Q.Parameters.AddWithValue("@EndDay", EndDay);
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                while (R.Read() == true)
                {
                    A.Id = Convert.ToInt32(R["Id"]);
                    A.Topic = Convert.ToString(R["Topic"]);
                    A.PhoneNumber = Convert.ToString(R["PhoneNumber"]);
                    A.Content = Convert.ToString(R["Content"]);
                    A.SendTime = Convert.ToDateTime(R["SendTime"]);
                    A.SendResult = Convert.ToString(R["SendResult"]);
                    C.AddFirst(A);
                    A = null;
                    A = new TextModel();
                }
                X.Close();
            }
            catch (Exception)
            {
                A.Id = 1;
                A.Topic = "失敗";
                A.PhoneNumber = "失敗";
                A.Content = "失敗";
                A.SendResult = "失敗";
                C.AddFirst(A);
            }
            finally { X.Close(); }
            return C;
        }


        public AdminResultModel<Every8DResultModel> Send(string Topic, string Content, IEnumerable<string> PhoneNumbers)
        {
            var Result = new SMSHttp().SendSMSNow(Topic, Content, PhoneNumbers);
            return Result;
        }


        public LinkedList<TextModel> Show(string City)
        {
            TextModel A = new TextModel();
            LinkedList<TextModel> C = new LinkedList<TextModel>();
            string G;
            try
            {
                X.Open();
                if (City == "ALL")
                {G = "Select * from Text";}
                else
                {G = "Select * from Text where City=@City";}
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@City", City);
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                while (R.Read() == true)
                {
                    A.Id = Convert.ToInt32(R["Id"]);
                    A.Topic = Convert.ToString(R["Topic"]);
                    A.PhoneNumber= Convert.ToString(R["PhoneNumber"]);
                    A.Content = Convert.ToString(R["Content"]);
                    A.SendTime= Convert.ToDateTime(R["SendTime"]);
                    A.SendResult = Convert.ToString(R["SendResult"]);
                    C.AddFirst(A);
                    A = null;
                    A = new TextModel();
                }
            }
            catch (Exception)
            {
                A.Id = 1;
                A.Topic = "連通失敗";
                A.PhoneNumber = "連通失敗";
                A.Content= "連通失敗";
                A.SendResult= "連通失敗";
                C.AddFirst(A);
            }
            finally { X.Close(); }
            return C;
        }
    }
}