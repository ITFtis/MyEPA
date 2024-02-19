using MyEPA.Extensions;
using MyEPA.Models.JsnoModel;
using MyEPA.Services;
using System;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class SystemConfigSettingController : BaseController
    {
        SystemConfigSettingService SystemConfigSettingService = new SystemConfigSettingService();
        DiasterService DiasterService = new DiasterService();
        public ActionResult Index(int diasterId = 0)
        {
            var result = SystemConfigSettingService.GetTaiwanMapReport(diasterId);

            return JsonResult(result);
        }
        public ActionResult Test()
        {
            ViewBag.Diasters = DiasterService.GetAll();
            return PartialView();
        }

    }
}