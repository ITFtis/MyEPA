using DocumentFormat.OpenXml.Office2010.Excel;
using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers.Rec
{
    public class OpenContractNewController : LoginBaseController
    {
        OpenContractService OpenContractService = new OpenContractService();
        ResourceTypeService ResourceTypeService = new ResourceTypeService();
        CityService CityService = new CityService();
        TownService TownService = new TownService();

        // GET: OpenContractNew
        public ActionResult Index()
        {
            OpenContractFilterParameter filter = new OpenContractFilterParameter
            {
            };

            var user = GetUserBrief();
            var duty = user.Duty;

            switch (duty)
            {
                case DutyEnum.EPA:
                case DutyEnum.Corps:
                case DutyEnum.Team:
                    break;
                default:
                    filter.CityIds = user.CityId.ToListCollection();
                    break;
            }

            var result = OpenContractService.GetCountListByFilter(filter);

            //排序
            result = result.OrderByDescending(a => a.CreateDate)                        
                        .ToList();

            ViewBag.Types = ResourceTypeService.GetList();

            return View(result);
        }
    }
}