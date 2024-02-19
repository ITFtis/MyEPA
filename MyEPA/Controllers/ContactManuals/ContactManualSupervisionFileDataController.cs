
using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ContactManualSupervisionFileDataController : LoginBaseController
    {
        ContactManualSupervisionFileDataService ContactManualSupervisionFileDataService = new ContactManualSupervisionFileDataService();

        public ActionResult GetSelectListItem(SupervisionSourceEnum? source = null)
        {
            var result = new List<SupervisionSourceEnum>
            {
                SupervisionSourceEnum.One,
                SupervisionSourceEnum.Two
            }.Select(e => new SelectListItem
            {
                Text = e.GetDescription(),
                Value = e.ToIntegerString(),
                Selected = e == source
            }).ToList();
            return PartialView(result);
        }
        public ActionResult Index()
        {
            var result = ContactManualSupervisionFileDataService.GetList();

            return View(result);
        }
        public ActionResult Upload(int? sourceId = null)
        {
            if (sourceId.HasValue)
            {
                var result = ContactManualSupervisionFileDataService.GetBySourceId(sourceId.Value);
                return View(result);
            }
            return View(new ContactManualSupervisionFileDataUploadViewModel
            {
                SourceType = SourceTypeEnum.Supervision
            });
        }
        [HttpPost]
        public ActionResult Upload(ContactManualSupervisionFileDataUploadViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            var user = GetUserBrief();
            ContactManualSupervisionFileDataService.UploadFile(new UploadFileBaseModel
            {
                File = model.File,
                SourceId = model.SourceId.Value,
                SourceType = SourceTypeEnum.Supervision,
                User = user.UserName,
                Description = model.Description
            });
            return RedirectToAction("Index", "ContactManualSupervisionFileData", new { });
        }
        public ActionResult Download(int id)
        {
            var result = ContactManualSupervisionFileDataService.GetFile(id);

            if (result == null)
            {
                return View();
            }

            return File(result);
        }

        public ActionResult Url(int id)
        {
            var result = ContactManualSupervisionFileDataService.GetFile(id);

            if (result == null)
            {
                return View();
            }

            return FilePathResult(result);
        }
    }
}
