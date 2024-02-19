using iTextSharp.text;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Models.SearchViewModel;
using MyEPA.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class WaterCheckController : LoginBaseController
    {
        WaterCheckService WaterCheckService = new WaterCheckService();
        WaterCheckDetailService WaterCheckDetailService = new WaterCheckDetailService();
        CityService CityService = new CityService();
        DiasterService DiasterService = new DiasterService();
        public ActionResult Index(int? diasterId = null)
        {
            List<DiasterModel>  diasters = DiasterService.GetAll();

            if (diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }

            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = diasters;
            
            if(diasterId.HasValue == false)
            {
                return View(new List<WaterCheckModel>());
            }

            List<WaterCheckViewModel> result = 
                WaterCheckService.GetByDiasterId(diasterId.Value,GetUserBrief());
         

            return View(result);
        }
        [HttpPost]
        public ActionResult UpdateStatus(WaterCheckModel model)
        {
            var result = WaterCheckService.UpdateStatus(GetUserBrief(), model);
            return JsonResult(result);
        }

        public ActionResult Memo(WaterCheckModel model)
        {
            var result = WaterCheckService.Get(model.Id);

            if(result == null)
            {
                return View(model);
            }

            return View(result);
        }

        public ActionResult UpdateMemo(WaterCheckModel model)
        {
            WaterCheckService.UpdateMemo(GetUserBrief(), model);
            return RedirectToAction("Index", new { diasterId = model.DiasterId });
        }
        [HttpGet]
        public ActionResult DownPrintView(int id)
        {
            var user = GetUserBrief();
            var result = WaterCheckService.Get(id);

            string diasterName = DiasterService.GetByFilter(new DiasterFilterParameter
            {
                Ids = result.DiasterId.ToListCollection()
            })
                .Select(e => e.DiasterName).FirstOrDefault();
            ViewBag.Details = WaterCheckDetailService.GetListByWaterCheckId(GetUserBrief(), id);
            ViewBag.DiasterName = diasterName;
            result.TownName = user.Town;
            result.CityName = user.City;
            var file = GeneratePDFByHtml("PrintView", result, $"{user.City}{ user.Town} 水質抽檢結果通報表{result.CheckDate:yyyyMMdd}.pdf", PageSize.A4.Rotate());
            return File(file);
        }
        
        [HttpGet]
        public ActionResult PrintView(int id)
        {
            var result = WaterCheckService.Get(id);
            ViewBag.Details = WaterCheckDetailService.GetListByWaterCheckId(GetUserBrief(), id);
            ViewBag.DiasterName =
                DiasterService.GetByFilter(new DiasterFilterParameter
                {
                    Ids = id.ToListCollection()
                })
                .Select(e => e.DiasterName).FirstOrDefault();
            return View(result);
        }
        
        public ActionResult Test()
        {
            ViewBag.Citys = CityService.GetCountyOrderBySort();
            return View();
        }
        /// <summary>
        /// 環保局水質通報
        /// </summary>
        /// <param name="diasterId"></param>
        /// <returns></returns>
        public ActionResult Report(int? diasterId = null)
        {
            List<DiasterModel> diasters = DiasterService.GetAll();

            if (diasterId.HasValue == false)
            {
                diasterId = diasters.Select(e => e.Id).FirstOrDefault();
            }
            ViewBag.DiasterId = diasterId;
            ViewBag.Diasters = diasters;

            List<WaterCheckReportModel> result = 
                WaterCheckService.GetReport(diasterId.Value);

            return View(result);

        }
        public ActionResult DownReportPDF(int? diasterId)
        {
            List<WaterCheckReportModel> model =
                WaterCheckService.GetReport(diasterId.Value);

            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("TownId");
            ignoreFields.Add("CityId");
            ignoreFields.Add("TownName");

            return File(GeneratePDF(model, "水質通報", ignoreFields));
        }
        public ActionResult DownReportODS(int? diasterId)
        {
            List<WaterCheckReportModel> model =
                WaterCheckService.GetReport(diasterId.Value);

            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("TownId");
            ignoreFields.Add("CityId");
            ignoreFields.Add("TownName");

            return File(GenerateODS(model, "水質通報", ignoreFields));
        }

        public ActionResult Statistics(int? diasterId = null)
        {
            var diaster = DiasterService.GetByFilter(new DiasterFilterParameter());
            if (diasterId.HasValue == false)
            {
                diasterId = diaster.OrderByDescending(e => e.Id).Select(e => e.Id).FirstOrDefault();
            }
            ViewBag.Diasters = DiasterService.GetAll();
            ViewBag.DiasterId = diasterId;
            var model = WaterCheckService.Statistics(diasterId.Value);

            return View(model);
        }

        public ActionResult DetailStatistics(DetailStatisticsSearchViewModel search)
        {
            var model = WaterCheckService.GetDetailStatistics(search);

            return View(model);
        }
    }
}