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
    public class ToiletCleaningLogController : LoginBaseController
    {
        ToiletCleaningLogService ToiletCleaningLogService = new ToiletCleaningLogService();
        ToiletLocationService ToiletLocationService = new ToiletLocationService();
        DiasterService DiasterService = new DiasterService();
        public ActionResult Index(int? id = null,int ? toiletLocationId = null)
        {
            var diasterIds = ToiletLocationService.GetDiasterIds();
            var diaster = DiasterService.GetByFilter(new DiasterFilterParameter 
            {
                Ids = diasterIds
            });

            int? managementTownId = 0;

            int? diasterId = 0;

            if (id.HasValue)
            {
                ViewBag.Model = ToiletCleaningLogService.Get(id.Value);
            }

            if (toiletLocationId.HasValue == false)
            {
                diasterId = diaster.Select(e => e.Id).FirstOrDefault();

                var toiletLocation = ToiletLocationService.GetByFilter(new ToiletLocationFilterParameter
                {
                    DiasterIds = diasterId.Value.ToListCollection()
                }).FirstOrDefault();

                managementTownId = toiletLocation?.ManagementTownId;

                toiletLocationId = toiletLocation?.Id;
            }
            else
            {
                var toiletLocation = ToiletLocationService.Get(toiletLocationId.Value);

                managementTownId = toiletLocation?.ManagementTownId;

                toiletLocationId = toiletLocation?.Id;

                diasterId = toiletLocation?.DiasterId;
            }

            ViewBag.Diasters = diaster;

            ViewBag.DiasterId = diasterId;

            ViewBag.ManagementTownId = managementTownId;

            ViewBag.ToiletLocationId = toiletLocationId;

            if(toiletLocationId.HasValue == false)
            {
                return View(new List<ToiletCleaningLogModel>());
            }

            var result = ToiletCleaningLogService.GetByFilter(new ToiletCleaningLogFilterParameter 
            {
                ToiletLocationIds = toiletLocationId.Value.ToListCollection()
            });

            return View(result);
        }
        public ActionResult CreateOrEdit(ToiletCleaningLogModel model)
        {
            model.UpdateUser = GetUserName();
            AdminResultModel result = ToiletCleaningLogService.CreateOrUpdate(model);

            TempData["Result"] = result;

            return RedirectToAction("Index", "ToiletCleaningLog", new { model.ToiletLocationId });
        }
        public ActionResult Delete(int id)
        {
            ToiletCleaningLogModel model = ToiletCleaningLogService.Get(id);
            if (model == null)
            {
                return JsonResult(new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "資料不存在"
                });
            }
            AdminResultModel result = ToiletCleaningLogService.Delete(id);
            return JsonResult(result);
        }
        public ActionResult Search(int? toiletLocationId = null)
        {
            if (toiletLocationId.HasValue == false)
            {
                return View(new List<ToiletCleaningLogModel>());
            }

            var result = ToiletCleaningLogService.GetByFilter(new ToiletCleaningLogFilterParameter
            {
                ToiletLocationIds = toiletLocationId.Value.ToListCollection()
            });

            ViewBag.ToiletLocationId = toiletLocationId;

            return View(result);
        }
    }
}