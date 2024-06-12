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

        public ActionResult DisasterResponseReport()
        {
            if (!GetIsAdmin())
                return RedirectToAction("LoginRedirect", "Home");

            return View();
        }
    }
}