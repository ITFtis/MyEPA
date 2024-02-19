using MyEPA.Repositories;
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
    /// 環保局使用 Controller
    /// </summary>
    public class EPBMemberController : LoginBaseController
    {
        private DiasterService DiasterService = new DiasterService();
        private NewsService NewsService = new NewsService();
        private NoticeService NoticeService = new NoticeService();

        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = new HomeIndexViewModel()
            {
                UserBrief = GetUserBrief(),
                Diasters = DiasterService.GetAll(),
                Notices = NoticeService.GetByTop(top:5),
                News = NewsService.GetByTop(top:5)
            };
            ViewBag.IsDiasterRuning = DiasterService.IsExistsRuning();
            return View(viewModel);
        }
    }
}