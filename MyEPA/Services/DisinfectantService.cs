using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MyEPA.Services
{
    public class DisinfectantService
    {
        DisinfectantRepository DisinfectantRepository = new DisinfectantRepository();
        CityRepository CityRepository = new CityRepository();
        TownRepository TownRepository = new TownRepository();
        public List<DisinfectantModel> GetByFilter(DisinfectantFilterParameter filter)
        {
            return DisinfectantRepository.GetByFilter(filter);
        }

        public List<DisinfectantCityReportModel> GetCityReport(DisinfectantReportFilterParameter filter)
        {
            return DisinfectantRepository.GetCityReport(filter);
        }
        public List<DisinfectantCityReportModel> GetTownReport(DisinfectantReportFilterParameter filter)
        {
            return DisinfectantRepository.GetTownReport(filter);
        }
        private DateTime ToStartTime(ServiceLifeTypeEnum type,DateTime time)
        {
            DateTime result = time;
            switch(type)
            {
                case ServiceLifeTypeEnum.ThreeMonths:
                    result = time.AddMonths(-3);
                    break;
                case ServiceLifeTypeEnum.HalfYear:
                    result = time.AddMonths(-6);
                    break;
                case ServiceLifeTypeEnum.OneYear:
                    result = time.AddYears(-1);
                    break;
            }
            return result;
        }
        public List<DisinfectantSummaryCityReportModel> GetSummaryCityReport()
        {
            var query = DisinfectantRepository.GetSummaryCityReport().ToDictionary(e => e.City, e => e);
            var citys = CityRepository.GetListByFilter(new CityFilterParameter
            {
                IsCounty = true
            });
                        
            foreach (var item in citys)
            {
                if(query.ContainsKey(item.City) == false)
                {
                    query.Add(item.City,new DisinfectantSummaryCityReportModel
                    {
                        City = item.City,
                        CityId = item.Id,
                        Sort = item.Sort
                    });
                }
                else
                {
                    query[item.City].CityId = item.Id;
                    query[item.City].Sort = item.Sort;
                }
            }

            return query.Values.OrderBy(e => e.Sort).ToList();
        }

        //跨縣市調度
        public List<DisinfectantSummaryCityReportModel> GetSupportCityReport()
        {
            var query = DisinfectantRepository.GetSupportCityReport().ToDictionary(e => e.City, e => e);
            var citys = CityRepository.GetListByFilter(new CityFilterParameter
            {
                IsCounty = true
            });

            foreach (var item in citys)
            {
                if (query.ContainsKey(item.City) == false)
                {
                    query.Add(item.City, new DisinfectantSummaryCityReportModel
                    {
                        City = item.City,
                        CityId = item.Id,
                        Sort = item.Sort
                    });
                }
                else
                {
                    query[item.City].CityId = item.Id;
                    query[item.City].Sort = item.Sort;
                }
            }

            return query.Values.OrderBy(e => e.Sort).ToList();
        }

        public List<DisinfectantSummaryTownReportModel> GetSummaryTownReport(int cityId)
        {
            var query = DisinfectantRepository.GetSummaryTownReport();

            var citys = CityRepository.GetListByFilter(new CityFilterParameter
            {
                IsCounty = true,
                CityIds = cityId.ToListCollection()
            });

            if (citys.IsEmptyOrNull())
            {
                return new List<DisinfectantSummaryTownReportModel>();
            }

            var resultDic = query.Where(e => citys.Any(f => f.City == e.City)).ToDictionary(e => new { e.City, e.Town }, e => e);

            var citydic = citys.ToDictionary(e => e.Id, e => e.City);

            var towns = TownRepository.GetListByFilter(new TownFilterParameter
            {
                CityIds = citydic.Select(e => e.Key).ToList(),
            }).ToList();

            foreach (var item in towns)
            {
                if(citydic.ContainsKey(item.CityId) == false)
                {
                    continue;
                }
                var city = citydic[item.CityId];
                if (resultDic.ContainsKey(new { City = city, Town = item.Name}) == false)
                {
                    resultDic.Add(new {City= city,Town = item.Name },new DisinfectantSummaryTownReportModel
                    {
                        City = city,
                        CityId = item.CityId,
                        Town = item.Name,
                        TownId = item.Id
                    });
                }
                else
                {
                    resultDic[new { City = city, Town = item.Name }].CityId = item.CityId;
                    resultDic[new { City = city, Town = item.Name }].TownId = item.Id;
                }
            }
           
            return resultDic.Values.ToList();
        }

        public List<DisinfectantStatisticsCityViewModel> GetCityStatistics(ServiceLifeTypeEnum type)
        {
            DateTime endTime = DateTimeHelper.GetCurrentTime();
            DateTime startTime = ToStartTime(type, endTime);

            var statistic = DisinfectantRepository.GetStatistics(new DisinfectantStatisticsFilterParameter
            {
                ServiceLifeStartTime = startTime,
                ServiceLifeEndTime = endTime,
            }).GroupBy(e => new { e.City, e.CityId });

            return statistic.Select(e => new DisinfectantStatisticsCityViewModel
            {
                City = e.Key.City,
                CityId = e.Key.CityId,
                Count = e.Sum(f => f.Count),
                DisinfectantLiquidAmount = e.Where(f => f.DrugState == DrugStateEnum.Liquid.GetDescription()).Sum(f => f.Amount),
                DisinfectantSolidAmount = e.Where(f => f.DrugState == DrugStateEnum.Solid.GetDescription()).Sum(f => f.Amount),
                ServiceLife = e.Min(f => f.ServiceLife),
                UpdateTime = e.Max(f => f.UpdateTime)
            }).ToList();
        }

        public List<DisinfectantStatisticsTownViewModel> GetTownStatistics(ServiceLifeTypeEnum type,int cityId)
        {
            DateTime endTime = DateTimeHelper.GetCurrentTime();
            DateTime startTime = ToStartTime(type, endTime);

            var statistic = DisinfectantRepository.GetStatistics(new DisinfectantStatisticsFilterParameter
            {
                ServiceLifeStartTime = startTime,
                ServiceLifeEndTime = endTime,
                CityIds = cityId.ToListCollection()
            }).GroupBy(e => new { e.City, e.CityId, e.Town, e.TownId });

            return statistic.Select(e => new DisinfectantStatisticsTownViewModel
            {
                City = e.Key.City,
                Town = e.Key.Town,
                CityId = e.Key.CityId,
                TownId = e.Key.TownId,
                Count = e.Sum(f => f.Count),
                DisinfectantLiquidAmount = e.Where(f => f.DrugState == DrugStateEnum.Liquid.GetDescription()).Sum(f => f.Amount),
                DisinfectantSolidAmount = e.Where(f => f.DrugState == DrugStateEnum.Solid.GetDescription()).Sum(f => f.Amount),
                ServiceLife = e.Min(f => f.ServiceLife),
                UpdateTime = e.Max(f => f.UpdateTime)
            }).ToList();
        }
    }
}