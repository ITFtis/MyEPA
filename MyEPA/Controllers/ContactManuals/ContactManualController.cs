using MyEPA.Helper;
using MyEPA.Services;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace MyEPA.Controllers
{
    public class ContactManualController : LoginBaseController
    {
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ReportOfPdf()
        {
            var user = GetUserBrief();
            var result = new ContactManualReportService().ReportOfPDFNew(user);
            //var fileName = $"{DateTimeHelper.GetCurrentTime().ToString("yyyyMMdd")}.pdf";
            //var path = "~";
            var fullPath =  Server.MapPath("/"); 
            //Path.GetFullPath(path);
            string fileName = string.Format("環保署111年度環境污染事故(含春節期間)緊急應變摘要及通聯手冊_New.pdf", fullPath);
            return File(result, fileName);
        }
    }
}