using MyEPA.Extensions;
using MyEPA.Helper;
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
    public class PolymerDetailController : LoginBaseController
    {
        PolymerDetailService PolymerDetailService = new PolymerDetailService();
        PolymerService PolymerService = new PolymerService();
        public ActionResult Index(int? polymerId = null)
        {
            if (polymerId.HasValue == false)
            {
                return RedirectToMainIndex();
            }

            if(PolymerService.IsExists(polymerId.Value) == false)
            {
                return RedirectToMainIndex();
            }

            ViewBag.PolymerId = polymerId.Value;

            var result = PolymerDetailService.GetByPolymerId(polymerId.Value);

            return View(result);
        }

        public ActionResult Create(int polymerId)
        {
            var now = DateTimeHelper.GetCurrentTime();
            return View(new PolymerDetailModel 
            { 
                PolymerId = polymerId,
                UseDate = now
            });
        }

        [HttpPost]
        public ActionResult Create(PolymerDetailModel model)
        {
            PolymerDetailService.Create(GetUserBrief(),model);
            return RedirectToIndex(model.PolymerId);
        }

        public ActionResult Edit(int id)
        {
            var result = PolymerDetailService.Get(id);

            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(PolymerDetailModel model)
        {
            PolymerDetailService.Update(GetUserBrief(), model);
            return RedirectToIndex(model.PolymerId);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            AdminResultModel result = PolymerDetailService.Delete(id);
            return JsonResult(result);
        }
        public ActionResult Search(int? polymerId = null)
        {
            if (polymerId.HasValue == false)
            {
                return RedirectToMainSearch();
            }

            if (PolymerService.IsExists(polymerId.Value) == false)
            {
                return RedirectToMainSearch();
            }

            ViewBag.PolymerId = polymerId.Value;

            var result = PolymerDetailService.GetByPolymerId(polymerId.Value);

            return View(result);
        }
        private RedirectToRouteResult RedirectToIndex(int polymerId)
        {
            return RedirectToAction("Index",new { polymerId });
        }

        private RedirectToRouteResult RedirectToMainIndex()
        {
            return RedirectToAction("Index", "Polymer",new { });
        }
        private RedirectToRouteResult RedirectToMainSearch()
        {
            return RedirectToAction("Search", "Polymer", new { });
        }
    }
}