
using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Services;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class FileDataController : LoginBaseController
    {
        FileDataService FileDataService = new FileDataService();

        public ActionResult Index(SourceTypeEnum sourceType)
        {
            var result = FileDataService.GetListBySource(sourceType);

            ViewBag.SourceType = sourceType;

            return View(result);
        }
        public ActionResult GetBySource(SourceTypeEnum sourceType,int id)
        {
            var result = FileDataService.GetBySource(sourceType, id);

            ViewBag.SourceType = sourceType;

            return View(result);
        }
        public ActionResult Upload(HttpPostedFileBase file, SourceTypeEnum sourceType,string description)
        {
            var user = GetUserBrief();
            FileDataService.UploadFileByGuidName(new UploadFileBaseModel
            {
                File = file,
                SourceId = 0,
                SourceType = sourceType,
                User = user.UserName,
                Description = description
            });
            return RedirectToAction("Index", "FileData", new { sourceType });
        }
        public ActionResult Delete(int id, SourceTypeEnum sourceType)
        {
            AdminResultModel result = FileDataService.DeleteFile(id);

            return JsonResult(result);
        }
        public ActionResult Download(int id)
        {
            var result = FileDataService.GetFile(id);

            if(result == null)
            {
                return View();
            }

            return File(result);
        }

        public ActionResult Url(int id)
        {
            var result = FileDataService.GetFile(id);

            if (result == null)
            {
                return View();
            }

            return FilePathResult(result);
        }
        public ActionResult Reference()
        {
            //var user = GetUserBrief();
            //if (user.Duty != DutyEnum.EPA)
            if (!GetIsAdmin())
            {
                //非系統管理者，回首頁                                
                return RedirectToAction("LoginRedirect", "Home");
            }

            return View();
        }
        
    }
}