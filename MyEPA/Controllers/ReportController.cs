using MyEPA.Helper;
using MyEPA.Services;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class ReportController : LoginBaseController
    {
        YearReportService YearReportService = new YearReportService();
        ApplyReportService ApplyReportService = new ApplyReportService();
        /// <summary>
        /// 年度統計報表
        /// </summary>
        /// <param name="year">民國年</param>
        /// <returns></returns>
        public ActionResult YearReport(int? year = null)
        {
            if (year.HasValue == false)
            {
                //取得現在年度轉民國
                year = DateTimeHelper.GetCurrentTime().Year - 1911;
            }
            ViewBag.Year = year;
            var result = YearReportService.GetYearReport(year.Value);
            return View(result);
        }
        public ActionResult GetYearReportYears(int year)
        {
            IEnumerable<SelectListItem> result = YearReportService.GetYearReportYears()
                .Select(e => 
                {
                    var value = e - 1911;
                    bool selected = value == year;
                    return new SelectListItem 
                    { 
                        Selected = selected,
                        Text = value.ToString(), 
                        Value = value.ToString() 
                    }; 
                });
            return PartialView(result);
        }

        [Route("Report/DownYearReport/{file}")]
        public ActionResult DownYearReport(string file, int year)
        {
            var result = YearReportService.GetYearReport(year);
            var fileName = $"年度報表-{year}年度";
            if (file == "PDF")
            {
                return File(GeneratePDF(result, fileName));
            }
            return File(GenerateODS(result, fileName));
        }

        public ActionResult ApplyReport(int? diasterId = null)
        {
            if (diasterId.HasValue == false)
            {
                return View(new List<ApplyStatusReportViewModel> ());
            }
            var result = ApplyReportService.GetApplyReport(diasterId.Value);
            return View(result);
        }
    }
}