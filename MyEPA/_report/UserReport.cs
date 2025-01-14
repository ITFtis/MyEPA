using MyEPA.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace MyEPA
{
    public class UserReport
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 匯出Excel_聯絡人登入清單
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string ExportLoginList(List<UserLoginViewModel> datas)
        {
            string result = "";

            try
            {
                string fileTitle = "聯絡人登入清單";
                string folder = HttpContext.Current.Server.MapPath("~/FileDatas/Temp/");               

                //產出Dynamic資料 (給Excel)
                List<dynamic> list = new List<dynamic>();

                int serial = 1;
                foreach (var data in datas)
                {
                    dynamic f = new ExpandoObject();
                    f.序號 = serial;
                    serial++;
                    f.機關類別 = data.Duty;
                    f.機關名稱_縣市 = data.City;
                    f.單位名稱_鄉鎮 = data.Town;
                    f.帳號 = data.UserName;   //ooooooooooo
                    f.姓名 = data.Name == null ? "" : data.Name;   //ooooooooooo
                    f.Email = data.Email == null ? "": data.Email;
                    f.最後登入時間 = data.LoginTime == null ? "無" : DateFormat.ToDate7(data.LoginTime);
                    f.未登入天數 = data.LoginTime == null ? "無" : data.LoginRange.ToString();

                    f.SheetName = fileTitle;//sheep.名稱;
                    list.Add(f);
                }

                //查無符合資料表數
                if (list.Count == 0)
                {
                    logger.Error("執行錯誤：匯出Excel_聯絡人登入");
                    return "";
                }

                List<string> titles = new List<string>();

                //"0":不調整width,"1":自動調整長度(效能差:資料量多),"2":字串長度調整width,"3":字串長度調整width(展開)
                int autoSizeColumn = 2;

                //產出excel
                string fileName = ExcelSpecHelper.GenerateExcelByLinqF1(fileTitle, titles, list, folder, autoSizeColumn);

                string path = folder + fileName;

                result = path;
            }
            catch (Exception ex)
            {
                logger.Error("執行錯誤：匯出Excel_聯絡人登入");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }

            return result;
        }
    }
}