using MyEPA.EPA.Attribute;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class MainShiftScheduleController : LoginBaseController
    {
        MainShiftScheduleService MainShiftScheduleService = new MainShiftScheduleService();
        DiasterService DiasterService = new DiasterService();
        DepartmentService DepartmentService = new DepartmentService();

        UsersService UsersService = new UsersService();
        public ActionResult Index(int diasterId = 0)
        {
            var result = MainShiftScheduleService.GetByDiasterId(diasterId);

            ViewBag.DiasterId = diasterId;

            ViewBag.Diasters = DiasterService.GetAll();

            ViewBag.Departments = DepartmentService.GetAll();

            return View(result);
        }
        public ActionResult Create(int diasterId = 0)
        {
            MainShiftScheduleService.Create(diasterId);
            return RedirectToAction("Index",new { diasterId });
        }
        [HttpPost]
        public ActionResult Edit(List<MainShiftScheduleViewModel> models)
        {
            if(models != null)
            {
                MainShiftScheduleService.Update(models);
            }
           
            return JsonResult(new
            {
                IsSuccess = true
            });
        }

        public ActionResult Delete(int diasterId, int id)
        {
            MainShiftScheduleService.Delete(id);
            return RedirectToAction("Index", new { diasterId});
        }

        public ActionResult Moving(int diasterId, int id,int changId)
        {
            MainShiftScheduleService.Moving(id, changId);
            return RedirectToAction("Index", new { diasterId });
        }
    }
}