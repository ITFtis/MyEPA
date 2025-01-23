using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Repositories;
using MyEPA.Services;

namespace MyEPA.Controllers
{
    public class HomexRegisterController : Controller
    {
        CityRepository cityRepository = new CityRepository();
        TownRepository townRepository = new TownRepository();
        RegisterService RegisterService = new RegisterService();
        public ActionResult InputRegister(RegistersModel model ,List<string> humanType)
        {
            if (!PwdHelper.ValidPassword(model.Pwd))
            {
                TempData["Msg"] = PwdHelper.ErrorMessage;
                return RedirectToAction("Register");
            }

            model.SourceIP = LoginHelper.GetClientIP(Request);
            AdminResultModel result = RegisterService.Register(model, humanType);

            ViewBag.Msg = result.ErrorMessage;
            return View("~/Views/Home/Message.cshtml");
        }

        public ActionResult Register()
        {
            ViewBag.Positions = new PositionService().GetAll();
            ViewBag.Msg = TempData["Msg"];
            return View("~/Views/Home/Register.cshtml");
        }

    }
}