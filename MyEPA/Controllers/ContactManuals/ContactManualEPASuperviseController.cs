using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ContactManualEPASuperviseController : LoginBaseController
    {
        ContactManualService ContactManualService = new ContactManualService();
        ContactManualDepartmentService ContactManualDepartmentService = new ContactManualDepartmentService();
        UsersService UsersService = new UsersService();
        public ActionResult GetTypeSelectListItem(ContactManualTypeEnum? type = null)
        {
            List<SelectListItem> result = GetTypeSelectListItems(type);

            return PartialView(result);
        }

        private List<SelectListItem> GetTypeSelectListItems(ContactManualTypeEnum? type)
        {
            var user = GetUserBrief();
            var result = new List<ContactManualTypeEnum>
            {
                ContactManualTypeEnum.EPASuperviseAirSecurity,
                ContactManualTypeEnum.EPASuperviseChemical,
                ContactManualTypeEnum.EPASuperviseTeam,
                ContactManualTypeEnum.EPASuperviseControlAssessment,
                ContactManualTypeEnum.EPASuperviseGeneralPlanning,
                ContactManualTypeEnum.EPASuperviseRecycle,
                ContactManualTypeEnum.EPASuperviseSoilPollution,
                ContactManualTypeEnum.EPASuperviseWaste,
                ContactManualTypeEnum.EPASuperviseWaterSecurity,
                ContactManualTypeEnum.EPASuperviseSupervision,
                ContactManualTypeEnum.EPASuperviseTeam,
            }.ConvertToGroupSelectListItems(type);

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
            var result = ContactManualService.GetListByType(type).Where(e=>e.SourceId == GetUserContactManualDepartmentId());

            var user = GetUserBrief();
            if(user.ContactManualDuty == ContactManualDutyEnum.Business)
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

            UsersModel userModel = UsersService.GetById(model.UserId);
            if(userModel != null)
            {
                model.DepartmentId = userModel.ContactManualDepartmentId;
                ContactManualService.Create(user, model);
            }

            return RedirectToAction("Index", new { type = model.Type });
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            ContactManualService.Delete(id);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}