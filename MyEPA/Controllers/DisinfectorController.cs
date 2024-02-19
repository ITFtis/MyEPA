using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models.FilterParameter;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers
{
    public class DisinfectorController : LoginBaseController
    {
        DisinfectorService DisinfectorService = new DisinfectorService();

        CityService CityService = new CityService();

        public ActionResult Search(DisinfectorFilterParameter filter)
        {
            var result = DisinfectorService.GetByFilter(filter);

            ViewBag.Summary = DisinfectorService.GetReportByFilter(filter);

            return View(result);
        }
        public ActionResult SummaryCityReport()
        {
            var result = DisinfectorService.GetSummaryCityReport();
            return View(result);
        }
        [Route("Disinfector/DownSummaryCityReport/{file}")]
        public ActionResult DownSummaryCityReport(string file)
        {
            var model = DisinfectorService.GetSummaryCityReport();
            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("TownId");
            ignoreFields.Add("CityId");
            ignoreFields.Add("Sort");
            if (file == "PDF")
            {
                return File(GeneratePDF(model, "消毒設備報表", ignoreFields));
            }
            return File(GenerateODS(model, "消毒設備報表", ignoreFields));
        }

        public ActionResult SummaryTownReport(int cityId)
        {
            var result = DisinfectorService.GetSummaryTownReport(cityId);
            ViewBag.CityId = cityId;
            return View(result);
        }
        [Route("Disinfector/DownSummaryTownReport/{file}")]
        public ActionResult DownSummaryTownReport(string file, int cityId)
        {
            var model = DisinfectorService.GetSummaryTownReport(cityId);
            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("TownId");
            ignoreFields.Add("CityId");

            if (file == "PDF")
            {
                return File(GeneratePDF(model, "消毒設備報表-鄉鎮", ignoreFields));
            }
            return File(GenerateODS(model, "消毒設備報表-鄉鎮", ignoreFields));
        }
        public ActionResult CityReport(int? cityId = null,int? townId = null,string name = null, int? useType = null,int? year = null)
        {
            var result = DisinfectorService.GetCityReport(new DisinfectorFilterCityReportParameter 
            {
                CityIds = cityId.HasValue ? cityId.Value.ToListCollection() : new List<int>(),
                TownIds = townId.HasValue ? townId.Value.ToListCollection() : new List<int>(),
                Name = name,
                UseTypes = useType.HasValue ? useType.Value.ToListCollection() : new List<int>(),
                Year = year
            });
            ViewBag.CityId = cityId;
            ViewBag.TownId = townId;
            ViewBag.Name = name;
            ViewBag.UseType = useType;
            ViewBag.Year = year;
            ViewBag.Citys = CityService.GetCountyOrderBySort();
            ViewBag.Names = ExtensionsOfEnum.GetEnumAllValue<DisinfectorNameEnum>();
            ViewBag.UseTypes = ExtensionsOfEnum.GetEnumAllValue<DisinfectorUseTypeEnum>();
            ViewBag.ROCYears = DisinfectorService.GetROCYears().OrderByDescending(e => e).ToList();
            return View(result);
        }
        [Route("Disinfector/DownCityReport/{file}")]
        public ActionResult DownCityReport(string file, int? cityId = null, int? townId = null, string name = null, int? useType = null, int? year = null)
        {
            var result = DisinfectorService.GetCityReport(new DisinfectorFilterCityReportParameter
            {
                CityIds = cityId.HasValue ? cityId.Value.ToListCollection() : new List<int>(),
                TownIds = townId.HasValue ? townId.Value.ToListCollection() : new List<int>(),
                Name = name,
                UseTypes = useType.HasValue ? useType.Value.ToListCollection() : new List<int>(),
                Year = year
            });
            ViewBag.CityId = cityId;
            ViewBag.TownId = townId;
            ViewBag.Name = name;
            ViewBag.UseType = useType;
            ViewBag.Year = year;
            ViewBag.Citys = CityService.GetCountyOrderBySort();
            ViewBag.Names = ExtensionsOfEnum.GetEnumAllValue<DisinfectorNameEnum>();
            ViewBag.UseTypes = ExtensionsOfEnum.GetEnumAllValue<DisinfectorUseTypeEnum>();
            ViewBag.ROCYears = DisinfectorService.GetROCYears().OrderByDescending(e => e).ToList();


            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("CityId");
            if (file == "PDF")
            {
                return File(GeneratePDF(result, "環境消毒設備統計", ignoreFields));
            }
            return File(GenerateODS(result, "環境消毒設備統計", ignoreFields));
        }

        public ActionResult TownReport(int? cityId = null, int? townId = null, string name = null, int? useType = null, int? year = null)
        {
            var result = DisinfectorService.GetTownReport(new DisinfectorFilterCityReportParameter
            {
                CityIds = cityId.HasValue ? cityId.Value.ToListCollection() : new List<int>(),
                TownIds = townId.HasValue ? townId.Value.ToListCollection() : new List<int>(),
                Name = name,
                UseTypes = useType.HasValue ? useType.Value.ToListCollection() : new List<int>(),
                Year = year
            });
            ViewBag.CityId = cityId;
            ViewBag.TownId = townId;
            ViewBag.Name = name;
            ViewBag.UseType = useType;
            ViewBag.Year = year;
            ViewBag.Citys = CityService.GetCountyOrderBySort();
            ViewBag.Names = ExtensionsOfEnum.GetEnumAllValue<DisinfectorNameEnum>();
            ViewBag.UseTypes = ExtensionsOfEnum.GetEnumAllValue<DisinfectorUseTypeEnum>();
            ViewBag.ROCYears = DisinfectorService.GetROCYears().OrderByDescending(e => e).ToList();
            return View(result);
        }
        [Route("Disinfector/DownTownReport/{file}")]
        public ActionResult DownTownReport(string file, int? cityId = null, int? townId = null, string name = null, int? useType = null, int? year = null)
        {
            var result = DisinfectorService.GetTownReport(new DisinfectorFilterCityReportParameter
            {
                CityIds = cityId.HasValue ? cityId.Value.ToListCollection() : new List<int>(),
                TownIds = townId.HasValue ? townId.Value.ToListCollection() : new List<int>(),
                Name = name,
                UseTypes = useType.HasValue ? useType.Value.ToListCollection() : new List<int>(),
                Year = year
            });
            ViewBag.CityId = cityId;
            ViewBag.TownId = townId;
            ViewBag.Name = name;
            ViewBag.UseType = useType;
            ViewBag.Year = year;
            ViewBag.Citys = CityService.GetCountyOrderBySort();
            ViewBag.Names = ExtensionsOfEnum.GetEnumAllValue<DisinfectorNameEnum>();
            ViewBag.UseTypes = ExtensionsOfEnum.GetEnumAllValue<DisinfectorUseTypeEnum>();
            ViewBag.ROCYears = DisinfectorService.GetROCYears().OrderByDescending(e => e).ToList();


            List<string> ignoreFields = new List<string>();
            ignoreFields.Add("TownId");
            ignoreFields.Add("CityId");
            if (file == "PDF")
            {
                return File(GeneratePDF(result, "環境消毒設備統計", ignoreFields));
            }
            return File(GenerateODS(result, "環境消毒設備統計", ignoreFields));
        }
    }
}