using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ContactManualEPAController : LoginBaseController
    {
        ContactManualService ContactManualService = new ContactManualService();
        ContactManualDepartmentService ContactManualDepartmentService = new ContactManualDepartmentService();
        [HttpGet]
        public ActionResult Index(ContactManualTypeEnum type, int? searchDepartmentId = null)
        {
            ViewBag.SearchDepartmentId = searchDepartmentId;
            ViewBag.Type = type;
            
            ViewBag.DepartmentCount = ContactManualDepartmentService.GetContactManualDepartments(GetUserBrief()).Count();

            if (searchDepartmentId.HasValue == false)
            {
                return View(new List<ContactManualViewModel>());
            }
           
            var result = ContactManualService.GetListBySourceId(type, searchDepartmentId.Value);
            return View(result);
        }
        public ActionResult Create(ContactManualTypeEnum type, int? departmentId = null)
        {
            return View(new ContactManualEPAViewModel
            {
                Type = type,
                DepartmentId = departmentId
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

            return RedirectToAction("Index",new { searchDepartmentId = model.DepartmentId, type = model.Type });
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            ContactManualService.Delete(id);

            return new HttpStatusCodeResult(HttpStatusCode.OK); ;
        }
    }
}