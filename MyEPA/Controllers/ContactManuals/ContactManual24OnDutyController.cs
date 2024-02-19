using MyEPA.Enums;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ContactManual24OnDutyController : LoginBaseController
    {
        ContactManualService ContactManualService = new ContactManualService();
        ContactManualDepartmentService ContactManualDepartmentService = new ContactManualDepartmentService();
        ContactManualTypeEnum type = ContactManualTypeEnum.EPA24OnDutyChemical;
        [HttpGet]
        public ActionResult Index(int? searchDepartmentId = null)
        {
            ViewBag.SearchDepartmentId = searchDepartmentId;
            ViewBag.Type = type;
            ViewBag.DepartmentCount = ContactManualDepartmentService.GetContactManualDepartments(GetUserBrief(), ContactManualDepartmentTypeEnum.EPA24OnDutyChemical).Count();

            if (searchDepartmentId.HasValue == false)
            {
                return View(new List<ContactManualViewModel>());
            }
            var result = ContactManualService.GetListBySourceId(type, searchDepartmentId.Value);

            return View(result);
        }
        public ActionResult Create()
        {
            return View(new ContactManualEPAViewModel
            {
                Type = type,
            });
        }

        [HttpPost]
        public ActionResult Create(ContactManualEPAViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            model.Type = type;
            var user = GetUserBrief();
            ContactManualService.Create(user, model);

            return RedirectToAction("Index", new { searchDepartmentId = model.DepartmentId, type = model.Type });
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            ContactManualService.Delete(id);

            return new HttpStatusCodeResult(HttpStatusCode.OK); ;
        }
    }
}