using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers.Rec
{
    public class OpenContractNewController : LoginBaseController
    {
        OpenContractService OpenContractService = new OpenContractService();
        ResourceTypeService ResourceTypeService = new ResourceTypeService();
        CityService CityService = new CityService();
        TownService TownService = new TownService();

        // GET: OpenContractNew
        public ActionResult Index(int? cityId, int? townId, int? resourceTypeId, int? year)
        {
            bool isEffective = Request.QueryString["isEffective"] == null ? false : bool.Parse(Request.QueryString["isEffective"].ToString());

            OpenContractFilterParameter filter = new OpenContractFilterParameter
            {
                CityIds = cityId.HasValue ? cityId.Value.ToListCollection() : null,
                TownIds = townId.HasValue ? townId.Value.ToListCollection() : null,
                ResourceTypeIds = resourceTypeId.HasValue ? resourceTypeId.Value.ToListCollection() : null,
                Year = year,
                IsEffective = isEffective,
            };

            //編輯或其他的錯誤訊息
            if (TempData["Msg"] != null)
            {
                ViewBag.Msg = TempData["Msg"];
            }

            var user = GetUserBrief();
            var duty = user.Duty;

            switch (duty)
            {
                case DutyEnum.EPA:
                case DutyEnum.Corps:
                case DutyEnum.Team:
                    break;
                default:
                    filter.CityIds = user.CityId.ToListCollection();
                    break;
            }

            var result = OpenContractService.GetCountListByFilter(filter);

            //排序
            result = result.OrderByDescending(a => a.CreateDate)                        
                        .ToList();

            ViewBag.Types = ResourceTypeService.GetList();
            ViewBag.Citys = CityService.GetAll();
            ViewBag.Towns = TownService.GetAll();

            //querystring
            ViewBag.Year = year;
            ViewBag.ResourceTypeId = resourceTypeId;
            ViewBag.CityId = cityId;
            ViewBag.TownId = townId;
            ViewBag.IsEffective = isEffective;

            return View(result);
        }

        public ActionResult Create()
        {
            return View(new OpenContractModel()
            {
                OContractDateBegin = DateTimeHelper.GetCurrentTime(),
                OContractDateEnd = DateTimeHelper.GetCurrentTime(),
                KeyInDate = DateTimeHelper.GetCurrentTime(),
            });
        }

        [HttpPost]
        public ActionResult Create(OpenContractModel model, HttpPostedFileBase file)
        {
            OpenContractService.Create(GetUserBrief(), model, file);
            return RedirectToIndex();
        }

        public ActionResult Edit(int? id)
        {
            if (id.HasValue == false)
            {
                return RedirectToIndex();
            }
            var result = OpenContractService.Get(id.Value);
            if (result == null)
            {
                return RedirectToIndex();
            }
            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(string submitButton, OpenContractViewModel model, HttpPostedFileBase file)
        {
            int type = model.ResourceTypeId;

            ////if (submitButton == "Copy")
            ////{
            ////    //複製來源主約Id
            ////    return CopyOpenContractById(model.Id);
            ////}

            bool done = OpenContractService.Update(GetUserBrief(), model, file);
            if(!done)
            {
                TempData["Msg"] = OpenContractService.ErrorMessage;
            }

            return RedirectToAction("index");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            AdminResultModel result = OpenContractService.Delete(GetUserBrief(), id);
            return JsonResult(result);
        }

        private RedirectToRouteResult RedirectToIndex()
        {
            return RedirectToAction("Index");
        }
    }
}