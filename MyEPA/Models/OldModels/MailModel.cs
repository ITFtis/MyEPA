using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Web.Configuration;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyEPA.Models
{

    public class MailModel
    { 
        public SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());
        public String Id { get; set; }
        public String Topic { get; set; }
        public String MailAddress { get; set; }
        public String Content { get; set; }
        public String SendTime { get; set; }
        public String SendResult { get; set; }

        public String Add(string Topic, string PhoneNumber, string Content, string SendResult)
        {
            try
            {
                X.Open();
                string G = "Insert into Mail(Id, Topic,MailAddress, Content, SendTime, SendResult) Values(@Id, @Topic, @MailAddress, @Content, @SendTime,@SendResult)";
                SqlCommand Q = new SqlCommand(G, X);
                int SecondsCount = (int)(DateTime.UtcNow - new DateTime(2000, 1, 1)).TotalSeconds;
                string Id = SecondsCount.ToString();
                Q.Parameters.AddWithValue("@Id", Id);
                Q.Parameters.AddWithValue("@Topic", Topic);
                Q.Parameters.AddWithValue("@MailAddress", MailAddress);
                Q.Parameters.AddWithValue("@Content", Content);
                Q.Parameters.AddWithValue("@SendResult", SendResult);
                var TaipeiTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
                DateTime TaipeiLocalTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TaipeiTimeZoneInfo);
                Q.Parameters.AddWithValue("@SendTime", TaipeiLocalTime.ToString("yyyy/MM/dd HH: mm:ss"));
                Q.ExecuteNonQuery();
                X.Close();
                return ("電郵已存檔。");
            }
            catch (Exception)
            {
                X.Close();
                return ("簡訊未存檔。");
            }
        }

        public LinkedList<MailModel> SearchByDays(DateTime BeginDay, DateTime EndDay)
        {
            MailModel A = new MailModel();
            LinkedList<MailModel> C = new LinkedList<MailModel>();
            string G;
            try
            {
                X.Open();
                G = "Select * from  Mail where SendTime>=@BeginDay and SendTime<=@EndDay";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@BeginDay", BeginDay);
                Q.Parameters.AddWithValue("@EndDay", EndDay);
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                while (R.Read() == true)
                {
                    A.Id = Convert.ToString(R["Id"]);
                    A.Topic = Convert.ToString(R["Topic"]);
                    A.MailAddress = Convert.ToString(R["MailAddress"]);
                    A.Content = Convert.ToString(R["Content"]);
                    A.SendTime = Convert.ToString(R["SendTime"]);
                    A.SendResult = Convert.ToString(R["SendResult"]);
                    C.AddFirst(A);
                    A = null;
                    A = new MailModel();
                }
                X.Close();
            }
            catch (Exception)
            {
                A.Id = "1";
                A.Topic = "失敗";
                A.MailAddress = "失敗";
                A.Content = "失敗";
                A.SendTime = "失敗";
                A.SendResult = "失敗";
                C.AddFirst(A);
            }
            finally { X.Close(); }
            return C;
        }



        //public bool Send(string Id, string Pwd, string Topic, string Content, string PhoneNumber)
        //{
        //    SMSHttp TextMachine = new SMSHttp();
        //    string SendTime = DateTime.Now.ToString("yyyyMMddHHmmss");
        //    bool Result = TextMachine.sendSMS(Id, Pwd, Topic, Content, PhoneNumber, SendTime);
        //    return Result;
        //}


        public LinkedList<MailModel> Show(string City)
        {
            MailModel A = new MailModel();
            LinkedList<MailModel> C = new LinkedList<MailModel>();
            string G;
            try
            {
                X.Open();
                if (City == "ALL")
                {G = "Select * from Mail";}
                else
                {G = "Select * from Mail where City=@City";}
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@City", City);
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                while (R.Read() == true)
                {
                    A.Id = Convert.ToString(R["Id"]);
                    A.Topic = Convert.ToString(R["Topic"]);
                    A.MailAddress= Convert.ToString(R["MailAddress"]);
                    A.Content = Convert.ToString(R["Content"]);
                    A.SendTime= Convert.ToString(R["SendTime"]);
                    A.SendResult = Convert.ToString(R["SendResult"]);
                    C.AddFirst(A);
                    A = null;
                    A = new MailModel();
                }
            }
            catch (Exception)
            {
                A.Id = "1";
                A.Topic = "連通失敗";
                A.MailAddress = "連通失敗";
                A.Content= "連通失敗";
                A.SendTime= "連通失敗";
                A.SendResult= "連通失敗";
                C.AddFirst(A);
            }
            finally { X.Close(); }
            return C;
        }
    }
}