using MyEPA.Enums;
using MyEPA.EPA.Attribute;
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
    [LoginRequired]
    public class DefendController : LoginBaseController
    {
        DefendService DefendService = new DefendService();
        DiasterService DiasterService = new DiasterService();
        TownService TownService = new TownService();
        CityService CityService = new CityService();
        public ActionResult Index(int? diasterId = null, int? townId = null)
        {
            var user = GetUserBrief();

            var towns = TownService.GetListByFilter(new TownFilterParameter 
            {
                CityIds = user.CityId.ToListCollection(),
                Ids = user.Duty == DutyEnum.Cleaning ? user.TownId.ToListCollection() : new List<int>()
            });

            ViewBag.TownId = townId.HasValue ? townId.Value : user.TownId;

            ViewBag.Towns = towns;

            var diasters = DiasterService.GetByFilter(new DiasterFilterParameter
            {
                IsRunning = true
            });

            if (diasterId.HasValue == false)
            {
                diasterId = diasters.FirstOrDefault()?.Id;
            }


            ViewBag.Diasters = diasters;
            ViewBag.DiasterId = diasterId;

            return View();
        }
        public ActionResult Edit(int? diasterId = null, int? townId = null)
        {
            var user = GetUserBrief();

            int cityId = GetUserCityId();
            ViewBag.UnNotifications = DefendService.GetUnNotifications(diasterId.Value, cityId);

            DefendViewModel defend = new DefendViewModel 
            {
                CityId = user.CityId,
                TownId = townId.HasValue ? townId.Value : user.TownId,
                DiasterId = diasterId
            };
            
            var result = DefendService.Get(GetUserName(), defend);


            return PartialView(result);
        }

        [HttpPost]
        public ActionResult Edit(DefendViewModel model)
        {
            DefendService.CreateOrUpdate(GetUserName(), model);
            return JsonResult(new
            {
                IsSuccess = true
            });
        }
        
        public ActionResult TeamConfirmList(int? diasterId = null)
        {
            var diasters = DiasterService.GetAll();
            if (diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }
            AreaEnum? area = GetArea();
            if (area.HasValue == false)
            {
                throw new NotImplementedException();
            }
            var result = DefendService.GetConfirmList(diasterId.Value, area.Value);

            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = diasters;
            return View(result);
        }
        public ActionResult ConfirmList(int? diasterId = null)
        {
            var diasters = 
                DiasterService.GetAll()
                .OrderByDescending(e => e.Status)
                .ThenByDescending(e => e.Id)
                .ToList();

            if(diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }


            int cityId = GetUserCityId();
            var result = DefendService.GetConfirmList(diasterId.Value, cityId);

            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = diasters;
            ViewBag.City = GetUserCity();
            ViewBag.UnNotifications = DefendService.GetUnNotifications(diasterId.Value, cityId);
            return View(result);
        }

        public ActionResult Confirm(int id, DefendStatusEnum status)
        {
            DefendService.Confirm(GetUserBrief(),id, status);
            return JsonResult(new
            {
                IsSuccess = true
            });
        }

        public ActionResult Report(int? diasterId = null,int? cityId = null)
        {
            var diasters = DiasterService.GetAll();
            if (diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }
            ViewBag.Diasters = diasters;
            ViewBag.DiasterId = diasterId;
           
            ViewBag.CityId = cityId;
            ViewBag.Citys = CityService.GetAll();
            var result = DefendService.GetReport(new DefendReportFilterModel 
            {
                DiasterId = diasterId.Value,
                CityId = cityId
            });
            return View(result);
        }
        private string GetDiasterName(int? diasterId)
        {
            if(diasterId.HasValue == false)
            {
                return string.Empty;
            }
            return DiasterService.GetByFilter(new DiasterFilterParameter
            {
                Ids = diasterId.Value.ToListCollection()
            }).Select(e => e.DiasterName).FirstOrDefault();
        }
        public ActionResult DownReportPDF(int? diasterId, int? cityId = null)
        {
            
            var model = DefendService.GetReport(new DefendReportFilterModel
            {
                DiasterId = diasterId.Value,
                CityId = cityId
            });
            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("TownId");
            ignoreFields.Add("CityId");

            return File(GeneratePDF(model, $"整備通報 [{GetDiasterName(diasterId)}]", ignoreFields));
        }
        public ActionResult DownReportODS(int? diasterId, int? cityId = null)
        {
            var model = DefendService.GetReport(new DefendReportFilterModel
            {
                DiasterId = diasterId.Value,
                CityId = cityId
            });
            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("TownId");
            ignoreFields.Add("CityId");

            return File(GenerateODS(model, $"整備通報 [{GetDiasterName(diasterId)}]", ignoreFields));
        }
        public ActionResult TownReport(int diasterId,int cityId)
        {
            ViewBag.DiasterId = diasterId;
            ViewBag.CityId = cityId;
            var result = DefendService.GetTownReport(new DefendTownReportFilterModel
            {
                DiasterId = diasterId,
                CityId = cityId
            });
            return View(result);
        }
        private string GetCityName(int? cityId)
        {
            if(cityId.HasValue == false)
            {
                return string.Empty;
            }
            return CityService.Get(cityId.Value)?.City;
        }
        public ActionResult DownTownReportPDF(int diasterId, int cityId)
        {
            var result = DefendService.GetTownReport(new DefendTownReportFilterModel
            {
                DiasterId = diasterId,
                CityId = cityId
            });
            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("TownId");
            ignoreFields.Add("CityId");

            return File(GeneratePDF(result, $"整備通報-[{GetCityName(cityId)}] [{GetDiasterName(diasterId)}]", ignoreFields));
        }
        public ActionResult DownTownReportODS(int diasterId, int cityId)
        {
            var result = DefendService.GetTownReport(new DefendTownReportFilterModel
            {
                DiasterId = diasterId,
                CityId = cityId
            });
            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("TownId");
            ignoreFields.Add("CityId");

            return File(GenerateODS(result, $"整備通報-[{GetCityName(cityId)}] [{GetDiasterName(diasterId)}]", ignoreFields));
        }
        public ActionResult TownQuestions(int diasterId, int? townId, int? cityId = null)
        {
            var diaster = DiasterService.GetByFilter(new DiasterFilterParameter
            {
                Ids = diasterId.ToListCollection()
            }).FirstOrDefault();
            
            ViewBag.DiasterId = diasterId;
            ViewBag.DiasterName = diaster?.DiasterName;
            ViewBag.TownId = townId;
            ViewBag.CityId = cityId;

            var result = GetTownAns(diasterId, townId, cityId);

            return View(result);
        }
        
        public ActionResult DownTownQuestionsFile(int diasterId, string file, int? townId, int? cityId = null)
        {
            var result = GetTownAns(diasterId, townId, cityId);

            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("TownId");
            ignoreFields.Add("CityId");
            ignoreFields.Add("Sort");
            
            if (file == "PDF")
            {
                return File(GeneratePDF(result, $"整備通報-[{GetCityName(cityId)}] [{GetDiasterName(diasterId)}]", ignoreFields));
            }
            return File(GenerateODS(result, $"整備通報-[{GetCityName(cityId)}] [{GetDiasterName(diasterId)}]", ignoreFields));
        }
        private List<DefendTownQuestionModel> GetTownAns(int diasterId, int? townId, int? cityId = null)
        {
            if (townId.HasValue)
            {
                var town = TownService.Get(townId.Value);
                if (town != null)
                {
                    ViewBag.TownName = town.Name;
                    cityId = town.CityId;
                }
            }

            if (cityId.HasValue)
            {
                var city = CityService.Get(cityId.Value);
                ViewBag.CityName = city?.City;
            }

            var result = DefendService.GetTownAns(diasterId, townId, cityId);

            return result;
        }
    }
}