using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MyEPA
{
    public class UserReport
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 匯出Excel_聯絡人登入
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string ExportLoginList()
        {
            string result = "";

            try
            {
                int a = int.Parse("aaa");
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