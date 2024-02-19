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
    public class PolymerController : LoginBaseController
    {
        PolymerService PolymerService = new PolymerService();
        DiasterService DiasterService = new DiasterService();
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
                return View(new List<PolymerModel>());
            }

            var result = PolymerService.GetByDiasterId(diasterId.Value);

            return View(result);
        }

        public ActionResult Create(int diasterId)
        {
            var now = DateTimeHelper.GetCurrentTime();
            return View(new PolymerModel 
            { 
                DiasterId = diasterId,
                BuyDate = now,
                EffectiveDate = now
            });
        }

        [HttpPost]
        public ActionResult Create(PolymerModel model)
        {
            PolymerService.Create(GetUserBrief(),model);
            return RedirectToIndex(model.DiasterId);
        }

        public ActionResult Edit(int id)
        {
            var result = PolymerService.Get(id);


            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(PolymerModel model)
        {
            PolymerService.Update(GetUserBrief(), model);
            return RedirectToIndex(model.DiasterId);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            AdminResultModel result = PolymerService.Delete(id);
            return JsonResult(result);
        }

        public ActionResult Search(int? diasterId = null)
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
                return View(new List<PolymerModel>());
            }

            var result = PolymerService.GetByDiasterId(diasterId.Value);

            return View(result);
        }
        private RedirectToRouteResult RedirectToIndex(int? diasterId = null)
        {
            return RedirectToAction("Index",new { diasterId });
        }
    }
}