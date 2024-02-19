using MyEPA.Services;
using MyEPA.ViewModels;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    /// <summary>
    /// 自來水公司使用 Controller
    /// </summary>
    public class WaterMemberController : LoginBaseController
    {
        private DiasterService DiasterService = new DiasterService();
        [HttpGet]
        public ActionResult Index()
        {
            var viewModel = new HomeIndexViewModel()
            {
                UserBrief = GetUserBrief(),
                Diasters = DiasterService.GetAll()
            };

            return View(viewModel);
        }
    }
}