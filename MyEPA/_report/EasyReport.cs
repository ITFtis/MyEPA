using MyEPA.Enums;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
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
    }
}