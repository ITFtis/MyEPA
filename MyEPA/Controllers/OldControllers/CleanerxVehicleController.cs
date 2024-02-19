using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.Services;

namespace MyEPA.Controllers
{
    public class CleanerxVehicleController : LoginBaseController
    {
        VehicleRepository VehicleRepository = new VehicleRepository();
        public ActionResult C3x1Vehicle(int? townId = null)
        {

            VehicleFilterParameter filter = new VehicleFilterParameter();

            UserBriefModel user = GetUserBrief();

            switch (_Duty)
            {
                case Enums.DutyEnum.Cleaning:
                    filter.CityIds = user.CityId.ToListCollection();
                    filter.TownIds = user.TownId.ToListCollection();
                    ViewBag.TownId = user.TownId;
                    ViewBag.Towns = new TownService().GetListByFilter(new TownFilterParameter
                    {
                        Ids = user.TownId.ToListCollection()
                    });
                    break;
                case Enums.DutyEnum.EPB:
                    filter.CityIds = user.CityId.ToListCollection();
                    if(townId.HasValue)
                    {
                        filter.TownIds = townId.Value.ToListCollection();
                    }
                    ViewBag.Towns = new TownService().GetListByFilter(new TownFilterParameter 
                    {
                        CityIds = user.CityId.ToListCollection(),
                    });
                    ViewBag.TownId = townId;
                    break;
                default:
                    break;
            }
            ViewBag.Data = VehicleRepository.GetByFilter(filter);
            ViewBag.City = user.City;
            ViewBag.Town = user.Town;
            return View("~/Views/Cleaner/C3x1Vehicle.cshtml");
        }
        public ActionResult Confirm(int? townId = null)
        {
            var user = GetUserBrief();

            VehicleFilterParameter filter = new VehicleFilterParameter();

            if (user.Duty == DutyEnum.Cleaning)
            {
                filter.CityIds = user.CityId.ToListCollection();
                filter.TownIds = user.TownId.ToListCollection();
            }
            else if (user.Duty == DutyEnum.EPB)
            {
                filter.CityIds = user.CityId.ToListCollection();
                if(townId.HasValue)
                {
                    filter.TownIds = townId.Value.ToListCollection();
                }
            }
            VehicleRepository.UpdateConfirmTimeByFilter(filter);

            return RedirectToAction("C3x1Vehicle", "CleanerxVehicle",new { townId });
        }
    }
}