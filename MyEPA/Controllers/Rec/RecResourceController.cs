using MyEPA.Models;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class RecResourceController : LoginBaseController
    {
        DiasterService DiasterService = new DiasterService();
        RecResourceService RecResourceService = new RecResourceService();
        //CityService CityService = new CityService();

        // GET: RecResource
        public ActionResult Index(int? diasterId = null)
        {
            List<DiasterModel> diasters = DiasterService.GetAll();

            if (diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }

            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = diasters;

            if (diasterId.HasValue == false)
            {
                return View(new List<WaterCheckModel>());
            }

            List<RecResourceModel> result =
                RecResourceService.GetByDiasterId(diasterId.Value, GetUserBrief());


            return View(result);
        }
    }
}