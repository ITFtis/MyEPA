using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.JsnoModel;
using MyEPA.Repositories;
using System.Linq;

namespace MyEPA.Services
{
    public class SystemConfigSettingService
    {
        SystemConfigSettingRepository SystemConfigSettingRepository = new SystemConfigSettingRepository();
        ResourcesReportRepository ResourcesReportRepository = new ResourcesReportRepository();
        DefendRepository DefendRepository = new DefendRepository();
        public SystemConfigSettingModel GetTaiwanMapSetting()
        {
            return SystemConfigSettingRepository.Get(Enums.SystemConfigSettingFunctionEnum.TaiwanMapSetting);
        }
        
        public TaiwanMapSettingModel GetTaiwanMapReport(int diasterId)
        {
            TaiwanMapSettingModel result = GetTaiwanMapSetting().Value.JsonConvertToModel<TaiwanMapSettingModel>();

            MultiKeyDictionary<string, string, ResourcesReportTownModel> townResourcesReport =
                ResourcesReportRepository.GetResourcesTownReport()
                .GroupBy(e=> new { e.City,e.Town})
                .ToMultiKeyDictionary(e => e.Key.City, e => e.Key.Town, e => e.FirstOrDefault());

            var defendDic = DefendRepository.GetCityCountByDiasterId(diasterId).ToMultiKeyDictionary(e => e.City, e => e.Town, e => e.Count);

            var defendCityDic = DefendRepository.GetCityCountByDiasterId(diasterId).GroupBy(e => e.City).ToDictionary(e => e.Key, e => e.Sum(f => f.Count));

            foreach (var properties in result.objects.tw_mercator.geometries.Select(e => e.properties))
            {
                if(townResourcesReport.ContainsKey(properties.COUNTY, properties.TOWN) == false)
                {
                    continue;
                }
                ResourcesReportTownModel town = townResourcesReport[properties.COUNTY, properties.TOWN];
                properties.DisinfectorCount = town.DisinfectorCount;
                properties.DumpCount = town.DumpCount;
                properties.DisinfectantLiquidAmount = town.DisinfectantLiquidAmount;
                properties.DisinfectantSolidAmount = (int)town.DisinfectantSolidAmount;
                properties.GarbageLandfillCount = 0;
                properties.PestCount = town.PestCount;
                properties.ToiletCount = town.ToiletCount;
                properties.UserCount = town.UserCount;
                properties.VehicleCount = town.VehicleCount;
                properties.VolunteerCount = town.VolunteerCount;
                //特殊規則，如果環保局 OK 就改縣市加總
                properties.DefendCount = defendDic.ContainsKey(properties.COUNTY, "環保局") ? defendCityDic.GetValue(properties.COUNTY) : defendDic.GetValue(properties.COUNTY, properties.TOWN);
            }

            return result;
        }
    }
}