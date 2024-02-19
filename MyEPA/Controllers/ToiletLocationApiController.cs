using MyEPA.Extensions;
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
    public class ToiletLocationApiController : LoginBaseController
    {
        ToiletLocationService ToiletLocationService = new ToiletLocationService();
        public JsonResult Index(int? diasterId = null, int? managementTownId = null)
        {
            var result = ToiletLocationService.GetByFilter(new ToiletLocationFilterParameter
            {
                ManagementTownIds = managementTownId.HasValue ? managementTownId.Value.ToListCollection() : new List<int>(),
                DiasterIds = diasterId.Value.ToListCollection()
            });

            return this.Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ManagementTown(int? diasterId = null)
        {
            var result = ToiletLocationService.GetManagementTownByFilter(new ToiletLocationFilterParameter
            {
                DiasterIds = diasterId.Value.ToListCollection()
            });
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}