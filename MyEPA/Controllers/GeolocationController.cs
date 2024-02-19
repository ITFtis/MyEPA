using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Services;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class GeolocationController : LoginBaseController
    {
        TownService TownService = new TownService();
        [HttpGet]
        public JsonResult Towns(int cityId, bool isByUser = false)
        {
            List<TownModel> result = null;

            if(isByUser && _Duty == DutyEnum.Cleaning)
            {
                var user = GetUserBrief();
                result = TownService.Get(user.TownId).ToListCollection();
            }
            else
            {
                result = TownService.GetByCityId(cityId);
            }

            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}