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

    public class EmisModel
    {
        public SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());
        public string Id { get; set; }
        public string VoicePwd { get; set; }
        public string ChangeVoicePwd(string Id, string NewPwd)
        {
            try
            {
                X.Open();
                string G = "Update [Emis] Set VoicePwd=@NewPwd where Id= @Id";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@Id", Id);
                Q.Parameters.AddWithValue("@NewPwd", NewPwd);
                Q.ExecuteNonQuery();              
                X.Close();
                return ("已更換語音密碼");
            }
            catch (Exception)
            {
                X.Close();
                return ("抱歉，未更換語音密碼");
            }
        }

        public UserModel FindInfo(string username, string pwd)
        {      
            UserModel Person = new UserModel();
            try
            {   X.Open();
                string G = "Select * from [Users] where UserName= @UserName";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@UserName", username);
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                if (R.Read() == true)
                {
                    String CorrectPwd = Convert.ToString(R["Pwd"]);
                    String LoginDuty = Convert.ToString(R["Duty"]).Trim();
                    String LoginCity = Convert.ToString(R["City"]).Trim();
                    if (pwd == CorrectPwd)
                    {
                        Person.UserName = username;
                        Person.Duty = LoginDuty;
                        Person.City = LoginCity;                
                    }
                    else
                    {
                        Person.UserName = "Null";
                        Person.Duty = "Null";
                        Person.City = "Null";
                    }
                }
                else
                {
                    Person.UserName = "Null";
                    Person.Duty = "Null";
                    Person.City = "Null";
                }
                X.Close(); return Person;
            }
            catch (Exception)
            {
                Person.UserName = "Null";
                Person.Duty = "Null";
                Person.City = "Null";
                X.Close(); return Person;
            }        
        }
                
    }
}