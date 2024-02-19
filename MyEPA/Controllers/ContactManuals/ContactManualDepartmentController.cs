using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ContactManualDepartmentController : LoginBaseController
    {
        ContactManualDepartmentService ContactManualDepartmentService = new ContactManualDepartmentService();
        public ActionResult GetSelectListItem(ContactManualDepartmentTypeEnum? type = null, int? id = null)
        {
            var user = GetUserBrief();

            if (user.ContactManualDuty == ContactManualDutyEnum.Business)
            {
                id = user.ContactManualDepartmentId;
            }
            
            return GetDepartmentSelectListItem(type, id);
        }
        public ActionResult GetSearchSelectListItem(ContactManualDepartmentTypeEnum? type = null, int? id = null)
        {
            return GetDepartmentSelectListItem(type, id);
        }
        public ActionResult Index(ContactManualDepartmentTypeEnum type)
        {
            ViewBag.Type = type;
            var departments = ContactManualDepartmentService.GetByType(type);
            return View(departments);
        }
        public ActionResult Edit(ContactManualDepartmentTypeEnum type,int? departmentId = null)
        {
            if (departmentId.HasValue)
            {
                var result = ContactManualDepartmentService.Get(departmentId.Value);
                result.Type = type;
                return View(result);
            }
            return View(new ContactManualDepartmentViewModel
            {
                Type = type
            });
        }
        [HttpPost]
        public ActionResult Edit(ContactManualDepartmentViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            var user = GetUserBrief();
            if (model.Id.HasValue)
            {
                ContactManualDepartmentService.Update(user, model);
            }
            else
            {
                ContactManualDepartmentService.Create(user, model);
            }
            return RedirectToAction("Index", new { model.Type });
        }
        private ActionResult GetDepartmentSelectListItem(ContactManualDepartmentTypeEnum? type = null, int? id = null)
        {
            var result = ContactManualDepartmentService.GetContactManualDepartments(GetUserBrief(), type).Select(e => new SelectListItem
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