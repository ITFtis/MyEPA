using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Models.SearchViewModel;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MyEPA.Controllers
{
    public class UsersController : LoginBaseController
    {
        UsersService UsersService = new UsersService();
        public ActionResult Index(UserSearchPaginationViewModel searchModel, PaginationModel pagination)
        {
            if (searchModel == null)
            {
                searchModel = new UserSearchPaginationViewModel();
            }
            if (pagination == null)
            {
                pagination = new PaginationModel();
            }

            ViewBag.SearchModel = searchModel;

            if (string.IsNullOrWhiteSpace(pagination.SortBy))
            {
                pagination.SortBy = nameof(UsersModel.Id);
            }

            UsersFilterPaginationParameter usersFilter = new UsersFilterPaginationParameter
            {
                DutyIds = searchModel.SearchDutyId?.ToListCollection(),
                CityIds = searchModel.SearchCityId?.ToListCollection(),
                TownIds = searchModel.SearchTownId?.ToListCollection(),
                MainContacter = searchModel.SearchMainContacter,
                HumanType = searchModel.SearchHumanType,
                Pagination = pagination
            };
            
            var result = UsersService.GetPagingList(usersFilter);

            return View(result);
        }

        public ActionResult Edit(int? id)
        {
            if (id.HasValue == false) 
            {
                return View();
            }
            var result = UsersService.GetEditById(id.Value);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(UserModel user)
        {
            UsersService.Updare(user);
            UserSearchPaginationViewModel searchModel = new UserSearchPaginationViewModel 
            {
                SearchCityId = user.CityId,
                SearchDutyId = user.DutyId,
                SearchHumanType = user.HumanType,
                SearchMainContacter = user.MainContacter,
                SearchTownId = user.TownId
            };
            return RedirectToAction("Index", searchModel);
        }
        public ActionResult EditPwd()
        {
            ViewBag.Msg = TempData["Msg"];
            return View();
        }

        public ActionResult PostEditPwd(UsersEditPwdViewModel user)
        {
            var result = UsersService.UpdatePwd(GetUserId(), user );
            TempData["Msg"] = result.ErrorMessage;
            return RedirectToAction("EditPwd");
        }

        public JsonResult GetByDepartmentId(int departmentId)
        {
            var result = UsersService.GetByDepartmentId(departmentId);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(int? cityId,int? townId)
        {
            UsersInfoFilterParameter filter = new UsersInfoFilterParameter { };

            if (cityId.HasValue)
            {
                filter.CityIds = cityId.Value.ToListCollection();

            }

            if (townId.HasValue)
            {
                filter.TownIds = townId.Value.ToListCollection();
            }

            var result = UsersService.GetUsersInfoByFilter(filter);

            return View(result);
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            UsersService.Delete(id);

            return new HttpStatusCodeResult(HttpStatusCode.OK); ;
        }
    }
}