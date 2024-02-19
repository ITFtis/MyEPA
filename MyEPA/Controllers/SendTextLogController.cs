using MyEPA.Extensions;
using MyEPA.Models.FilterParameter;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class SendTextLogController : LoginBaseController
    {
        SendTextLogService SendTextLogService = new SendTextLogService();

        public ActionResult Index()
        {
            var result = SendTextLogService.GetTextLogs();

            return View(result);
        }

        public ActionResult Details(int sendTextLogId)
        {
            var result = SendTextLogService.GetLogDetails(sendTextLogId);

            return View(result);
        }
    }
}