using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Models;
using MyEPA.Services;

namespace MyEPA.Controllers
{
    public class EPBxDistrictController : LoginBaseController
    {
        public ActionResult Update(DistrictModel model)
        {
            var user = GetUserBrief();
            new DistrictService().Update(GetUserBrief(), model);
            ViewBag.Msg = "修改成功";
            ViewBag.City = user.City;
            return View("/Views/EPB/EPB.cshtml");
        }

        public ActionResult Confirm()
        {
            String City = Session["AuthenticateCity"].ToString();
            StatisticsModel Statistics = new StatisticsModel();
            String ResponseMsg = Statistics.StoreConfirmTime("District", City);
            ViewBag.Msg = ResponseMsg;
            ViewBag.City = City;
            return View("/Views/EPB/EPB.cshtml");
        }
    }
}