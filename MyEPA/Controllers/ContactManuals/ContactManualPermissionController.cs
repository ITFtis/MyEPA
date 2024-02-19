using MyEPA.Enums;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ContactManualPermissionController : LoginBaseController
    {
        ContactManualPermissionService ContactManualPermissionService = new ContactManualPermissionService();
        public ActionResult GetSelectListItem(ContactManualDutyEnum duty = ContactManualDutyEnum.Administrator)
        {
            return GetDutySelectListItem(duty);
        }
        public ActionResult GetSearchSelectListItem(ContactManualDutyEnum duty = ContactManualDutyEnum.Administrator)
        {
            return GetDutySelectListItem(duty);
        }
        public ActionResult Index(ContactManualDutyEnum duty = ContactManualDutyEnum.Administrator)
        {
            ViewBag.Duty = duty;
            return View();
        }
        public ActionResult Search(ContactManualDutyEnum duty)
        {
            ViewBag.Duty = duty;
            var result = ContactManualPermissionService.GetListByDuty(duty);
            return PartialView(result);
        }
        public ActionResult Create(ContactManualDutyEnum duty)
        {
            return View(new ContactManualPermissionViewModel 
            {
                ContactManualDuty = duty
            });
        }
        [HttpPost]
        public ActionResult Create(ContactManualPermissionViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            var user = GetUserBrief();
            ContactManualPermissionService.Create(user, model);

            return RedirectToAction("Index", new { model.ContactManualDuty });
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var user = GetUserBrief();
            ContactManualPermissionService.Delete(user, id);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
        private ActionResult GetDutySelectListItem(ContactManualDutyEnum type)
        {
            List<ContactManualDutyEnum> dutys = new List<ContactManualDutyEnum>()
            {
                ContactManualDutyEnum.Administrator,
                ContactManualDutyEnum.Business,
                ContactManualDutyEnum.User
            };
            var result = dutys.ConvertToSelectListItems(type);
            return PartialView(result);
        }
    }
}