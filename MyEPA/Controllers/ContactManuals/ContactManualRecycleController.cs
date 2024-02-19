using MyEPA.Enums;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ContactManualRecycleController : LoginBaseController
    {
        ContactManualRecycleService ContactManualRecycleService = new ContactManualRecycleService();
        public ActionResult GetTypeSelectListItem(ContactManualTypeEnum? type = null)
        {
            List<SelectListItem> result = GetTypeSelectListItems(type);

            return PartialView(result);
        }

        private List<SelectListItem> GetTypeSelectListItems(ContactManualTypeEnum? type)
        {
            var result = new List<ContactManualTypeEnum>
            {
                ContactManualTypeEnum.EPARecycle,
                ContactManualTypeEnum.EPARecycleEF,

            }.ConvertToGroupSelectListItems(type);

            var user = GetUserBrief();
            if (user.ContactManualDuty == ContactManualDutyEnum.Business)
            {
                result = result.Where(e => e.Group.Name == user.ContactManualDepartment).ToList();
            }

            return result;
        }

        [HttpGet]
        public ActionResult Index(ContactManualTypeEnum type)
        {
            ViewBag.Type = type;
            ViewBag.DepartmentCount = GetTypeSelectListItems(type).Count;
            var result = ContactManualRecycleService.GetList(type);

            var user = GetUserBrief();
            if (user.ContactManualDuty == ContactManualDutyEnum.Business)
            {
                result = result.Where(e => e.SourceId == GetUserContactManualDepartmentId()).ToList();
            }

            return View(result);
        }
        public ActionResult Create(ContactManualTypeEnum type)
        {
            return View(new ContactManualEPAViewModel
            {
                Type = type
            });
        }

        [HttpPost]
        public ActionResult Create(ContactManualEPAViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            var user = GetUserBrief();
            ContactManualRecycleService.Create(user, model);

            return RedirectToAction("Index", new { searchDepartmentId = model.DepartmentId, type = model.Type });
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            ContactManualRecycleService.Delete(id);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}