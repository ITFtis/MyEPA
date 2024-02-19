using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ApplyCarController : ApplyBaseController<ApplyCarModel, ApplyCarViewModel>
    {
        public ApplyCarController()
        {
            BaseApplyService = new ApplyCarService();
        }

        [HttpGet]
        public override ActionResult Index(ApplyRequestViewModel requestViewModel)
        {
            ViewBag.Options = GetOptions();
            return base.Index(requestViewModel);
        }
        [HttpGet]
        public override ActionResult Processing(int id)
        {
            ViewBag.HandlingSituations = ((ApplyCarService)BaseApplyService).GetHandlingSituations(id);
            ViewBag.Options = GetOptions();
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
            ViewBag.HandlingSituations = ((ApplyCarService)BaseApplyService).GetHandlingSituations(id);
            ViewBag.Options = GetOptions();
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
        public ActionResult UpdateApplyCarEpaStatus(ApplyCarUpdateStatusViewModel request)
        {
            var result = ((ApplyCarService)BaseApplyService).UpdateApplyCarEpaStatus(request);
            if (result)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
        [HttpPost]
        public ActionResult UpdateApplyCarEpbStatus(ApplyCarUpdateStatusViewModel request)
        {
            var result = ((ApplyCarService)BaseApplyService).UpdateApplyCarEpbStatus(request);
            if (result)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
        [HttpGet]
        public override ActionResult Create(ApplyRequestViewModel requestViewModel)
        {
            ViewBag.Options = GetOptions();
            return base.Create(requestViewModel);
        }

        [HttpPost]
        public ActionResult CreateModel(ApplyCarModel model, List<ApplyCarDetailModel> details, HttpPostedFileBase file, ApplyRequestViewModel requestViewModel)
        {
            var user = GetUserBrief();
            model.Details = details;
            BaseApplyService.Create(user, model, file, requestViewModel);

            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        public override ActionResult Edit(int id, ApplyRequestViewModel requestViewModel)
        {
            ViewBag.Options = GetOptions();
            return base.Edit(id, requestViewModel);
        }

        [HttpPost]
        public ActionResult EditModel(ApplyCarModel model, List<ApplyCarDetailModel> details, HttpPostedFileBase file, ApplyRequestViewModel requestViewModel)
        {
            var user = GetUserBrief();

            model.Details = details;

            BaseApplyService.Edit(user, model, file);
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        [HttpGet]
        public ActionResult CarOptions()
        {
           
            return JsonResult(GetOptions());
        }


        private Dictionary<string, int> GetOptions()
        {
            var dictionary = new Dictionary<string, int>()
            {
                { "請選擇" , 0 }
            };

            var types = ((ApplyCarService)BaseApplyService).GetTypes();
            foreach (var type in types)
            {
                dictionary.Add(type.DisplayName, type.Id);
            }

            return dictionary;
        }
    }
}