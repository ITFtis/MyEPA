using MyEPA.Enums;
using MyEPA.Helper;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ContactManualOnDutyController : LoginBaseController
    {
        ContactManualOnDutyService ContactManualOnDutyService = new ContactManualOnDutyService();
        public ActionResult GetTypeSelectListItem(ContactManualTypeEnum? type = null)
        {
            List<SelectListItem> result = GetTypeSelectListItems(type);

            return PartialView(result);
        }

        private List<SelectListItem> GetTypeSelectListItems(ContactManualTypeEnum? type)
        {
            var result = new List<ContactManualTypeEnum>
            {
                ContactManualTypeEnum.EPAOnDutySupervision,
                ContactManualTypeEnum.EPAOnDutySupervisionInformation,

            }.ConvertToGroupSelectListItems(type);

            var user = GetUserBrief();
            if (user.ContactManualDuty == ContactManualDutyEnum.Business)
            {
                result = result.Where(e => e.Group.Name == user.ContactManualDepartment).ToList();
            }

            return result;
        }

        public ActionResult Index(ContactManualTypeEnum type)
        {
            ViewBag.Type = type;
            ViewBag.DepartmentCount = GetTypeSelectListItems(type).Count;

            if (type.GetContactManualGroup() != GetUserContactManualDepartment())
            {
                return View(new List<ContactManualOnDutyViewModel>());
            }

            var result = ContactManualOnDutyService
                .GetListByOnDutyType(type);

            return View(result);
        }

        public ActionResult Create(ContactManualTypeEnum type)
        {
            return View(new ContactManualOnDutyCreateViewModel
            {
                Type = type,
                Date = DateTimeHelper.GetCurrentTime().Date
            });
        }

        [HttpPost]
        public ActionResult Create(ContactManualOnDutyCreateViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            var user = GetUserBrief();

            ContactManualOnDutyService.Create(user, model);

            return RedirectToAction("Index", new { type = model.Type });
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            ContactManualOnDutyService.Delete(id);

            return new HttpStatusCodeResult(HttpStatusCode.OK); ;
        }
    }
}