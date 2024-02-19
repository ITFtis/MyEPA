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
    public class VehicleModel
    {
        public SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());
        [AutoKey]
        public int Id { get; set; }
        [DisplayName("車牌號碼")]
        public string PlateNumber {get;set;}
        [DisplayName("縣市")]
        public string City { get; set; }
        [DisplayName("鄉鎮名")]
        public string Town { get; set; }
        [DisplayName("部門")]
        public string ContactUnit { get; set; }
        [DisplayName("車輛名稱")]
        public string VehicleName { get; set; }
        [DisplayName("車輛設備現況")]
        public string VehicleState { get; set; }
        [DisplayName("資料更新日期")]
        public DateTime UpdateTime { get; set; }
        [DisplayName("載重(噸)")]
        public string Load { get; set; }
        [DisplayName("馬力(HP)")]
        public string EnginePower { get; set; }
        [DisplayName("購置年份")]
        public string ROCyear { get; set; }
        [DisplayName("是否為環保署補助購置")]
        public string EPAsubsidy { get; set; }
        [DisplayName("可否提供環保署跨縣市調度支援")]
        public string CrossCityUse { get; set; }
        [DisplayName("可否提供縣府跨鄉鎮調度支援")]
        public string CrossTownUse { get; set; }
        [DisplayName("備註")]
        public string Note { get; set; }
        [DisplayName("X")]
        public decimal Xpos { get; set; }
        [DisplayName("Y")]
        public decimal Ypos { get; set; }
        [DisplayName("車輛類別")]
        public string VehicleType { get; set; }
        [DisplayName("用途")]
        public string Purpose { get; set; }

        public DateTime? ConfirmTime { get; set; }

        public LinkedList<VehicleModel> Show(string City, string Town)
        {
            VehicleModel A = new VehicleModel();
            LinkedList<VehicleModel> C = new LinkedList<VehicleModel>();
            string G;
            try
            {
                X.Open(); SqlCommand Q;
                if ( (City == "ALL") && (Town=="ALL"))
                {
                    G = "Select * from [Vehicle]";
                    Q = new SqlCommand(G, X);
                    System.Diagnostics.Debug.WriteLine("****************************************");
                    System.Diagnostics.Debug.WriteLine("A");
                }
                else if ((City == "ALL") && (Town != "ALL"))
                {
                    G = "Select * from [Vehicle] where Town=@Town";
                    Q = new SqlCommand(G, X);
                    Q.Parameters.AddWithValue("@Town", Town);
                    System.Diagnostics.Debug.WriteLine("****************************************");
                    System.Diagnostics.Debug.WriteLine("B");
                }
                else if ((City != "ALL") && (Town == "ALL"))
                {
                    G = "Select * from [Vehicle] where City=@City";
                    Q = new SqlCommand(G, X);
                    Q.Parameters.AddWithValue("@City", City);
                    System.Diagnostics.Debug.WriteLine("****************************************");
                    System.Diagnostics.Debug.WriteLine("C");
                }
                else
                {
                    G = "Select * from [Vehicle] where City=@City And Town=@Town";
                    Q = new SqlCommand(G, X);
                    Q.Parameters.AddWithValue("@City", City);
                    Q.Parameters.AddWithValue("@Town", Town);
              
                }
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                while (R.Read() == true)
                {
                    A.PlateNumber = Convert.ToString(R["PlateNumber"]);
                    A.ContactUnit = Convert.ToString(R["ContactUnit"]);
                    A.City = Convert.ToString(R["City"]);
                    A.Town = Convert.ToString(R["Town"]);
                    A.ContactUnit = Convert.ToString(R["ContactUnit"]);
                    A.VehicleName = Convert.ToString(R["VehicleName"]);
                    A.VehicleState = Convert.ToString(R["VehicleState"]);
                    A.UpdateTime = Convert.ToDateTime(R["UpdateTime"]);
                    A.Load = Convert.ToString(R["Load"]);
                    A.EnginePower = Convert.ToString(R["EnginePower"]);
                    A.ROCyear = Convert.ToString(R["ROCyear"]);
                    A.EPAsubsidy = Convert.ToString(R["EPAsubsidy"]);
                    A.CrossCityUse = Convert.ToString(R["CrossCityUse"]);
                    A.CrossTownUse = Convert.ToString(R["CrossTownUse"]);
                    A.Note = Convert.ToString(R["Note"]);
                    A.Xpos = Convert.ToDecimal(R["Xpos"]);
                    A.Ypos = Convert.ToDecimal(R["Ypos"]);
                    C.AddFirst(A);
                    System.Diagnostics.Debug.WriteLine("******dddd");
                    System.Diagnostics.Debug.WriteLine(A.City);
                    System.Diagnostics.Debug.WriteLine(A.Town);
                    A = null;
                    A = new VehicleModel();
                }
            }
            catch (Exception)
            {
                A.ContactUnit = "連通失敗";
                C.AddFirst(A);

            }
            finally { X.Close(); }
            return C;
        }

        public LinkedList<VehicleModel> Show(string City)
        {
            VehicleModel A = new VehicleModel();
            LinkedList<VehicleModel> C = new LinkedList<VehicleModel>();
            string G;
            try
            {
                X.Open();
                if (City == "ALL")
                {
                    G = "Select * from [Vehicle]";
                }
                else
                {
                    G = "Select * from [Vehicle] where City=@City";
                }
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@City", City);
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                while (R.Read() == true)
                {
                    A.PlateNumber = Convert.ToString(R["PlateNumber"]);
                    A.ContactUnit= Convert.ToString(R["ContactUnit"]);
                    A.City= Convert.ToString(R["City"]);
                    A.Town = Convert.ToString(R["Town"]);
                    A.ContactUnit = Convert.ToString(R["ContactUnit"]);
                    A.VehicleName = Convert.ToString(R["VehicleName"]);
                    A.VehicleState = Convert.ToString(R["VehicleState"]);
                    A.UpdateTime = Convert.ToDateTime(R["UpdateTime"]);
                    A.Load = Convert.ToString(R["Load"]);
                    A.EnginePower = Convert.ToString(R["EnginePower"]);
                    A.ROCyear = Convert.ToString(R["ROCyear"]);
                    A.EPAsubsidy = Convert.ToString(R["EPAsubsidy"]);
                    A.CrossCityUse = Convert.ToString(R["CrossCityUse"]);
                    A.CrossTownUse = Convert.ToString(R["CrossTownUse"]);
                    A.Note = Convert.ToString(R["Note"]);
                    A.Xpos = Convert.ToDecimal(R["Xpos"]);
                    A.Ypos = Convert.ToDecimal(R["Ypos"]);
                    C.AddFirst(A);
                    A = null;
                    A = new VehicleModel();
                }
            }
            catch (Exception)
            {
                A.ContactUnit = "連通失敗";
                C.AddFirst(A);
 
            }
            finally { X.Close(); }
            return C;
        }
    }
}