
using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ContactManualFileDataController : LoginBaseController
    {
        ContactManualFileDataService ContactManualFileDataService = new ContactManualFileDataService();
        
        public ActionResult Index(SourceTypeEnum sourceType)
        {
            var result = ContactManualFileDataService.GetListBySource(sourceType);

            ViewBag.SourceType = sourceType;

            return View(result);
        }
        public ActionResult Upload(SourceTypeEnum sourceType, int? sourceId = null)
        {
            if (sourceId.HasValue)
            {
                var result = ContactManualFileDataService.GetBySource(sourceType, sourceId.Value);
                return View(result);
            }
            return View(new ContactManualFileDataUploadViewModel 
            {
                SourceType = sourceType
            });
        }
        [HttpPost]
        public ActionResult Upload(ContactManualFileDataUploadViewModel model)
        {
            if(model.File?.ContentType != GetContentType(".docx"))
            {
                ModelState.AddModelError(nameof(model.File), "檔案格式錯誤");
            }
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            var user = GetUserBrief();
            ContactManualFileDataService.UploadFile(new UploadFileBaseModel
            {
                File = model.File,
                SourceId = model.DepartmentId.Value,
                SourceType = model.SourceType,
                User = user.UserName,
                Description = model.Description
            });
            return RedirectToAction("Index", "ContactManualFileData", new { model.SourceType });
        }
        public ActionResult Download(int id)
        {
            var result = ContactManualFileDataService.GetFile(id);

            if (result == null)
            {
                return View();
            }

            return File(result);
        }

        public ActionResult Url(int id)
        {
            var result = ContactManualFileDataService.GetFile(id);

            if (result == null)
            {
                return View();
            }

            return FilePathResult(result);
        }
    }
}
