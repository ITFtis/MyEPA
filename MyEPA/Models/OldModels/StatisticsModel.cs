using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Web.Configuration;
namespace MyEPA.Models
{

    public class StatisticsModel
    {
        public SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());

        //以下方法利用BubbleSort來排序，後來沒用到，直接在Controller寫
        public int[] SortCityByNumber(int[] B)
        {
            int[] A = new int[23] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,17, 18, 19, 20, 21, 22 };
            for (int j = 22; j > 1; j--)
            {
                for (int i = 1; i < j; i++)
                {
                    if (B[i] < B[i + 1])
                    {
                        int TempA = A[i];
                        int TempB = B[i];
                        A[i] = A[i + 1]; B[i] = B[i + 1];
                        A[i + 1] = TempA;B[i + 1] = TempB;
                    }
                }
            }
            return A;
        }

        public string StoreNewestCountValue(string TblName, string City,string Count)
        {
            string G = "";
            try
            {
                X.Open();
                switch (TblName)
                {
                    case "ConfirmTime":
                        G = "Update NewestCountValue Set ConfirmTime =@Count where City=@City";
                        break;
                    case "UpdateTime":
                        G = "Update NewestCountValue Set UpdateTime =@Count where City=@City";
                        break;
                    case "Dump":
                        G = "Update NewestCountValue Set Dump =@Count where City=@City";
                        break;
                    case "Disinfector":
                        G = "Update NewestCountValue Set Disinfector =@Count where City=@City";
                        break;
                    case "Disinfectant":
                        G = "Update NewestCountValue Set Disinfectant =@Count where City=@City";
                        break;
                    case "SolidDisinfectant":
                        G = "Update NewestCountValue Set SolidDisinfectant =@Count where City=@City";
                        break;
                    case "LiquidDisinfectant":
                        G = "Update NewestCountValue Set LiquidDisinfectant =@Count where City=@City";
                        break;
                    case "Toilet":
                        G = "Update NewestCountValue Set Toilet =@Count where City=@City";
                        break;
                    case "Users":
                        G = "Update NewestCountValue Set Users =@Count where City=@City";
                        break;
                    case "Pest":
                        G = "Update NewestCountValue Set Pest =@Count where City=@City";
                        break;
                    case "Vehicle":
                        G = "Update NewestCountValue Set Vehicle =@Count where City=@City";
                        break;
                    case "Volunteer":
                        G = "Update NewestCountValue Set Volunteer =@Count where City=@City";
                        break;
                    default: break;
                }
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@City", City);
                Q.Parameters.AddWithValue("@Count",Count);
                Q.ExecuteNonQuery();
                X.Close(); return "Ok";
            }
            catch (Exception)
            { X.Close(); return "NotOk"; }
        }

        public string StoreConfirmTime(string TblName, string City)
        {     
            string G = "";
            try
            {
                X.Open();
                switch (TblName)
                {
                    case "Dump":
                        G = "Update ConfirmTime Set Dump =@UpdateTime where City=@City";
                        break;
                    case "Disinfector":
                        G = "Update ConfirmTime Set Disinfector =@UpdateTime where City=@City";
                        break;
                    case "Disinfectant":
                        G = "Update ConfirmTime Set Disinfectant =@UpdateTime where City=@City";
                        break;
                    case "Toilet":
                        G = "Update ConfirmTime  Set Toilet =@UpdateTime where City=@City";
                        break;
                    //user 是SQL關鍵字，無法使用，改為Users
                    case "Users":
                        G = "Update ConfirmTime Set Users =@UpdateTime where City=@City";
                        break;
                    case "Pest":
                        G = "Update ConfirmTime  Set Pest =@UpdateTime where City=@City";
                        break;
                    case "Landfill":
                        G = "Update ConfirmTime Set Landfill =@UpdateTime where City=@City";
                        break;
                    case "Incinerator":
                        G = "Update ConfirmTime  Set Incinerator =@UpdateTime where City=@City";
                        break;
                    case "District":
                        G = "Update ConfirmTime  Set District =@UpdateTime where City=@City";
                        break;
                    case "Volunteer":
                        G = "Update ConfirmTime  Set Volunteer =@UpdateTime where City=@City";
                        break;
                    default: break;
                }
                SqlCommand Q = new SqlCommand(G, X);
                var TaipeiTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
                DateTime TaipeiLocalTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TaipeiTimeZoneInfo);
                Q.Parameters.AddWithValue("@City", City);
                Q.Parameters.AddWithValue("@UpdateTime", TaipeiLocalTime.ToString("yyyy/MM/dd HH: mm:ss"));
                Q.ExecuteNonQuery();
                X.Close();

                //以下是把最近確認時間存到NewCountValue的表格
                StatisticsModel A = new StatisticsModel();
                string Msg = A.StoreNewestCountValue("ConfirmTime", City, TaipeiLocalTime.ToString("yyyy/MM/dd HH: mm:ss"));
                return "資料已確認";
            }
            catch (Exception)
            { X.Close(); return "資料未確認"; }
        }

        public string StoreNewestUpdateTime(string TblName, string City)
        {
            string G = "";
            try
            {
                X.Open();
                switch (TblName)
                {
                   
                    case "Dump":
                        G = "Update NewestUpdateTime Set Dump =@UpdateTime where City=@City";
                        break;
                    case "Disinfector":
                        G = "Update NewestUpdateTime Set Disinfector =@UpdateTime where City=@City";
                        break;
                    case "Disinfectant":
                        G = "Update NewestUpdateTime Set Disinfectant =@UpdateTime where City=@City";
                        break;
                    case "Toilet":
                        G = "Update NewestUpdateTime Set Toilet =@UpdateTime where City=@City";
                        break;
                        //user 是SQL關鍵字，無法使用，改為Users
                    case "Users":
                        G = "Update NewestUpdateTime Set Users =@UpdateTime where City=@City";
                        break;
                    case "Pest":
                        G = "Update NewestUpdateTime Set Pest =@UpdateTime where City=@City";
                        break;
                    case "Landfill":
                        G = "Update NewestUpdateTime Set Landfill =@UpdateTime where City=@City";
                        break;
                    case "Incinerator":
                        G = "Update NewestUpdateTime Set Incinerator =@UpdateTime where City=@City";
                        break;
                    case "District":
                        G = "Update NewestUpdateTime  Set District =@UpdateTime where City=@City";
                        break;
                    case "Volunteer":
                        G = "Update NewestUpdateTime  Set Volunteer=@UpdateTime where City=@City";
                        break;
                    default:break;
                }                       
                SqlCommand Q = new SqlCommand(G, X);
                var TaipeiTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
                DateTime TaipeiLocalTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TaipeiTimeZoneInfo);
                Q.Parameters.AddWithValue("@City", City);
                Q.Parameters.AddWithValue("@UpdateTime", TaipeiLocalTime.ToString("yyyy/MM/dd HH: mm:ss"));
                Q.ExecuteNonQuery();            
                X.Close();

                StatisticsModel A = new StatisticsModel();
                string Msg = A.StoreNewestCountValue("UpdateTime", City, TaipeiLocalTime.ToString("yyyy/MM/dd HH: mm:ss"));
                return "Ok";
            }
            catch (Exception)
            { X.Close(); return "NotOk"; }
        }

        public string FindConfirmTime(string TblName, string City)
        {

            string G = ""; string ConfirmTime = string.Empty;
            try
            {
                X.Open();
                G = "Select * from ConfirmTime where City=@City";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@City", City);
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                while (R.Read() == true)
                { ConfirmTime= R[TblName].ToString().Trim(); }
                X.Close(); return ConfirmTime;
            }
            catch (Exception)
            { X.Close(); return ConfirmTime; }
        }

        //以下方法在輸入表格名稱以及城市名稱後，會找到該表格之該城市最後資料，
        //並回傳其建立時間，也就是回傳該城市的最新資料更新時間。

        //但此方法遺落刪除的時間，因此可能造成環境部在統計上，
        //而刪除應該算是更新的一種。因此之後要建立一個表格，內部欄位是各資料表名稱，
        //每次有刪除時，便要在該表格更新。

        public string FindNewestUpdateTime(string TblName, string City)
        {
            string G = "";  string NewestUpdateTime = string.Empty;
            try{
                X.Open();              
                G = "Select * from NewestUpdateTime where City=@City";
                SqlCommand Q = new SqlCommand(G, X);
                Q.Parameters.AddWithValue("@City", City);
                Q.ExecuteNonQuery();
                SqlDataReader R = Q.ExecuteReader();
                while (R.Read() == true)     
                    { NewestUpdateTime = R[TblName].ToString().Trim(); }             
                    X.Close();return NewestUpdateTime;
            } catch (Exception)
            { X.Close(); return NewestUpdateTime; }
        }

        public int[] CountAllCity(string TableName)
        {
            string G = ""; string msg = "";
            int[] CityCount = new int[23];
            for (int i=0; i<23;i++)
            {
                CityCount[i] = 0;
            }
            try
            {
                X.Open();
                switch (TableName)
                {
                    case "Dump":
                        G = "Select * from Dump";
                        break;
                    case "Disinfector":
                        G = "Select * from Disinfector";
                        break;
                    case "SolidDisinfectant":
                        G = "Select * from Disinfectant";
                        break;
                    case "LiquidDisinfectant":
                        G = "Select * from Disinfectant";
                        break;
                    case "Disinfectant":
                        G = "Select * from Disinfectant";
                        break;
                    case "Toilet":
                        G = "Select * from Toilet";
                        break;
                    case "Users":
                        G = "Select * from Users";
                        break;
                    case "Pest":
                        G = "Select * from Pest";
                        break;
                    case "Vehicle":
                        G = "Select * from Vehicle";
                        break;
                    case "District":
                        G = "Select * from District";
                        break;
                    case "Volunteer":
                        G = "Select * from Volunteer";
                        break;
                    default:
                        return CityCount;
                }
                SqlCommand Q = new SqlCommand(G, X);
                Q.ExecuteNonQuery();


                //Convert.ToInt16(x)在x是整數構成的字串時，會回應該整數值
                //在x 是null時，則會回應0;在 x 是空字串時，則會當掉

                int Value = 1;
                
                SqlDataReader R = Q.ExecuteReader();
                string City;
                while (R.Read() == true)
                {
                    City = R["City"].ToString().Trim();
                    switch (TableName)
                    {
                        case "Dump":
                            Value = 1;  //因為廢棄物掩埋場每筆資料只有一處
                            break;
                        case "Disinfector":
                            if (R["Amount"] == null)
                            { Value = 0; }
                            else if (R["Amount"].ToString().Trim() == string.Empty)
                            { Value = 0; }
                            else
                            { Value = Convert.ToInt16(R["Amount"].ToString().Trim()); }
                            //因為每筆資料的消毒設備會有多台，存放於Amount欄位

                            break;
                        case "Disinfectant":

                            if (R["Amount"] == null)
                            { Value = 0; }
                            else if (R["Amount"].ToString().Trim() == string.Empty)
                            { Value = 0; }
                            else
                            { Value = Convert.ToInt16(R["Amount"].ToString().Trim()); }

                            //因為每筆資料的消毒藥水會有很多公升，存放於Amount欄位
                            break;
                        case "SolidDisinfectant":

                            if (R["Amount"] == null)
                            { Value = 0; }
                            else if (R["Amount"].ToString().Trim() == string.Empty)
                            { Value = 0; }
                            else
                            {
                                if (R["DrugState"] == null)
                                { Value = 0; }
                                else if (R["DrugState"].ToString().Trim() == string.Empty)
                                { Value = 0; }
                                else if (R["DrugState"].ToString().Trim() == "固體")
                                {
                                    Value = Convert.ToInt16(R["Amount"].ToString().Trim());
                                }
                                else
                                { Value = 0; }
                            }

                            //因為每筆資料的消毒藥水會有很多公升，存放於Amount欄位
                            break;

                        case "LiquidDisinfectant":

                            if (R["Amount"] == null)
                            { Value = 0; }
                            else if (R["Amount"].ToString().Trim() == string.Empty)
                            { Value = 0; }
                            else
                            {
                              
                                if (R["DrugState"] == null)
                                { Value = 0; }
                                else if (R["DrugState"].ToString().Trim() == string.Empty)
                                { Value = 0; }
                                else if (R["DrugState"].ToString().Trim() == "液體")
                                {
                                    Value = Convert.ToInt16(R["Amount"].ToString().Trim());
                                }
                                else
                                { Value = 0; }
                            }

                            //因為每筆資料的消毒藥水會有很多公升，存放於Amount欄位
                            break;

                        case "Toilet":
                            G = "Select * from Toilet";
                            Value = 1;  //因為流浪廁所每筆資料只有一處
                            break;
                        case "Users":
                            Value = 1;  //因為人員每筆資料只有一個人
                            break;
                        case "Pest":
                            Value = 1;  
                            break;
                        case "Vehicle":
                            Value = 1; 
                            break;
                        case "Volunteer":
                            Value = 1;
                            break;
                        default:
                            return CityCount;
                    }

                    switch (City)
                    {
                        case "基隆市":
                            CityCount[1] = CityCount[1] + Value;
                            break;
                        case "臺北市":
                            CityCount[2] = CityCount[2] + Value;
                            break;
                        case "新北市":
                            CityCount[3] = CityCount[3] + Value;
                            break;
                        case "桃園市":
                            CityCount[4] = CityCount[4] + Value;
                            break;
                        case "新竹縣":
                            CityCount[5] = CityCount[5] + Value;
                            break;
                        case "新竹市":
                            CityCount[6] = CityCount[6] + Value;
                            break;
                        case "苗栗縣":
                            CityCount[7] = CityCount[7] + Value;
                            break;
                        case "臺中市":
                            CityCount[8] = CityCount[8] + Value;
                            break;
                        case "南投縣":
                            CityCount[9] = CityCount[9] + Value;
                            break;
                        case "雲林縣":
                            CityCount[10] = CityCount[10] + Value;
                            break;
                        case "彰化縣":
                            CityCount[11] = CityCount[11] + Value;
                            break;
                        case "嘉義縣":
                            CityCount[12] = CityCount[12] + Value;
                            break;
                        case "嘉義市":
                            CityCount[13] = CityCount[13] + Value;
                            break;
                        case "臺南市":
                            CityCount[14] = CityCount[14] + Value;
                            break;
                        case "高雄市":
                            CityCount[15] = CityCount[15] + Value;
                            break;
                        case "屏東縣":
                            CityCount[16] = CityCount[16] + Value;
                            break;
                        case "臺東縣":
                            CityCount[17] = CityCount[17] + Value;
                            break;
                        case "花蓮縣":
                            CityCount[18] = CityCount[18] + Value;
                            break;
                        case "宜蘭縣":
                            CityCount[19] = CityCount[19] + Value;
                            break;
                        case "澎湖縣":
                            CityCount[20] = CityCount[20] + Value;
                            break;
                        case "金門縣":
                            CityCount[21] = CityCount[21] + Value;
                            break;
                        case "連江縣":
                            CityCount[22] = CityCount[22] + Value;
                            break;
                        default:
                            break;
                    }
                        }
                X.Close();
                string[] CityName = new string[23] {"","基隆市","臺北市","新北市","桃園市","新竹縣","新竹市","苗栗縣","臺中市","南投縣","雲林縣","彰化縣","嘉義縣","嘉義市","臺南市", "高雄市","屏東縣","臺東縣","花蓮縣","宜蘭縣","澎湖縣","金門縣","連江縣"};
                StatisticsModel A = new StatisticsModel();
                for (int i=1; i<23; i++)
                {
                    string Msg = A.StoreNewestCountValue(TableName, CityName[i], CityCount[i].ToString());
                }
                return CityCount;
            }
            catch (Exception)
            { msg = G; X.Close(); return CityCount; }
        }       
    }
}