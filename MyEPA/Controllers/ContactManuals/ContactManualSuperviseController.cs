using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ContactManualSuperviseController : LoginBaseController
    {
        ContactManualSuperviseService ContactManualSuperviseService = new ContactManualSuperviseService();
        public ActionResult Index()
        {
            var departments = ContactManualSuperviseService.GetAll();
            return View(departments);
        }
        public ActionResult Edit(int? id = null)
        {
            if (id.HasValue)
            {
                var result = ContactManualSuperviseService.Get(id.Value);
                return View(result);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Edit(ContactManualSuperviseViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(new ContactManualSuperviseModel 
                {
                    DepartmentId = model.DepartmentId,
                    Id = model.Id.GetValueOrDefault(),
                    Describe = model.Describe
                });
            }
            var user = GetUserBrief();
            if (model.Id.HasValue)
            {
                ContactManualSuperviseService.Update(user, model);
            }
            else
            {
                ContactManualSuperviseService.Create(user, model);
            }
            return RedirectToAction("Index");
        }
    }
}