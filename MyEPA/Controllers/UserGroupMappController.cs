using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Models.SearchViewModel;
using MyEPA.Services;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class UserGroupMappController : LoginBaseController
    {
        UserGroupMappService UserGroupMappService = new UserGroupMappService();
        UserGroupService UserGroupService = new UserGroupService();
        UsersService UsersService = new UsersService();
        CityService CityService = new CityService();
        public ActionResult Index(int? cityId = null,int? townId = null,int? groupId = null)
        {
            var userGroupMapp = UserGroupMappService.GetBriefAll();
            if (groupId.HasValue)
            {
                userGroupMapp = userGroupMapp.Where(e => e.GroupId == groupId.Value).ToList();
            }

            var result = UsersService.GetListBriefByFilter(new UsersBriefFilterParameter
            {
                CityIds = cityId.HasValue ? cityId.Value.ToListCollection() : new List<int>(),
                TownIds = townId.HasValue ? townId.Value.ToListCollection() : new List<int>(),
                UserIds = groupId.HasValue ? userGroupMapp.Select(e => e.UserId) : new List<int>(),
            });

            ViewBag.UserGroup = UserGroupService.GetListByType(UserGroupTypeEnum.SMS);
            ViewBag.UserGroupMapps = userGroupMapp;
            ViewBag.CityId = cityId;
            ViewBag.TownId = townId;
            ViewBag.GroupId = groupId;
            ViewBag.Citys = CityService.GetCountyOrderBySort();
            return View(result);
        }
        public ActionResult GroupSMS()
        {
            var result = UserGroupService.GetListByType(UserGroupTypeEnum.SMS);
            return View(result);
        }
        public ActionResult SendGroupSMS(int id)
        {
            var result = UserGroupService.Get(id);
            return View(new SendGroupSMSViewModel 
            {
                GroupId = result.Id,
                GroupName = result.GroupName
            });
        }
        [HttpPost]
        public ActionResult SendGroupSMS(SendGroupSMSViewModel model)
        {
            UserGroupMappService.SendGroupSMS(model);
            TempData["Message"] = "發送成功";
            return RedirectToIndex();
        }
        public ActionResult Create(UserGroupMappBriefModel model)
        {
            UserGroupMappService.Create(GetUserBrief(), model);
            return JsonResult(new AdminResultModel 
            {
                IsSuccess = true
            });
        }

        public ActionResult Delete(UserGroupMappBriefModel model)
        {
            AdminResultModel result = UserGroupMappService.Delete(model.UserId,model.GroupId);
            return JsonResult(result);
        }
        private RedirectToRouteResult RedirectToIndex()
        {
            return RedirectToAction("GroupSMS", new { });
        }
    }
}