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
    public class DumpController : LoginBaseController
    {
        DumpService DumpService = new DumpService();

        public ActionResult Search(DumpFilterParameter filter)
        {
            var result = DumpService.GetByFilter(filter);

            return View(result);
        }
        public ActionResult Confirm(int? townId = null)
        {
            var result = DumpService.Confirm(GetUserBrief(), townId);

            return JsonResult(result);
        }
    }
}