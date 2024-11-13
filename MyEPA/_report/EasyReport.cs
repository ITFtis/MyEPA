using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA
{
    public class EasyReport
    {
        /// <summary>
        /// 車輛(環境清理機具)全部
        /// </summary>
        /// <returns></returns>
        public static List<CarsModel> GetCars()
        {
            VehicleTypeRepository VehicleTypeRepository = new VehicleTypeRepository();
            VehicleService VehicleService = new VehicleService();

            var carTypes = VehicleTypeRepository.GetList();
            var carDatas = VehicleService.GetCarsCountByCity();
            var Totals = carTypes.GroupJoin(carDatas, a => a.Name, b => b.VehicleName, (o, c) => new CarsModel
            {
                Type = o.Type.Trim(),
                TypeName = o.Name.Trim(),
                Count = c.Sum(a => a.Count),
            }).ToList();

            return Totals;
        }

        /// <summary>
        /// 車輛(類型)全部
        /// </summary>
        /// <returns></returns>
        public static List<VehicleTypeModel> GetCarsType()
        {
            VehicleTypeRepository VehicleTypeRepository = new VehicleTypeRepository();
            var Totals = VehicleTypeRepository.GetList();

            return Totals;
        }

        /// <summary>
        /// 消毒設備全部
        /// </summary>
        /// <returns></returns>
        public static List<DisinfectorSummaryCityReportModel> GetDisinfector()
        {
            DisinfectorService DisinfectorService = new DisinfectorService();
            var Totals = DisinfectorService.GetSummaryCityReport();

            return Totals;
        }

        /// <summary>
        /// 消毒藥劑全部
        /// </summary>
        /// <returns></returns>
        public static List<DisinfectantCityReportModel> GetDisinfectant()
        {
            DisinfectantService DisinfectantService = new DisinfectantService();
            var Totals = DisinfectantService.GetTownReport(new DisinfectantReportFilterParameter
            {
            });

            return Totals;
        }

        /// <summary>
        /// 消毒藥劑(Enum.環境消毒)
        /// </summary>
        /// <returns></returns>
        public static List<DisinfectantCityReportModel> GetDisinfectantEnumEnvironment()
        {
            DisinfectantService DisinfectantService = new DisinfectantService();
            var Environments = DisinfectantService.GetTownReport(new DisinfectantReportFilterParameter
            {
                UseType = DisinfectantUseTypeEnum.Environment,
            });

            return Environments;
        }

        /// <summary>
        /// 消毒藥劑(Enum.登革熱)
        /// </summary>
        /// <returns></returns>
        public static List<DisinfectantCityReportModel> GetDisinfectantEnumDengue()
        {
            DisinfectantService DisinfectantService = new DisinfectantService();
            var Dengues = DisinfectantService.GetTownReport(new DisinfectantReportFilterParameter
            {
                UseType = DisinfectantUseTypeEnum.Dengue,
            });

            return Dengues;
        }

        public class CarsModel
        {
            public string Type { get; set; }
            public string TypeName { get; set; }
            public int Count { get; set; }
        }
    }
}