using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Models.SearchViewModel;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class UserGroupController : LoginBaseController
    {
        UserGroupService UserGroupService = new UserGroupService();
        [Route("UserGroup/{type}")]
        public ActionResult Index(UserGroupTypeEnum type)
        {
            var result = UserGroupService.GetListByType(type);
            ViewBag.Type = type;
            return View(result);
        }

        public ActionResult Create(UserGroupTypeEnum type)
        {
            return View(new UserGroupModel 
            { 
                Type = type.ToInteger()
            });
        }

        [HttpPost]
        public ActionResult Create(UserGroupModel model)
        {
            UserGroupService.Create(GetUserBrief(),model);
            return RedirectToIndex((UserGroupTypeEnum)model.Type);
        }

        public ActionResult Edit(int id)
        {
            var result = UserGroupService.Get(id);

            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(UserGroupModel model)
        {
            UserGroupService.Update(GetUserBrief(), model);
            return RedirectToIndex((UserGroupTypeEnum)model.Type);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            AdminResultModel result = UserGroupService.Delete(id);
            return JsonResult(result);
        }
        private RedirectToRouteResult RedirectToIndex(UserGroupTypeEnum type)
        {
            return RedirectToAction("Index",new { type });
        }
    }
}