using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class CityController : Controller
    {
        CityService CityService = new CityService();
        private List<CityModel> GetByType(CityTypeEnum type)
        {
            var result = CityService.GetListByFilter(new CityFilterParameter
            {
                Types = type.ToInteger().ToListCollection()
            });
            return result;
        }
        public ActionResult GetSelectListItem(int? id, DutyEnum duty = DutyEnum.EPB)
        {
            var result = GetCitySelectListItem(id, duty);
            return PartialView(result);
        }
        public ActionResult GetSearchSelectListItem(int? id, DutyEnum? duty = null)
        {
            var result = GetCitySelectListItem(id, duty);
            return PartialView(result);
        }
        private List<SelectListItem> GetCitySelectListItem(int? id, DutyEnum? duty = null)
        {
            List<SelectListItem> result = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = "請選擇",
                    Value = ""
                }
            };
            if (duty.HasValue == false)
            {
                return result;
            }
            CityTypeEnum type = CityTypeEnum.General;
            if (duty == DutyEnum.EPA)
                type = CityTypeEnum.EPA;
            else if (duty == DutyEnum.Water)
                type = CityTypeEnum.Water;

            GetByType(type).ForEach(e =>
            {
                result.Add(new SelectListItem
                {
                    Text = e.City,
                    Value = e.Id.ToString(),
                    Selected = e.Id == id
                });
            });
            return result;
        }
    }
}