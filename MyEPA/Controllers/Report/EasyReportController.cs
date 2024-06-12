using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        /// 匯出緊急應變統計表
        /// </summary>
        public ActionResult ExportDisasterResponseReport()
        {
            return RedirectToAction("DisasterResponseReport");
        }
    }
}