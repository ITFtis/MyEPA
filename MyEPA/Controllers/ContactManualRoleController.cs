using MyEPA.Enums;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ContactManualRoleController : LoginBaseController
    {
        ContactManualRoleService ContactManualRoleService = new ContactManualRoleService();
        public ActionResult GetSelectListItem(ContactManualRoleTypeEnum type = ContactManualRoleTypeEnum.General, int? id = null)
        {
            return GetContactManualRoleSelectListItem(type, id);
        }
        public ActionResult Index(ContactManualRoleTypeEnum type = ContactManualRoleTypeEnum.General)
        {
            var ContactManualRoles = ContactManualRoleService.GetByType(type);
            ViewBag.Type = type;
            return View(ContactManualRoles);
        }
        public ActionResult Edit(ContactManualRoleTypeEnum type,int? contactManualRoleId = null)
        {
            if (contactManualRoleId.HasValue)
            {
                var result = ContactManualRoleService.Get(contactManualRoleId.Value);
                result.Type = type;
                return View(result);
            }
            return View(new ContactManualRoleViewModel { Type = type });
        }
        [HttpPost]
        public ActionResult Edit(ContactManualRoleViewModel model)
        {
            var user = GetUserBrief();
            if (model.Id.HasValue)
            {
                ContactManualRoleService.Update(user, model);
            }
            else
            {
                ContactManualRoleService.Create(user, model);
            }
            return RedirectToAction("Index");
        }
        private ActionResult GetContactManualRoleSelectListItem(ContactManualRoleTypeEnum? type ,int? id)
        {
            var result = ContactManualRoleService.GetByType(type).Select(e => new SelectListItem
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