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
    public class LandfillController : LoginBaseController
    {
        LandfillService LandfillService = new LandfillService();
        CityService CityService = new CityService();
        public ActionResult Report(int? id = null,string city = null)
        {
            var result = LandfillService.GetByFilter(new LandfillFilterParameter 
            {
                City = city
            });

            if(id.HasValue)
            {
                ViewBag.Model = LandfillService.Get(id.Value);
            }
            ViewBag.City = city;
            ViewBag.Citys = CityService.GetAll();
            ViewBag.Result = TempData["Result"];

            return View(result);
        }

        public ActionResult Edit(LandfillModel model)
        {
            AdminResultModel result = LandfillService.Update(model);

            TempData["Result"] = result;

            return RedirectToAction("Report", "Landfill", new {  });
        }
        public ActionResult Confirm()
        {
            var result = LandfillService.Confirm(GetUserBrief());

            return JsonResult(result);
        }
    }
}