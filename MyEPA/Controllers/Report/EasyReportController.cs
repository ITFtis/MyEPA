using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Office.Interop.Word;
using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.Services;
using NPOI.HSSF.UserModel;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using Spire.Pdf.Graphics;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Xceed.Words.NET;

namespace MyEPA.Controllers.Report
{
    public class EasyReportController : LoginBaseController
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: EasyReport
        //public ActionResult Index()
        //{
        //    return View();
        //}

        /// <summary>
        /// 緊急應變統計表
        /// </summary>
        /// <returns></returns>
        public ActionResult DisasterResponseReport()
        {
            if (!GetIsAdmin())
                return RedirectToAction("LoginRedirect", "Home");

            return View();
        }

        /// <summary>
        /// 匯出(簡報產製1_緊急應變統計表)
        /// </summary>
        public ActionResult Export1_Urgent()
        {
            string toPath = EasyReport.Export1_Urgent();

            if (toPath != "")
            {
                //讀成串流
                var iStream = new FileStream(toPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                //回傳出檔案
                return File(iStream, GetContentType("xlsx"), Path.GetFileName(toPath));
            }
            else
            {
                ViewBag.Msg = "執行失敗：" + "簡報產製1_緊急應變統計表";
                return View("DisasterResponseReport");
            }
        }

        /// <summary>
        /// 匯出(簡報產製5_環境消毒物資)
        /// </summary>
        public ActionResult Export5_EnvDisinSupplies()
        {
            string toPath = EasyReport.Export5_EnvDisinSupplies();

            if (toPath != "")
            {
                //讀成串流
                var iStream = new FileStream(toPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                //回傳出檔案
                return File(iStream, GetContentType("xlsx"), Path.GetFileName(toPath));
            }
            else
            {
                ViewBag.Msg = "執行失敗：" + "(範本)簡報產製5_環境消毒物資.xlsx";
                return View("DisasterResponseReport");
            }
        }
    }
}