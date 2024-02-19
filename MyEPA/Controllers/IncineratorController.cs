using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class IncineratorController : LoginBaseController
    {
        IncineratorService IncineratorService = new IncineratorService();
        CityService CityService = new CityService();
        public ActionResult Report(int? id = null, string city = null)
        {
            var result = IncineratorService.GetByFilter(new IncineratorFilterParameter
            {
                City = city
            });

            if (id.HasValue)
            {
                ViewBag.Model = IncineratorService.Get(id.Value);
            }
            ViewBag.City = city;
            ViewBag.Citys = CityService.GetAll();
            ViewBag.Result = TempData["Result"];

            return View(result);
        }

        public ActionResult Edit(IncineratorModel model)
        {
            AdminResultModel result = IncineratorService.Update(model);

            TempData["Result"] = result;

            return RedirectToAction("Report", "Incinerator", new {  });
        }
        public ActionResult Confirm()
        {
            var result = IncineratorService.Confirm(GetUserBrief());

            return JsonResult(result);
        }
    }
}