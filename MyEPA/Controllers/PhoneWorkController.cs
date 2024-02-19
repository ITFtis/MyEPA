using iTextSharp.text;
using MyEPA.EPA.Attribute;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MyEPA.Controllers
{
    public class PhoneWorkController : LoginBaseController
    {
        PhoneWorkService PhoneWorkService = new PhoneWorkService();
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

            var result = PhoneWorkService.GetByDiasterId(diasterId.Value);

            return View(result);
        }

        public ActionResult Create(int diasterId)
        {
            return View(new PhoneWorkModel() 
            {
                PhoneTime = DateTimeHelper.GetCurrentTime(),
                DiasterId = diasterId
            });
        }
        [HttpPost]
        public ActionResult Create(PhoneWorkModel model)
        {
            PhoneWorkService.Create(GetUserBrief(),model);
            return RedirectToIndex(model.DiasterId);
        }

        public ActionResult Edit(int? id)
        {
            if (id.HasValue == false)
            {
                return RedirectToIndex();
            }
            var result = PhoneWorkService.Get(id.Value);
            if (result == null)
            {
                return RedirectToIndex();
            }
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(PhoneWorkViewModel model)
        {
            PhoneWorkService.Update(GetUserBrief() ,model);
            return RedirectToIndex(model.DiasterId);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            AdminResultModel result = PhoneWorkService.Delete(id);
            return JsonResult(result);
        }

        private RedirectToRouteResult RedirectToIndex(int? diasterId = null)
        {
            return RedirectToAction("Index",new { diasterId });
        }
    }
}