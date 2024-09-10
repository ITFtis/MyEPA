using MyEPA.Services;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    /// <summary>
    /// 環境部使用 Controller
    /// </summary>
    public class EPAMemberController : LoginBaseController
    {
        private DiasterService DiasterService = new DiasterService();
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.User = GetUserBrief();
            var viewModel = new HomeIndexViewModel()
            {
                UserBrief = GetUserBrief(),
                Diasters = DiasterService.GetAll()
            };

            return View(viewModel);
        }
    }
}