using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class InfectiousDiseaseController : LoginBaseController
    {
        InfectiousDiseaseService InfectiousDiseaseService = new InfectiousDiseaseService();
        CityService CityService = new CityService();
        TownService TownService = new TownService();
        public ActionResult Index(int? cityId = null, int? townId = null, DateTime? date = null, string msg = null)
        {
            InfectiousDiseaseFilterParameter filter = new InfectiousDiseaseFilterParameter()
            {
                Date = date
            };
          
            var user = GetUserBrief();

            switch (user.Duty)
            {
                case DutyEnum.EPA:
                case DutyEnum.Corps:
                case DutyEnum.Team:
                    filter.CityIds = cityId.HasValue ? cityId.Value.ToListCollection() : new List<int> { };
                    filter.TownIds = townId.HasValue ? townId.Value.ToListCollection() : new List<int> { };
                    break;

                case DutyEnum.EPB:
                    filter.CityIds = user.CityId.ToListCollection();
                    filter.TownIds = townId.HasValue ? townId.Value.ToListCollection() : new List<int> { };
                    break;
                case DutyEnum.Cleaning:
                    filter.TownIds = user.TownId.ToListCollection();
                    break;
                default:
                    throw new NotImplementedException();
            }
            ViewBag.Msg = msg;
            ViewBag.TownId = townId;
            ViewBag.CityId = cityId;
            ViewBag.Date = date;
            SetCitys(user);

            var result = InfectiousDiseaseService.GetByFilter(filter).OrderByDescending(e => e.Date);
            
            return View(result);
        }
        private void SetCitys(UserBriefModel user)
        {
            switch (user.Duty)
            {
                case DutyEnum.EPA:
                case DutyEnum.Corps:
                case DutyEnum.Team:
                    ViewBag.Citys = CityService.GetCountyOrderBySort();
                    break;

                case DutyEnum.EPB:
                    ViewBag.Citys = CityService.Get(user.CityId).ToListCollection();
                    break;
                case DutyEnum.Cleaning:
                    ViewBag.Citys = CityService.Get(user.CityId).ToListCollection();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public ActionResult Create()
        {
            var user = GetUserBrief();

            SetCitys(user);

            var now = DateTimeHelper.GetCurrentTime().Date;
            return View(new InfectiousDiseaseModel 
            { 
                Date = now
            });
        }

        [HttpPost]
        public ActionResult Create(InfectiousDiseaseModel model)
        {
            var result = InfectiousDiseaseService.Create(GetUserBrief(),model);
            return RedirectToIndex(result.ErrorMessage);
        }

        public ActionResult Edit(int id)
        {
            var result = InfectiousDiseaseService.Get(id);

            var user = GetUserBrief();

            SetCitys(user);

            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(InfectiousDiseaseModel model)
        {
            var result = InfectiousDiseaseService.Update(GetUserBrief(), model);
            return RedirectToIndex(result.ErrorMessage);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            AdminResultModel result = InfectiousDiseaseService.Delete(id);
            return JsonResult(result);
        }

        public ActionResult CityStatistics(DateTime? startDate = null,DateTime? endDate = null)
        {
            var result = InfectiousDiseaseService.GetCityStatistics(new InfectiousDiseaseFilterParameter
            {
                StartDate = startDate,
                EndDate = endDate
            });

            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(result);
        }

        public ActionResult TownStatistics(int cityId, DateTime? startDate = null, DateTime? endDate = null)
        {

            var result = InfectiousDiseaseService.GetTownStatistics(new InfectiousDiseaseFilterParameter
            {
                StartDate = startDate,
                EndDate = endDate,
                CityIds = cityId.ToListCollection()
            });

            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(result);
        }

        private RedirectToRouteResult RedirectToIndex(string msg = null)
        {
            return RedirectToAction("Index",new { msg });
        }
    }
}