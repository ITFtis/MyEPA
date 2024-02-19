using MyEPA.Enums;
using MyEPA.EPA.Attribute;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Services;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    [LoginRequired]
    public class DamageController : LoginBaseController
    {
        DamageMainService DamageMainService = new DamageMainService();
        DamageService DamageService = new DamageService();
        DiasterService DiasterService = new DiasterService();
        TownService TownService = new TownService();
        CityService CityService = new CityService();
        LandfillService LandfillService = new LandfillService();
        FileDataService FileDataService = new FileDataService();

        public ActionResult Index(int? diasterId = null, int? cityId = null,int? townId = null, DateTime? startTime = null,DateTime? endTime = null)
        {
            List<DiasterModel> diasters = DiasterService.GetAll();
            
            DamageFilterParameter filter = new DamageFilterParameter()
            {
                CityIds = new List<int>(),
                TownIds = new List<int>(),
                DiasterIds = new List<int>(),
                EndTime = endTime,
                StartTime = startTime
            };

            if (cityId.HasValue)
            {
                filter.CityIds.Add(cityId.Value);
            }
            if(townId.HasValue)
            {
                filter.TownIds.Add(townId.Value);
            }
            if (diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }

            filter.DiasterIds.Add(diasterId.Value);

            ViewBag.DiasterId = diasterId;
            ViewBag.CityId = cityId;
            ViewBag.TownId = townId;
            ViewBag.StartTime = startTime;
            ViewBag.EndTime = endTime;
            ViewBag.Diasters = diasters;
            ViewBag.Citys = CityService.GetCountyOrderBySort();

            return View(DamageService.GetByFilter(filter));
        }

        public ActionResult Details(int id)
        {
            var user = GetUserBrief();

            var result = DamageService.Get(id);

            ViewBag.Landfills = new LandfillService().GetByFilter(new LandfillFilterParameter
            {
                City = user.City
            }).Select(e => new SelectListItem { Text = e.ContactUnit, Value = e.Id.ToString() }).ToList();

            ViewBag.Incinerators = new IncineratorService().GetByFilter(new IncineratorFilterParameter
            {
                City = user.City
            }).Select(e => new SelectListItem { Text = e.ContactUnit, Value = e.Id.ToString() }).ToList();

            ViewBag.Files = FileDataService.GetBySource(SourceTypeEnum.DamageFile, id);
            ViewBag.Images = FileDataService.GetBySource(SourceTypeEnum.DamageImage, id);
            return View(result);
        }
        public ActionResult Edit(int id)
        {
            var user = GetUserBrief();

            var result = DamageService.Get(id);
            
            ViewBag.Landfills = new LandfillService().GetByFilter(new LandfillFilterParameter
            {
                City = user.City
            }).Select(e => new SelectListItem { Text = e.ContactUnit, Value = e.Id.ToString() }).ToList();

            ViewBag.Incinerators = new IncineratorService().GetByFilter(new IncineratorFilterParameter
            {
                City = user.City
            }).Select(e => new SelectListItem { Text = e.ContactUnit, Value = e.Id.ToString() }).ToList();
            ViewBag.Files = FileDataService.GetBySource(SourceTypeEnum.DamageFile, id);
            ViewBag.Images = FileDataService.GetBySource(SourceTypeEnum.DamageImage, id);
            return View(result);
        }

        public ActionResult del(int id)
        {
             SqlConnection X = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyData"].ConnectionString.ToString());
             X.Open();
            string G = "DELETE FROM [dbo].[Damage] where Id=@id";
            SqlCommand Q = new SqlCommand(G, X);
            Q.Parameters.AddWithValue("@id", id);
            Q.ExecuteNonQuery();
            X.Close();
            return RedirectToAction("B1c", "EPBxDamage");
        }
        



        [HttpPost]
        public ActionResult Edit(DamageViewModel model)
        {
            DamageService.Update(GetUserBrief(), model, GetUploadFiles());

            return RedirectToAction("B1c", "EPBxDamage", new { diasterId = model.DiasterId });

        }

        public ActionResult TownList(int diasterId, DateTime? date, int? cityId = null)
        {
            var result = DamageService.GetTownList(diasterId, date, cityId);
            return View(result);
        }
        public ActionResult TeamConfirmList(int? diasterId = null,int? cityId = null)
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
            var result = DamageService.GetConfirmList(diasterId.Value, area.Value, cityId);

            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = diasters;
            ViewBag.Citys = CityService.GetListByFilter(new CityFilterParameter 
            {
                AreaIds = area.Value.ToInteger().ToListCollection(),
                IsCounty = true
            });

            return View(result);
        }
        public ActionResult Confirm(int id, DamageStatusEnum status)
        {
            DamageService.Confirm(GetUserBrief(), id, status);
            return JsonResult(new
            {
                IsSuccess = true
            });
        }
        /// <summary>
        /// 狀態改為無災情
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult NotDamage(DamageModel model)
        {
            DamageService.NotDamage(GetUserBrief(), model);
            return JsonResult(new
            {
                IsSuccess = true
            });
        }
        /// <summary>
        /// 環保局結案
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Done(int diasterId)
        {
            DamageMainService.UpdateByDone(diasterId,GetUserCityId());
            return JsonResult(new
            {
                IsSuccess = true
            });
        }
        /// <summary>
        /// 災情通報
        /// </summary>
        /// <param name="diasterId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public ActionResult Report(int? diasterId,int? cityId = null)
        {
            var diasters = DiasterService.GetAll();
            if(diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }
            ViewBag.Diasters = diasters;
            ViewBag.DiasterId = diasterId;
            ViewBag.CityId = cityId;
            ViewBag.Citys = CityService.GetCountyOrderBySort();
            var result = DamageService.GetReport(new DamageReportFilterModel 
            {
                DiasterId = diasterId.Value,
                CityId = cityId
            });
            return View(result);
        }
        public ActionResult DownReportPDF(int? diasterId, int? cityId = null)
        {
            var model = DamageService.GetReport(new DamageReportFilterModel
            {
                DiasterId = diasterId.Value,
                CityId = cityId
            });
            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("UpdateDate");
            ignoreFields.Add("Status");
            ignoreFields.Add("CityId");
            
            return File(GeneratePDF(model, "災情通報", ignoreFields));
        }
        public ActionResult DownReportODS(int? diasterId, int? cityId = null)
        {
            var model = DamageService.GetReport(new DamageReportFilterModel
            {
                DiasterId = diasterId.Value,
                CityId = cityId
            });
            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("UpdateDate");
            ignoreFields.Add("Status");
            ignoreFields.Add("CityId");

            return File(GenerateODS(model, "災情通報", ignoreFields));
        }
        public ActionResult TownReport(int diasterId,int cityId)
        {
            ViewBag.DiasterId = diasterId;
            var result = DamageService.GetTownReport(new DamageTownReportFilterModel
            {
                DiasterId = diasterId,
                CityId = cityId
            });
            return View(result);
        }
        /// <summary>
        /// 災情統計
        /// </summary>
        /// <param name="diasterId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ActionResult CityStatistics(int? diasterId = null, DateTime? date = null)
        {
            var diasters = DiasterService.GetAll();
            if(diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }
            ViewBag.Diasters = diasters;
            ViewBag.Diaster = diasters.FirstOrDefault(e => e.Id == diasterId.Value);
            ViewBag.Date = date;
            var result = DamageService.GetCityStatistics(new DamageReportFilterModel
            {
                DiasterId = diasterId.Value,
                Date = date
            });

            return View(result);
        }
        /// <summary>
        /// 災情統計-鄉鎮
        /// </summary>
        /// <param name="diasterId"></param>
        /// <param name="cityId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public ActionResult TownStatistics(int diasterId,int cityId, DateTime? date = null)
        {
            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = DiasterService.GetAll();

            var result = DamageService.GetTownStatistics(new DamageReportFilterModel
            {
                DiasterId = diasterId,
                CityId = cityId,
                Date = date
            });

            return View(result);
        }
        /// <summary>
        /// 環境設施災損查詢
        /// </summary>
        /// <param name="diasterId"></param>
        /// <param name="cityId"></param>
        /// <param name="townId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult FacilityDamage(int? diasterId,int? cityId, int? townId, FacilityDamageTypeEnum type = FacilityDamageTypeEnum.ALL)
        {
            List<DiasterModel> diasters = DiasterService.GetAll();

            if (diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }
            ViewBag.Type = type;
            ViewBag.Citys = CityService.GetAll();
            ViewBag.Towns = TownService.GetAll();
            ViewBag.CityId = cityId;
            ViewBag.TownId = townId;
            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = diasters;
            ViewBag.FacilityDamageTypes = ExtensionsOfEnum.GetEnumAllValue<FacilityDamageTypeEnum>();
            var filter = new DamageFilterParameter();


            filter = new DamageFilterParameter()
            {
                DiasterIds = diasterId.Value.ToListCollection(),
                Type = type,
                CityIds = cityId.HasValue ? cityId.Value.ToListCollection() : new List<int>(),
                TownIds = townId.HasValue ? townId.Value.ToListCollection() : new List<int>(),
            };

            var result = DamageService.GetFacilityDamages(filter, type);
            return View(result);
        }
        public ActionResult CorpsHandlingSituationShow(int id, FacilityDamageTypeEnum type)
        {
            var result = DamageService.GetDamage(id);
            ViewBag.Type = type;
            return View(result);
        }

        public ActionResult CorpsHandlingSituation(int id, FacilityDamageTypeEnum type)
        { 
            var result = DamageService.GetCorpsHandlingSituation(id, type);

            return View(result);
        }
        [HttpPost]
        public ActionResult CorpsHandlingSituation(CorpsHandlingSituationViewModel model)
        {
            DamageModel damage = DamageService.UpdateCorpsHandlingSituation(model);
            
            return RedirectToAction("FacilityDamage", "Damage",new { diasterId  = damage.DiasterId, cityId = damage.CityId,townId = damage.TownId,type = model.Type });
        }

        public ActionResult MemoShow(int id, FacilityDamageTypeEnum type)
        {
            var result = DamageService.GetDamage(id);
            ViewBag.Type = type;
            return View(result);
        }

        public ActionResult Memo(int id, FacilityDamageTypeEnum type)
        {
            var result = DamageService.GetDamageMemo(id, type);

            return View(result);
        }
        [HttpPost]
        public ActionResult Memo(DamageMemoViewModel model)
        {
            DamageModel damage = DamageService.UpdateMemo(model);

            return RedirectToAction("FacilityDamage", "Damage", new { diasterId = damage.DiasterId, cityId = damage.CityId, townId = damage.TownId, type = model.Type });
        }
        

    }
}