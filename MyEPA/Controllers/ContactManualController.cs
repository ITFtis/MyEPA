using MyEPA.Helper;
using MyEPA.Services;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ContactManualController : LoginBaseController
    {
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ReportOfPdf()
        {
            var user = GetUserBrief();
            var result = new ContactManualReportService().ReportOfPDF(user);
            var fileName = $"{DateTimeHelper.GetCurrentTime().ToString("yyyyMMdd")}.pdf";
            return File(result, fileName);
        }
    }
}