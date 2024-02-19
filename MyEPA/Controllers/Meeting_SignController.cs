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
    public class Meeting_SignController : LoginBaseController
    {
        Meeting_SignService Meeting_SignService = new Meeting_SignService();
        public ActionResult Index()
        {
            var result = Meeting_SignService.GetAll();
            return View(result);
        }

        public ActionResult Create()
        {
            var now = DateTimeHelper.GetCurrentTime().Date.AddDays(1);
            return View(new Meeting_SignModel 
            { 
                Meeting_Datetime = now,
            });
        }

        [HttpPost]
        public ActionResult Create(Meeting_SignModel model)
        {
            Meeting_SignService.Create(GetUserBrief(),model);
            return RedirectToIndex();
        }

        public ActionResult Edit(int id)
        {
            var result = Meeting_SignService.Get(id);

            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(Meeting_SignModel model)
        {
            Meeting_SignService.Update(GetUserBrief(), model);
            return RedirectToIndex();
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            AdminResultModel result = Meeting_SignService.Delete(id);
            return JsonResult(result);
        }
        private RedirectToRouteResult RedirectToIndex()
        {
            return RedirectToAction("Index",new {  });
        }
    }
}