using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Models.SearchViewModel;
using MyEPA.Services;
using MyEPA.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyEPA.Controllers
{
    public class UserEPAController : LoginBaseController
    {
        UserEPAService UserEPAService = new UserEPAService();
        private List<int> GetDutys()
        {
            List<int> dutyIds = new List<int>();
            dutyIds.Add(DutyEnum.EPA.ToInteger());
            dutyIds.Add(DutyEnum.Team.ToInteger());
            return dutyIds;
        }
        public ActionResult GetSelectListItem(UserEPASearchViewModel searchModel,int? id)
        {
            if (string.IsNullOrEmpty(searchModel.SearchDepartmentName) == false)
            {
                ContactManualDepartmentViewModel department =
                    new ContactManualDepartmentService()
                    .GetByName(searchModel.SearchDepartmentName);

                searchModel.SearchDepartmentId = department?.Id;
            }

            UsersFilterParameter usersFilter = new UsersFilterParameter
            {
                ContactManualDepartmentIds = searchModel?.SearchDepartmentId?.ToListCollection(),
                DutyIds = GetDutys(),
                PositionIds = searchModel?.SearchPositionId?.ToListCollection(),
                TownIds = searchModel?.SearchTownId?.ToListCollection(),
                Name = searchModel?.SearchName,
                ContactManualDutys = searchModel?.ContactManualDuty.ToInteger().ToListCollection()
            };

            var result = UserEPAService.GetListByFilter(usersFilter).Select(e => new SelectListItem
            {
                Text = $"{e.Name} {e.DepartmentName}",
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
        public ActionResult Index(UserEPASearchPaginationViewModel searchModel, PaginationModel pagination)
        {
            if (searchModel == null)
            {
                searchModel = new UserEPASearchPaginationViewModel();
            }
            if (pagination == null)
            {
                pagination = new PaginationModel();
            }
            var user = GetUserBrief();
            if (user.ContactManualDuty != ContactManualDutyEnum.Administrator)
            {
                searchModel.SearchDepartmentId = user.ContactManualDepartmentId;
            }
            ViewBag.SearchModel = searchModel;
            UsersFilterPaginationParameter usersFilter = new UsersFilterPaginationParameter
            {
                ContactManualDepartmentIds = searchModel.SearchDepartmentId?.ToListCollection(),
                DutyIds = GetDutys(),
                PositionIds = searchModel.SearchPositionId?.ToListCollection(),
                TownIds = searchModel.SearchTownId?.ToListCollection(),
                Name = searchModel.SearchName,
                Pagination = pagination
            };
            if (string.IsNullOrWhiteSpace(usersFilter.Pagination.SortBy))
            {
                usersFilter.Pagination.SortBy = nameof(UsersModel.Id);
            }
            var result = UserEPAService.GetPagingList(usersFilter);
            return View(result);
        }

        public ActionResult Edit(int? uid = null)
        {
            if (uid.HasValue)
            {
                var result = UserEPAService.Get(uid.Value);
                return PartialView(result);
            }
            return View(new UserEPAViewModel());
        }

        [HttpPost]
        public ActionResult Edit(UserEPAViewModel model, UserEPASearchPaginationViewModel searchModel, PaginationModel pagination)
        {
            searchModel.Pagination = pagination;
            if (model.Id.HasValue)
            {
                UserEPAService.Update(model);
            }
            else
            {
                UserEPAService.Create(model);
            }

            RouteValueDictionary routeValue = searchModel.ToRouteValueDictionary();

            return RedirectToAction("Index", routeValue);
        }

        public ActionResult A9x9()
        {
            return View("~/Views/EPA/A9x9.cshtml");
        }
    }
}
