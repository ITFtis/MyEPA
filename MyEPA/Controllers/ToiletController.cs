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
    public class ToiletController : LoginBaseController
    {
        ToiletService ToiletService = new ToiletService();

        public ActionResult Search(ToiletFilterParameter filter)
        {
            var result = ToiletService.GetByFilter(filter);

            ViewBag.Summary = ToiletService.GetReportByFilter(filter);

            return View(result);
        }
    }
}