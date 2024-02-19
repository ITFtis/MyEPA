using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ApplyDisinfectionEquipmentController : ApplyBaseController<ApplyDisinfectionEquipmentModel, ApplyDisinfectionEquipmentViewModel>
    {
        public ApplyDisinfectionEquipmentController()
        {
            BaseApplyService = new ApplyDisinfectionEquipmentService();
        }
        [HttpGet]
        public override ActionResult Processing(int id)
        {
            ViewBag.Options = GetTypes();
            ViewBag.HandlingSituations = ((ApplyDisinfectionEquipmentService)BaseApplyService).GetHandlingSituations(id);

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
            ViewBag.Options = GetTypes();
            ViewBag.HandlingSituations = ((ApplyDisinfectionEquipmentService)BaseApplyService).GetHandlingSituations(id);
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
        public ActionResult UpdateApplyDisinfectionEquipmentEpaStatus(ApplyDisinfectionEquipmentUpdateStatusViewModel request)
        {
            var result = ((ApplyDisinfectionEquipmentService)BaseApplyService).UpdateApplyDisinfectionEquipmentEpaStatus(request);
            if (result)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
        [HttpPost]
        public ActionResult UpdateApplyDisinfectionEquipmentEpbStatus(ApplyDisinfectionEquipmentUpdateStatusViewModel request)
        {
            var result = ((ApplyDisinfectionEquipmentService)BaseApplyService).UpdateApplyDisinfectionEquipmentEpbStatus(request);
            if (result)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
        [HttpPost]
        public ActionResult CreateModel(ApplyDisinfectionEquipmentModel model, List<ApplyDisinfectionEquipmentDetailModel> details, HttpPostedFileBase file, ApplyRequestViewModel requestViewModel)
        {
            var user = GetUserBrief();
            model.Details = details;
            BaseApplyService.Create(user, model, file, requestViewModel);

            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult EditModel(ApplyDisinfectionEquipmentModel model, List<ApplyDisinfectionEquipmentDetailModel> details, HttpPostedFileBase file, ApplyRequestViewModel requestViewModel)
        {
            var user = GetUserBrief();

            model.Details = details;

            BaseApplyService.Edit(user, model, file);
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }
        protected override Dictionary<string, int> GetTypes()
        {
            var dictionary = new Dictionary<string, int>()
            {
                { "請選擇",0 }
            };
            
            foreach (var enumObj in ExtensionsOfEnum.GetEnumAllValue<ApplyDisinfectionEquipmentHandlingSituationTypeEnum>())
            {
                if (enumObj == 0)
                {
                    continue;
                }
                dictionary.Add(enumObj.GetDescription(), enumObj.ToInteger());
            }

            return dictionary;
        }
    }
}