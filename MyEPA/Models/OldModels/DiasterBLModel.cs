using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Web.Configuration;
namespace MyEPA.Models
{
    public class DiasterBLModel : DiasterModel
    {
        public SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());
        
        public string Add(string DiasterName, string DiasterType, string StartTime, string EndTime, string Comment,string DiasterState)
        {
            try
            {
                X.Open();
                string G = "Insert into Diaster(Id, DiasterName, DiasterType, StartTime, EndTime,Comment, DiasterState, CoverCity) Values (@Id, @DiasterName, @DiasterType, @StartTime,@EndTime,@Comment,@DiasterState,@CoverCity)";
                int SecondsCount = (int)(DateTime.UtcNow - new DateTime(2000, 1, 1)).TotalSeconds;
                SqlCommand Q = new SqlCommand(G, X);
                
                Q.Parameters.AddWithValue("@Id", SecondsCount);
                Q.Parameters.AddWithValue("@DiasterName", DiasterName);
                Q.Parameters.AddWithValue("@DiasterType", DiasterType);
                Q.Parameters.AddWithValue("@StartTime", StartTime);
                Q.Parameters.AddWithValue("@EndTime", EndTime);
                Q.Parameters.AddWithValue("@Comment", Comment);
                Q.Parameters.AddWithValue("@DiasterState", DiasterState);
                Q.Parameters.AddWithValue("@CoverCity", "00000000000000000000000");
                Q.ExecuteNonQuery();
                X.Close();

                return ("已新增資料。");
            }
            catch (Exception)
            {
                X.Close();
                return ("未新增資料");
            }
        }
        public bool[] GetCityCover(string DiasterName)
        {
            bool[] CityCover = new bool[23];
            try
            {
                X.Open();
                string G = "Select * from Diaster where DiasterName=@DiasterName";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@DiasterName", DiasterName);
                SqlDataReader R = Q.ExecuteReader();
                if (R.Read() == true)
                {
                    string temp= Convert.ToString(R["CoverCity"]);
                    CityCover= temp.Select(c => c == '1').ToArray();
                }
                Q.ExecuteNonQuery();
                X.Close();
                return (CityCover);
            }
            catch (Exception)
            {
                X.Close();
                return (CityCover);
            }
        }
        
        public String SetCityCover(string DiasterName, string CoverCity)
        {
            try
            {
                X.Open();
                string G = "Update Diaster Set CoverCity=@CoverCity where DiasterName=@DiasterName";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@DiasterName", DiasterName);
                Q.Parameters.AddWithValue("@CoverCity", CoverCity);
                Q.ExecuteNonQuery();
                X.Close();
                return ("已設定資料");
            }
            catch (Exception)
            {
                X.Close();
                return ("未設定資料");
            }
        }

        public String Update(string Id, string DiasterName, string DiasterType, string StartTime, string EndTime, string Comment,string DiasterState)
        {
            try
            {
                X.Open();
                string G = "Update Diaster Set DiasterName=@DiasterName,DiasterType=@DiasterType, StartTime=@StartTime, EndTime=@EndTime, Comment=@Comment,DiasterState=@DiasterState where  Id=@Id";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@Id", Id);
                Q.Parameters.AddWithValue("@DiasterName", DiasterName);
                Q.Parameters.AddWithValue("@DiasterType", DiasterType);
                Q.Parameters.AddWithValue("@StartTime", StartTime);
                Q.Parameters.AddWithValue("@EndTime", EndTime);
                Q.Parameters.AddWithValue("@Comment", Comment);
                Q.Parameters.AddWithValue("@DiasterState", DiasterState);
                Q.ExecuteNonQuery();
                X.Close();
                return ("已更新資料");
            }
            catch (Exception)
            {
                X.Close();
                return ("未更新資料");
            }
        }

    }
}