using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.Services;

namespace MyEPA.Controllers
{
    public class CleanerxToiletController : LoginBaseController
    {
        ToiletService ToiletService = new ToiletService();

        public ActionResult Add(ToiletModel model)
        {
            model.City = Session["AuthenticateCity"].ToString().Trim();
            model.Town = Session["AuthenticateTown"].ToString().Trim();

            var user = GetUserBrief();
            model.UpdateUser = user.UserName;
            ToiletService.Create(model);

            return RedirectToAction("C3x1Toilet", "Cleaner", new { });
        }
        public class ToiletDeleteModel
        {
            public int DeleteId { get; set; }
        }
        public ActionResult Delete(ToiletDeleteModel model)
        {
            ToiletService.Delete(model.DeleteId);
            return RedirectToAction("C3x1Toilet", "Cleaner", new { });
        }
        public JsonResult Edit(int id)
        {
            var item = ToiletService.GetById(id);

            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Confirm(int? townId = null)
        {
            ToiletService.Confirm(GetUserBrief(), townId);

            ViewBag.Msg = "資料確認成功";

            return RedirectToAction("C3x1Toilet", "Cleaner", new { townId });

        }
        public ActionResult Update(ToiletEditViewModel model)
        {
            var user = GetUserBrief();
            ToiletService.Update(model, user);
            return RedirectToAction("C3x1Toilet", "Cleaner", new { });
        }
    }
}