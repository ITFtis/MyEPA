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
    public class ContactManualEPBController : LoginBaseController
    {
        ContactManualService ContactManualService = new ContactManualService();
        ContactManualDepartmentService ContactManualDepartmentService = new ContactManualDepartmentService();
        public ActionResult GetTypeSelectListItem(ContactManualTypeEnum? type = null)
        {
            var result = new List<ContactManualTypeEnum>
            {
                ContactManualTypeEnum.EPB,
                ContactManualTypeEnum.EPBAirSecurity,
                ContactManualTypeEnum.EPBChemical,
                ContactManualTypeEnum.EPBChemicalDrug,
                ContactManualTypeEnum.EPBEnvironmentDisaster,
                ContactManualTypeEnum.EPBEnvironmentInfluenza,
                ContactManualTypeEnum.EPBControlAssessment,
                ContactManualTypeEnum.EPBRecycle,
                ContactManualTypeEnum.EPBSoilPollution,
                ContactManualTypeEnum.EPBTeam,
                ContactManualTypeEnum.EPBWaste,
                ContactManualTypeEnum.EPBWaterSecurityDrinkingWater,
                ContactManualTypeEnum.EPBWaterSecurityRiver,
            }.Select(e => new SelectListItem
            {
                Text = e.GetDescription(),
                Value = e.ToIntegerString(),
                Selected = e == type
            }).ToList();

            return PartialView(result);
        }
        [HttpGet]
        public ActionResult Index(ContactManualTypeEnum type, int? searchDepartmentId = null)
        {
            ViewBag.SearchCityId = searchDepartmentId;
            ViewBag.Type = type;
            ViewBag.DepartmentCount = ContactManualDepartmentService.GetContactManualDepartments(GetUserBrief(), ContactManualDepartmentTypeEnum.EPB).Count();
            if (searchDepartmentId.HasValue == false)
            {
                return View(new List<ContactManualViewModel>());
            }

            var result = ContactManualService.GetListBySourceId(type, searchDepartmentId.Value);
            return View(result);
        }
        public ActionResult Create(ContactManualTypeEnum type, int? cityId = null)
        {
            return View(new ContactManualEPBViewModel
            {
                Type = type,
                CityId = cityId.GetValueOrDefault()
            });
        }

        [HttpPost]
        public ActionResult Create(ContactManualEPBViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            var user = GetUserBrief();
            ContactManualService.Create(user, model);

            return RedirectToAction("Index", new { searchCityId = model.CityId, type = model.Type });
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            ContactManualService.Delete(id);

            return new HttpStatusCodeResult(HttpStatusCode.OK); ;
        }
    }
}