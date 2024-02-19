using MyEPA.Models;
using MyEPA.Services;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ContactManualDownloadRecordController : LoginBaseController
    {
        ContactManualDownloadRecordService ContactManualDownloadRecordService = new ContactManualDownloadRecordService();
        public ActionResult Index(PaginationModel pagination)
        {
            var result = ContactManualDownloadRecordService.GetPagingList(pagination);
            return View(result);
        }
    }
}