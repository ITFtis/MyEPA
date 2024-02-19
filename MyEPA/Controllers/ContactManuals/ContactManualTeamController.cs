using MyEPA.Enums;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Net;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ContactManualTeamController : LoginBaseController
    {
        ContactManualTypeEnum _Type = ContactManualTypeEnum.EPATeam;

        ContactManualTeamService ContactManualTeamService = new ContactManualTeamService();
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Type = _Type;

            var result = ContactManualTeamService.GetList(_Type);

            return View(result);
        }
        public ActionResult Create()
        {
            return View(new ContactManualEPAViewModel
            {
                Type = _Type
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

            model.Type = _Type;

            ContactManualTeamService.Create(user, model);

            return RedirectToAction("Index", new { searchDepartmentId = model.DepartmentId, type = _Type });
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            ContactManualTeamService.Delete(id);

            return new HttpStatusCodeResult(HttpStatusCode.OK); ;
        }
    }
}