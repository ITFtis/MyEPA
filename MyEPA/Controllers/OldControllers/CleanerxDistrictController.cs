using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Models;
using MyEPA.Services;

namespace MyEPA.Controllers
{
    public class CleanerxDistrictController : LoginBaseController
    {
        public ActionResult Update(DistrictModel model)
        {
            UserBriefModel user = GetUserBrief();

            new DistrictService().Update(user, model);

            ViewBag.Msg = "修改成功";
            ViewBag.City = user.City;
            ViewBag.Town = user.Town;

            return RedirectToAction("C3x1District", "Cleaner",new { Message = "修改成功"});
        }
    }
}