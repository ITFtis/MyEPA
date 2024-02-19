using MyEPA.Enums;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ContactManualEPARoleController : LoginBaseController
    {
        ContactManualEPARoleService ContactManualService = new ContactManualEPARoleService();
        public ActionResult GetTypeSelectListItem(ContactManualTypeEnum? type = null)
        {
            List<SelectListItem> result = GetTypeSelectListItems(type);

            return PartialView(result);
        }

        private List<SelectListItem> GetTypeSelectListItems(ContactManualTypeEnum? type)
        {
            var result = new List<ContactManualTypeEnum>
            {
                ContactManualTypeEnum.EPARoleAirSecurity,
                ContactManualTypeEnum.EPARoleWaterSecurityRiver,
                ContactManualTypeEnum.EPARoleWaterSecurityDrinkingWater,
                ContactManualTypeEnum.EPARoleWaste,
                ContactManualTypeEnum.EPARoleEnvironmentDisaster,
                ContactManualTypeEnum.EPARoleEnvironmentInfluenza,
                ContactManualTypeEnum.EPARoleControlAssessment,
                ContactManualTypeEnum.EPARoleSupervisionAirQuality,
                ContactManualTypeEnum.EPARoleSupervisionAir,
                ContactManualTypeEnum.EPARoleTeam,
                ContactManualTypeEnum.EPARoleRecycle,
                ContactManualTypeEnum.EPARoleChemicalPoison,
                ContactManualTypeEnum.EPARoleChemicalDrug,
                ContactManualTypeEnum.EPARoleSupervisionInformation,
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
            var result = ContactManualService.GetList(type);

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