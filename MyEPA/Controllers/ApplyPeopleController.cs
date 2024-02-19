using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    /// <summary>
    /// 申請人力資源
    /// </summary>
    public class ApplyPeopleController : ApplyBaseController<ApplyPeopleModel, ApplyPeopleViewModel>
    {
        public ApplyPeopleController() 
        {
            BaseApplyService = new ApplyPeopleService();
        }
        [HttpGet]
        public override ActionResult Processing(int id)
        {
            ViewBag.Options = GetTypes();
            ViewBag.HandlingSituations = ((ApplyPeopleService)BaseApplyService).GetHandlingSituations(id);

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
            ViewBag.HandlingSituations = ((ApplyPeopleService)BaseApplyService).GetHandlingSituations(id);
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
        public ActionResult UpdateApplyPeopleEpaStatus(ApplyPeopleUpdateStatusViewModel request)
        {
            var result = ((ApplyPeopleService)BaseApplyService).UpdateApplyPeopleEpaStatus(request);
            if (result)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
        [HttpPost]
        public ActionResult UpdateApplyPeopleEpbStatus(ApplyPeopleUpdateStatusViewModel request)
        {
            var result = ((ApplyPeopleService)BaseApplyService).UpdateApplyPeopleEpbStatus(request);
            if (result)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
        protected override Dictionary<string, int> GetTypes()
        {
            var dictionary = new Dictionary<string, int>()
            {
                { "請選擇",0 }
            };

            foreach (var enumObj in ExtensionsOfEnum.GetEnumAllValue<ApplyPeopleHandlingSituationTypeEnum>())
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
