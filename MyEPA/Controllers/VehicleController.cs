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
    public class VehicleController : LoginBaseController
    {
        VehicleService VehicleService = new VehicleService();

        CityService CityService = new CityService();
        public ActionResult Search(VehicleFilterParameter filter)
        {
            var result = VehicleService.GetByFilter(filter);
            result = result.OrderBy(a => a.City).ThenBy(b => b.Town).ToList();

            ViewBag.Summary = VehicleService.GetReportByFilter(filter);

            return View(result);
        }

        public ActionResult CarsCountReport()
        {
            var cars = VehicleService.GetCarTypes();

            var result = VehicleService.GetCarsCountByCity();

            ViewBag.Cars = cars;

            ViewBag.Citys = CityService.GetCountyOrderBySort();

            //跨縣市調度
            ViewBag.SupportCitys = VehicleService.GetCarsSupportCityCountByCity();

            return View(result);
        }

        [Route("Vehicle/DownCarsCountReport/{file}")]
        public ActionResult DownCarsCountReport(string file)
        {
            var cars = VehicleService.GetCarTypes();

            var result = VehicleService.GetCarsCountByCity();

            List<string> ignoreFields = new List<string>();


            if (file == "PDF")
            {
                return File(GeneratePDF(result, "環境清潔車輛統計報表", ignoreFields));
            }
            return File(GenerateODS(result, "環境清潔車輛統計報表", ignoreFields));
        }
    }
}