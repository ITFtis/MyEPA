using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Services;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ToiletLocationController : LoginBaseController
    {
        ToiletLocationService ToiletLocationService = new ToiletLocationService();
        CityService CityService = new CityService();
        TownService TownService = new TownService();
        DiasterService DiasterService = new DiasterService();
        public ActionResult Index(int? id = null,int? diasterId = null, int? cityId = null)
        {
            var diaster = DiasterService.GetAll();
            if (diasterId.HasValue == false)
            {
                diasterId = diaster.OrderByDescending(e => e.Id).Select(e => e.Id).FirstOrDefault();
            }
            var result = ToiletLocationService.GetByFilter(new ToiletLocationFilterParameter 
            {
                CityIds = cityId.HasValue ? cityId.Value.ToListCollection() : new List<int>(),
                DiasterIds = diasterId.Value.ToListCollection()
            });

            if(id.HasValue)
            {
                ViewBag.Model = ToiletLocationService.Get(id.Value);
            }
            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = diaster;
            ViewBag.CityId = cityId;
            ViewBag.Citys = CityService.GetAll();
            ViewBag.Towns = TownService.GetAll();
            ViewBag.Result = TempData["Result"];

            return View(result);
        }
        public ActionResult Statistics(int? diasterId = null,int? cityId = null)
        {
            var diasterIds = ToiletLocationService.GetDiasterIds();
            var diaster = DiasterService.GetByFilter(new DiasterFilterParameter());
            if (diasterId.HasValue == false)
            {
                diasterId = diaster.OrderByDescending(e => e.Id).Select(e => e.Id).FirstOrDefault();
            }
            ViewBag.Diasters = diaster;
            ViewBag.DiasterId = diasterId;
            ViewBag.CityId = cityId;
            ViewBag.Citys = CityService.GetAll();

            if (diasterId.HasValue == false)
            {
                return View(new List<ToiletLocationStatisticsViewModel>());
            }
            var result = ToiletLocationService.GetStatisticsByFilter(new ToiletLocationFilterParameter
            {
                CityIds = cityId.HasValue ? cityId.Value.ToListCollection() : new List<int>(),
                DiasterIds = diasterId.Value.ToListCollection()
            });

            return View(result);
        }
        public ActionResult CreateOrEdit(ToiletLocationModel model)
        {
            model.UpdateUser = GetUserName();
            AdminResultModel result = ToiletLocationService.CreateOrUpdate(model);

            TempData["Result"] = result;

            return RedirectToAction("Index", "ToiletLocation", new { model.DiasterId });
        }
        public ActionResult Delete(int id)
        {
            ToiletLocationModel model = ToiletLocationService.Get(id);
            if(model == null)
            {
                return JsonResult(new AdminResultModel 
                {
                    IsSuccess = false,
                    ErrorMessage = "資料不存在"
                });
            }
            AdminResultModel result = ToiletLocationService.Delete(id);
            return JsonResult(result);
        }
    }
}