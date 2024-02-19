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
    public class WaterEquipmentController : LoginBaseController
    {
        WaterEquipmentService WaterEquipmentService = new WaterEquipmentService();
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
                return View(new List<WaterEquipmentViewModel>());
            }

            var result = WaterEquipmentService.GetByDiasterId(diasterId.Value);

            return View(result);
        }

        public ActionResult Create(int diasterId)
        {
            var now = DateTimeHelper.GetCurrentTime();
            return View(new WaterEquipmentModel 
            { 
                DiasterId = diasterId,
                DoneDate = now,
            });
        }

        [HttpPost]
        public ActionResult Create(WaterEquipmentModel model)
        {
            WaterEquipmentService.Create(GetUserBrief(),model);
            return RedirectToIndex(model.DiasterId);
        }

        public ActionResult Edit(int id)
        {
            var result = WaterEquipmentService.Get(id);


            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(WaterEquipmentModel model)
        {
            WaterEquipmentService.Update(GetUserBrief(), model);
            return RedirectToIndex(model.DiasterId);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            AdminResultModel result = WaterEquipmentService.Delete(id);
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
                return View(new List<WaterEquipmentModel>());
            }

            var result = WaterEquipmentService.GetByDiasterId(diasterId.Value);

            return View(result);
        }
        private RedirectToRouteResult RedirectToIndex(int diasterId)
        {
            return RedirectToAction("Index",new { diasterId });
        }
    }
}