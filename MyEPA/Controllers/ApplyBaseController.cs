using MyEPA.Models.BaseModels;
using MyEPA.Services;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ApplyBaseController<T, TQ> : LoginBaseController
        where T : ApplyBaseModel
        where TQ : ApplyBaseModel
    {
        internal IApplyService<T, TQ> BaseApplyService;
        internal CityService CityService;
        internal TownService TownService;

        public ApplyBaseController() 
        {
            CityService = new CityService();
            TownService = new TownService();
        }

        [HttpGet]
        public virtual ActionResult Index(ApplyRequestViewModel requestViewModel)
        {
            var user = GetUserBrief();
            var viewModel = BaseApplyService.GetApplyIndexViewModel(user.Duty, requestViewModel);
            ViewBag.RequestViewModel = requestViewModel;
            return View(viewModel);
        }

        [HttpGet]
        public virtual ActionResult Get(ApplyRequestViewModel requestViewModel)
        {
            var user = GetUserBrief();
            ApplyViewModel result = BaseApplyService.GetApplyViewModel(user.Duty, requestViewModel);
            return JsonResult(result);
        }


        [HttpGet]
        public virtual ActionResult Create(ApplyRequestViewModel requestViewModel)
        {
            ViewBag.RequestViewModel = requestViewModel;
            return View(BaseApplyService.GetCreateModel());
        }

        [HttpPost]
        public virtual ActionResult Create(T model, HttpPostedFileBase file, ApplyRequestViewModel requestViewModel)
        {
            var user = GetUserBrief();
            BaseApplyService.Create(user, model, file, requestViewModel);

            return RedirectToAction("Index", requestViewModel);
        }

        [HttpGet]
        public virtual ActionResult Edit(int id, ApplyRequestViewModel requestViewModel)
        {
            var userName = GetUserName();
            var found = BaseApplyService.GetViewModelById(id);
            ViewBag.RequestViewModel = requestViewModel;
            if (found != null)
            {
                return View(found);
            }
            else
            {
                return RedirectToAction("Index", requestViewModel);
            }
        }

        [HttpPost]
        public virtual ActionResult Edit(T model, HttpPostedFileBase file, ApplyRequestViewModel requestViewModel)
        {
            var user = GetUserBrief();
            BaseApplyService.Edit(user, model, file);
            return RedirectToAction("Index", requestViewModel);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var user = GetUserBrief();
            BaseApplyService.Delete(user, id);

            return new HttpStatusCodeResult(HttpStatusCode.OK); ;
        }

        [HttpGet]
        public virtual ActionResult Processing(int id)
        {
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
            ViewBag.Options = GetTypes();
            ViewBag.HandlingSituations = BaseApplyService.GetHandlingSituations(id);
            return View();
        }
        [HttpGet]
        public virtual ActionResult Details(int id)
        {
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
            ViewBag.HandlingSituations = BaseApplyService.GetHandlingSituations(id);
            return View();
        }
        [HttpGet]
        public virtual ActionResult EPAProcessing(int id)
        {
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
            ViewBag.Options = GetTypes();
            ViewBag.HandlingSituations = BaseApplyService.GetHandlingSituations(id);
            return View();
        }
        protected virtual Dictionary<string, int> GetTypes()
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public virtual ActionResult UpdateEpbStatus(ApplySupportUpdateStatusViewModel request, List<ApplyHandlingSituationViewModel> handlingSituations)
        {
            var result = BaseApplyService.UpdateEpbStatus(request, handlingSituations);
            if (result)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
        [HttpPost]
        public virtual ActionResult UpdateEpaStatus(ApplySupportUpdateStatusViewModel request, List<ApplyHandlingSituationViewModel> handlingSituations)
        {
            var result = BaseApplyService.UpdateEpaStatus(request, handlingSituations);
            if (result)
            {
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
    }
}