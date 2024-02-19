using MyEPA.Extensions;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class TownController : Controller
    {
        TownService TownService = new TownService();
        CityService CityService = new CityService();
        public ActionResult GetSelectListItemByCityName(string cityName,int? id)
        {
            ViewBag.Id = id;

            var result = TownService.GetListByFilter(new Models.FilterParameter.TownFilterParameter
            {
                CityIds = CityService.GetByCityName(cityName).Id.ToListCollection()
            });

            return PartialView(result);
        }
        public ActionResult GetSearchSelectListItem(int? cityId, int? id, bool? isTown = null)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem
            {
                Text = "請選擇",
                Value = null
            });

            if (cityId.HasValue == false)
            {
                return PartialView(result);
            }

            ViewBag.Id = id;

            TownService.GetListByFilter(new Models.FilterParameter.TownFilterParameter
            {
                CityIds = cityId.Value.ToListCollection(),
                IsTown = isTown
            }).ForEach(town =>
            {
                result.Add(new SelectListItem
                {
                    Text = town.Name,
                    Value = town.Id.ToString(),
                    Selected = town.Id == id
                });
            });

            return PartialView(result);
        }
        public ActionResult GetSelectListItem(int? cityId, int? id, bool? isTown = null)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            result.Add(new SelectListItem
            {
                Text = "請選擇",
                Value = ""
            });

            if (cityId.HasValue == false)
            {
                return PartialView(result);
            }

            ViewBag.Id = id;

            TownService.GetListByFilter(new Models.FilterParameter.TownFilterParameter
            {
                CityIds = cityId.Value.ToListCollection(),
                IsTown = isTown
            }).ForEach(town =>
            {
                result.Add(new SelectListItem
                {
                    Text = town.Name,
                    Value = town.Id.ToString(),
                    Selected = town.Id == id
                });
            });

            return PartialView(result);
        }
    }
}