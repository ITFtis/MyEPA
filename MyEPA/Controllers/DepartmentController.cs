using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class DepartmentController : LoginBaseController
    {
        DepartmentService DepartmentService = new DepartmentService();
        public ActionResult GetSelectListItem(int? id)
        {
            return GetDepartmentSelectListItem(id);
        }
        public ActionResult GetSearchSelectListItem(int? id)
        {
            return GetDepartmentSelectListItem(id);
        }
        public ActionResult Index()
        {
            var departments = DepartmentService.GetAll();
            return View(departments);
        }
        public ActionResult Edit(int? departmentId = null)
        {
            if (departmentId.HasValue)
            {
                var result = DepartmentService.Get(departmentId.Value);
                return View(result);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Edit(DepartmentViewModel model)
        {
            if (model.Id.HasValue)
            {
                DepartmentService.Update(model);
            }
            else
            {
                DepartmentService.Create(model);
            }
            return RedirectToAction("Index");
        }
        private ActionResult GetDepartmentSelectListItem(int? id)
        {
            var result = DepartmentService.GetAll().Select(e => new SelectListItem
            {
                Text = e.Name,
                Value = e.Id.ToString(),
                Selected = id == e.Id
            }).ToList();

            result.Insert(0, new SelectListItem
            {
                Text = "請選擇",
                Value = null
            });

            return PartialView(result);
        }
    }
}