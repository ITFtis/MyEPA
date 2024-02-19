using MyEPA.Services;
using MyEPA.ViewModels;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    /// <summary>
    /// 清潔隊員使用 Controller
    /// </summary>
    public class CleaningMemberController : LoginBaseController
    {
        private DiasterService DiasterService = new DiasterService();
        private NewsService NewsService = new NewsService();
        private NoticeService NoticeService = new NoticeService();

        [HttpGet]
        public ActionResult Index()
        {
            var user = GetUserBrief();
            var viewModel = new HomeIndexViewModel()
            {
                UserBrief = user,
                Diasters = DiasterService.GetAll(),
                Notices = NoticeService.GetByTop(top : 5),
                News = NewsService.GetByTop(top: 5)
            };

            
            return View(viewModel);
        }
    }
}