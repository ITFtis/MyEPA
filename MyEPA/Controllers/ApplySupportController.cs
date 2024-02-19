using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    /// <summary>
    /// 請求支援通報用 controller
    /// </summary>
    public class ApplySupportController : LoginBaseController
    {
        ApplySupportService ApplySupportService = new ApplySupportService();
        DiasterService DiasterService = new DiasterService();
        CityService CityService = new CityService();
        TownService TownService = new TownService();

        public ActionResult Index(int? diasterId = null, int? cityId = null, int applyTypeId = 0)
        {
            var diasters = DiasterService.GetAll();
            if (diasterId.HasValue == false)
            {
                diasterId = diasters.Any() ? diasters.FirstOrDefault().Id : diasterId;
            }

            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = diasters;
            ViewBag.CityId = cityId;
            ViewBag.Citys = CityService.GetAll();
            ViewBag.ApplyTypeId = applyTypeId;
            ViewBag.ApplyTypes = GetTypes();

            var result = ApplySupportService.GetReport(new ApplySupportReportFilterModel
            {
                DiasterId = diasterId.Value,
                CityId = cityId,
                ApplyTypeId = applyTypeId
            });

            return View(result);
        }
        public ActionResult Search(int? diasterId = null, int? cityId = null, int applyTypeId = 0)
        {
            var diasters = DiasterService.GetAll();
            if (diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }
            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = diasters;
            ViewBag.CityId = cityId;
            ViewBag.Citys = CityService.GetAll();
            ViewBag.ApplyTypeId = applyTypeId;
            ViewBag.ApplyTypes = GetTypes();

            var result = ApplySupportService.GetReport(new ApplySupportReportFilterModel
            {
                DiasterId = diasterId.Value,
                CityId = cityId,
                ApplyTypeId = applyTypeId
            });

            return View(result);
        }

        public ActionResult DownIndexPDF(int diasterId = 0, int? cityId = null, int applyTypeId = 0)
        {
            var model = ApplySupportService.GetReport(new ApplySupportReportFilterModel
            {
                DiasterId = diasterId,
                CityId = cityId,
                ApplyTypeId = applyTypeId
            });

            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("Status");
            ignoreFields.Add("CreateDate");
            ignoreFields.Add("ContactPerson");
            ignoreFields.Add("Quantity");
            ignoreFields.Add("TownId");
            ignoreFields.Add("CityId");
            ignoreFields.Add("DiasterId");
            ignoreFields.Add("EPAConfirmStatus");
            ignoreFields.Add("EPBConfirmStatus");
            ignoreFields.Add("EPAConfirmDescribe");
            ignoreFields.Add("EPBConfirmDescribe");
            ignoreFields.Add("EPAConfirmUpdateTime");
            ignoreFields.Add("EPBConfirmUpdateTime");
            ignoreFields.Add("Id");

            return FileByPDF(model.Details, $"請求支援通報-{((ApplyTypeEnum)applyTypeId).GetDescription()}", ignoreFields);
        }
        public ActionResult DownIndexODS(int diasterId = 0, int? cityId = null, int applyTypeId = 0)
        {
            var model = ApplySupportService.GetReport(new ApplySupportReportFilterModel
            {
                DiasterId = diasterId,
                CityId = cityId,
                ApplyTypeId = applyTypeId
            });

            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("Status");
            ignoreFields.Add("CreateDate");
            ignoreFields.Add("ContactPerson");
            ignoreFields.Add("Quantity");
            ignoreFields.Add("TownId");
            ignoreFields.Add("CityId");
            ignoreFields.Add("DiasterId");
            ignoreFields.Add("EPAConfirmStatus");
            ignoreFields.Add("EPBConfirmStatus");
            ignoreFields.Add("EPAConfirmDescribe");
            ignoreFields.Add("EPBConfirmDescribe");
            ignoreFields.Add("EPAConfirmUpdateTime");
            ignoreFields.Add("EPBConfirmUpdateTime");
            ignoreFields.Add("Id");

            return FileByODS(model.Details, $"請求支援通報-{((ApplyTypeEnum)applyTypeId).GetDescription()}", ignoreFields);
        }
        public ActionResult DownSearchPDF(int diasterId = 0, int? cityId = null, int applyTypeId = 0)
        {
            var model = ApplySupportService.GetReport(new ApplySupportReportFilterModel
            {
                DiasterId = diasterId,
                CityId = cityId,
                ApplyTypeId = applyTypeId
            });

            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("EPAConfirmStatus");
            ignoreFields.Add("EPBConfirmStatus");
            ignoreFields.Add("ContactPerson");
            ignoreFields.Add("CreateUser");
            ignoreFields.Add("ContactPhone");
            ignoreFields.Add("Quantity");
            ignoreFields.Add("TownId");
            ignoreFields.Add("CityId");
            ignoreFields.Add("DiasterId");
            ignoreFields.Add("Id");
            
            return FileByPDF(model.Details, $"請求支援處理查詢-{((ApplyTypeEnum)applyTypeId).GetDescription()}", ignoreFields);
        }
        public ActionResult DownSearchODS(int diasterId = 0, int? cityId = null, int applyTypeId = 0)
        {
            var model = ApplySupportService.GetReport(new ApplySupportReportFilterModel
            {
                DiasterId = diasterId,
                CityId = cityId,
                ApplyTypeId = applyTypeId
            });

            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("EPAConfirmStatus");
            ignoreFields.Add("EPBConfirmStatus");
            ignoreFields.Add("ContactPerson");
            ignoreFields.Add("CreateUser");
            ignoreFields.Add("ContactPhone");
            ignoreFields.Add("Quantity");
            ignoreFields.Add("TownId");
            ignoreFields.Add("CityId");
            ignoreFields.Add("DiasterId");
            ignoreFields.Add("Id");

            return FileByODS(model.Details, $"請求支援處理查詢-{((ApplyTypeEnum)applyTypeId).GetDescription()}", ignoreFields);
        }
        public ActionResult CityStatusReport(int? diasterId)
        {
            var diasters = DiasterService.GetAll();

            if(diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }

            var result = ApplySupportService.GetCityStatusReportCountByFilter(new ApplySupportReportFilterModel
            {
                DiasterId = diasterId.Value
            });
            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = diasters;
            return View(result);
        }
        public ActionResult EPBStatusCount(int diasterId)
        {
            var result = ApplySupportService.GetEPBStatusCount(diasterId, GetUserCityId());

            return JsonResult(result);
        }
        public ActionResult TownStatusReport(int diasterId,int cityId)
        {
            var result = ApplySupportService.GetTownStatusReportCountByFilter(new ApplySupportReportFilterModel
            {
                DiasterId = diasterId,
                CityId = cityId
            });

            return View(result);
        }

        /// <summary>
        /// 環保署身分 Menu 用的 請求核定(補助款)
        /// </summary>
        public ActionResult SubsidyReport(int? diasterId = null)
        {
            var diasters = DiasterService.GetAll();
            if (diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }
            ViewBag.Diasters = diasters;
            ViewBag.DiasterId = diasterId;

            //ViewBag.ApplyStatuses = GetApplyStatus();
            //ViewBag.ApplyStatusId = applyStatusId;

            var model = ApplySupportService.GetSubsidyCountingReport(diasterId.Value);

            return View(model);
        }

        public ActionResult SubsidyReportDownPDF(int diasterId)
        {
            var model = ApplySupportService.GetSubsidyCountingReport(diasterId);
          
            return File(GeneratePDFByManyTable(model, "請求核定(補助款)"));
        }
        public ActionResult SubsidyReportDownExcel(int diasterId)
        {
            var model = ApplySupportService.GetSubsidyCountingReport(diasterId);

            return File(GenerateExcelByManyTable(model, "請求核定(補助款)"));
        }
        public ActionResult SubsidyReportDownODS(int diasterId)
        {
            var model = ApplySupportService.GetSubsidyCountingReport(diasterId);

            return File(GenerateODSByManyTable(model, "請求核定(補助款)"));
        }

        /// <summary>
        /// 環保署身分 Menu 用的 請求核定(處理情形)
        /// </summary>
        public ActionResult ProcessReport(int? diasterId = null, int? cityId = null, int? townId = null, int applyTypeId = 0)
        {
            var diasters = DiasterService.GetAll();

            if(diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }
            ViewBag.CityId = cityId;
            ViewBag.Citys = CityService.GetAll();

            ViewBag.TownId = townId;
            if (cityId.HasValue) 
            {
                ViewBag.Towns = TownService.GetByCityId(cityId.Value);
            }

            ViewBag.ApplyTypeId = applyTypeId;
            ViewBag.ApplyTypes = GetTypes();
            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = diasters;

            var result = ApplySupportService.ProcessReport(new ApplySupportReportFilterModel
            {
                DiasterId = diasterId.Value,
                CityId = cityId,
                ApplyTypeId = applyTypeId
            });

            return View(result);
        }
        public ActionResult ProcessReportDownPDF(int diasterId = 0, int? cityId = null, int applyTypeId = 0)
        {
            var model = ApplySupportService.ProcessReport(new ApplySupportReportFilterModel
            {
                DiasterId = diasterId,
                CityId = cityId,
                ApplyTypeId = applyTypeId
            });

            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("Id");
            ignoreFields.Add("ApplyType");
            ignoreFields.Add("UserId");
            ignoreFields.Add("UserName");
            ignoreFields.Add("EPAConfirmStatus");

            return File(GeneratePDF(model, $"請求核定(處理情形)-{((ApplyTypeEnum)applyTypeId).GetDescription()}", ignoreFields));
        }
        public ActionResult ProcessReportDownExcel(int diasterId = 0, int? cityId = null, int applyTypeId = 0)
        {
            var model = ApplySupportService.ProcessReport(new ApplySupportReportFilterModel
            {
                DiasterId = diasterId,
                CityId = cityId,
                ApplyTypeId = applyTypeId
            });

            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("Id");
            ignoreFields.Add("ApplyType");
            ignoreFields.Add("UserId");
            ignoreFields.Add("UserName");
            ignoreFields.Add("EPAConfirmStatus");

            return File(GenerateExcel(model, $"請求核定(處理情形)-{((ApplyTypeEnum)applyTypeId).GetDescription()}", ignoreFields));
        }
        public ActionResult ProcessReportDownODS(int diasterId = 0, int? cityId = null, int applyTypeId = 0)
        {
            var model = ApplySupportService.ProcessReport(new ApplySupportReportFilterModel
            {
                DiasterId = diasterId,
                CityId = cityId,
                ApplyTypeId = applyTypeId
            });

            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("Id");
            ignoreFields.Add("ApplyType");
            ignoreFields.Add("UserId");
            ignoreFields.Add("UserName");
            ignoreFields.Add("EPAConfirmStatus");

            return File(GenerateODS(model, $"請求核定(處理情形)-{((ApplyTypeEnum)applyTypeId).GetDescription()}", ignoreFields));
        }

        private Dictionary<string, int> GetTypes()
        {
            var dictionary = new Dictionary<string, int>();
            foreach (ApplyTypeEnum enumObj in Enum.GetValues(typeof(ApplyTypeEnum)))
            {
                dictionary.Add(enumObj.GetDescription(), enumObj.ToInteger());
            }

            return dictionary;
        }

        private Dictionary<string, int> GetApplyStatus()
        {
            var dictionary = new Dictionary<string, int>();

            foreach (ApplyStatusEnum enumObj in Enum.GetValues(typeof(ApplyStatusEnum)))
            {
                if (enumObj == ApplyStatusEnum.SendToEpa) 
                {
                    continue;
                }
                dictionary.Add(enumObj.GetDescription(), enumObj.ToInteger());
            }

           

            return dictionary;
        }

        public List<TownModel> GetTownsByCityId(int cityId) 
        {
            return TownService.GetByCityId(cityId);
        }
    }
}
