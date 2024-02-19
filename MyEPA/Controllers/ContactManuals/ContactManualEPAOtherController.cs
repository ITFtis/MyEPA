using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Services;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ContactManualEPAOtherController : LoginBaseController
    {
        ContactManualService ContactManualService = new ContactManualService();
        public ActionResult GetTypeSelectListItem(ContactManualTypeEnum? type = null)
        {
            List<SelectListItem> result = GetTypeSelectListItems(type);

            return PartialView(result);
        }

        private List<SelectListItem> GetTypeSelectListItems(ContactManualTypeEnum? type)
        {
            var result = new List<ContactManualTypeEnum>
            {
                ContactManualTypeEnum.EPAEnvironmentalInspection,
                ContactManualTypeEnum.EPAGeneralPlanning,
                ContactManualTypeEnum.EPANewsPublicRelationsTeam,
                ContactManualTypeEnum.EPASecretaryRoom,
                ContactManualTypeEnum.EPASoilPollution,
                ContactManualTypeEnum.EPATeam
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
            var result = ContactManualService.GetListByType(type);

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
            ContactManualService.Create(user, model);

            return RedirectToAction("Index", new { type = model.Type });
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            ContactManualService.Delete(id);

            return new HttpStatusCodeResult(HttpStatusCode.OK); ;
        }
    }
}