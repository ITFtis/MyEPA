using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ApplyMedicineController : ApplyBaseController<ApplyMedicineModel, ApplyMedicineViewModel>
    {
        public ApplyMedicineController()
        {
            BaseApplyService = new ApplyMedicineService();
        }
        [HttpGet]
        public override ActionResult Processing(int id)
        {
            ViewBag.HandlingSituations = ((ApplyMedicineService)BaseApplyService).GetHandlingSituations(id);
            ViewBag.Options = GetTypes();
            var viewModel = BaseApplyService.GetApplySupportProcessingDetailViewModel(id);
            if (viewModel != null)
            {
                viewModel.CityName = CityService.GetCiyNameByCityId(viewModel.CityId);
                viewModel.TownName = TownService.GetTownNameByTownId(viewModel.TownId);
                ViewData.Model = viewModel;
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View();
        }
        [HttpGet]
        public override ActionResult EPAProcessing(int id)
        {
            ViewBag.HandlingSituations = ((ApplyMedicineService)BaseApplyService).GetHandlingSituations(id);
            ViewBag.Options = GetTypes();
            var viewModel = BaseApplyService.GetApplySupportProcessingDetailViewModel(id);
            if (viewModel != null)
            {
                viewModel.CityName = CityService.GetCiyNameByCityId(viewModel.CityId);
                viewModel.TownName = TownService.GetTownNameByTownId(viewModel.TownId);
                ViewData.Model = viewModel;
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View();
        }
        [HttpPost]
        public ActionResult UpdateApplyMedicineEpaStatus(ApplyMedicineUpdateStatusViewModel request)
        {
            var result = ((ApplyMedicineService)BaseApplyService).UpdateApplyMedicineEpaStatus(request);
            if (result)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
        [HttpPost]
        public ActionResult UpdateApplyMedicineEpbStatus(ApplyMedicineUpdateStatusViewModel request)
        {
            var result = ((ApplyMedicineService)BaseApplyService).UpdateApplyMedicineEpbStatus(request);
            if (result)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
        [HttpGet]
        public override ActionResult Create(ApplyRequestViewModel requestViewModel)
        {
            ViewBag.Options = GetTypes();
            return base.Create(requestViewModel);
        }

        [HttpPost]
        public ActionResult CreateModel(ApplyMedicineModel model, List<ApplyMedicineDetailModel> details, HttpPostedFileBase file, ApplyRequestViewModel requestViewModel)
        {
            var user = GetUserBrief();

            model.Details = details;

            BaseApplyService.Create(user, model, file, requestViewModel);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public override ActionResult Edit(int id, ApplyRequestViewModel requestViewModel)
        {
            ViewBag.Options = GetTypes();
            return base.Edit(id, requestViewModel);
        }
        [HttpPost]
        public ActionResult EditModel(ApplyMedicineModel model, List<ApplyMedicineDetailModel> details, HttpPostedFileBase file, ApplyRequestViewModel requestViewModel)
        {
            var user = GetUserBrief();

            model.Details = details;

            BaseApplyService.Edit(user, model, file);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public ActionResult MedicineOptions()
        {
           
            return JsonResult(GetTypes());
        }

        protected override Dictionary<string, int> GetTypes()
        {
            var dictionary = new Dictionary<string, int>()
            {
                { "請選擇",0 }
            };


            foreach (ApplyMedicineTypeEnum enumObj in Enum.GetValues(typeof(ApplyMedicineTypeEnum)))
            {
                dictionary.Add(enumObj.GetDescription(), enumObj.ToInteger());
            }

            return dictionary;
        }
    }
}