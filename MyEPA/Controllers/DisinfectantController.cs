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
    public class DisinfectantController : LoginBaseController
    {
        DisinfectantService DisinfectantService = new DisinfectantService();

        CityService CityService = new CityService();
        TownService TownService = new TownService();
        public ActionResult Search(DisinfectantFilterParameter filter)
        {
            var result = DisinfectantService.GetByFilter(filter);

            return View(result);
        }
        public ActionResult SummaryCityReport()
        {
            var result = DisinfectantService.GetSummaryCityReport();
            return View(result);
        }
        [Route("Disinfectant/DownSummaryCityReport/{file}")]
        public ActionResult DownSummaryCityReport(string file)
        {
            var model = DisinfectantService.GetSummaryCityReport();
            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("TownId");
            ignoreFields.Add("CityId");
            ignoreFields.Add("Sort");
            
            if (file == "PDF")
            {
                return File(GeneratePDF(model, "消毒藥劑報表", ignoreFields));
            }
            return File(GenerateODS(model, "消毒藥劑報表", ignoreFields));
        }
      
        public ActionResult SummaryTownReport(int cityId)
        {
            var result = DisinfectantService.GetSummaryTownReport(cityId);
            ViewBag.CityId = cityId;
            return View(result);
        }
        [Route("Disinfectant/DownSummaryTownReport/{file}")]
        public ActionResult DownSummaryTownReport(string file,int cityId)
        {
            var model = DisinfectantService.GetSummaryTownReport(cityId);
            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("TownId");
            ignoreFields.Add("CityId");

            if (file == "PDF")
            {
                return File(GeneratePDF(model, "消毒藥劑報表-鄉鎮", ignoreFields));
            }
            return File(GenerateODS(model, "消毒藥劑報表-鄉鎮", ignoreFields));
        }
        public ActionResult TownReport(int? cityId , int? townId, DisinfectantUseTypeEnum? useType, string drugName,string drugType, string ActiveIngredients1, string ActiveIngredients2, string DrugState)
        {
            string city = string.Empty;
            string town = string.Empty;

            if (cityId.HasValue)
            {
                city = CityService.Get(cityId.Value)?.City;
            }

            if (townId.HasValue)
            {
                town = TownService.Get(townId.Value)?.Name;
            }

            var result = DisinfectantService.GetTownReport(new DisinfectantReportFilterParameter
            {
                City = city,
                DrugName = drugName,
                DrugType = drugType,
                Town = town,
                UseType = useType,
                ActiveIngredients1= ActiveIngredients1,
                ActiveIngredients2= ActiveIngredients2,
                DrugState= DrugState
            });
            ViewBag.UseType = useType;
            ViewBag.DrugName = drugName;
            ViewBag.DrugType = drugType;
            ViewBag.CityId = cityId;
            ViewBag.TownId = townId;
            ViewBag.Citys = CityService.GetCountyOrderBySort();
            ViewBag.UseTypes = ExtensionsOfEnum.GetEnumAllValue<DisinfectantUseTypeEnum>();
            ViewBag.DrugNames = ExtensionsOfEnum.GetEnumAllValue<DisinfectantNameEnum>();
            ViewBag.DrugTypes = ExtensionsOfEnum.GetEnumAllValue<DisinfectantDrugTypeEnum>();
            return View(result);
        }
        [Route("Disinfectant/DownTownReport/{file}")]
        public ActionResult DownTownReport(string file,int? cityId, int? townId, DisinfectantUseTypeEnum? useType, string drugName, string drugType)
        {
            string city = string.Empty;
            string town = string.Empty;

            if (cityId.HasValue)
            {
                city = CityService.Get(cityId.Value)?.City;
            }

            if (townId.HasValue)
            {
                town = TownService.Get(townId.Value)?.Name;
            }

            var result = DisinfectantService.GetTownReport(new DisinfectantReportFilterParameter
            {
                City = city,
                DrugName = drugName,
                DrugType = drugType,
                Town = town,
                UseType = useType
            });
            List<string> ignoreFields = new List<string>();

            ignoreFields.Add("TownId");
            ignoreFields.Add("CityId");
            ignoreFields.Add("Town");
            if (file == "PDF")
            {
                return File(GeneratePDF(result, "環境消毒藥劑統計", ignoreFields));
            }
            return File(GenerateODS(result, "環境消毒藥劑統計", ignoreFields));
        }
        public ActionResult CityStatistics(ServiceLifeTypeEnum type = ServiceLifeTypeEnum.ThreeMonths)
        {
            var result = DisinfectantService.GetCityStatistics(type);
            ViewBag.ServiceLifeType = type;
            return View(result);
        }

        public ActionResult TownStatistics(ServiceLifeTypeEnum type, int cityId)
        {
            var result = DisinfectantService.GetTownStatistics(type, cityId);
            ViewBag.ServiceLifeType = type;
            return View(result);
        }
    }
}