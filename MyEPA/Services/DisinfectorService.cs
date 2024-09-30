using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MyEPA.Services
{
    public class DisinfectorService
    {
        DisinfectorRepository DisinfectorRepository = new DisinfectorRepository();
        CityRepository CityRepository = new CityRepository();
        TownRepository TownRepository = new TownRepository();
        public DisinfectorModel Get(int id)
        {
            return DisinfectorRepository.Get(id);
        }
        public List<DisinfectorModel> GetByFilter(DisinfectorFilterParameter filter)
        {
            return DisinfectorRepository.GetByFilter(filter);
        }
        public List<DisinfectorReportModel> GetReportByFilter(DisinfectorFilterParameter filter)
        {
            return DisinfectorRepository.GetReportByFilter(filter);
        }

        public List<DisinfectorCityReportModel> GetCityReport(DisinfectorFilterCityReportParameter filter)
        {
            return DisinfectorRepository.GetCityReport(filter);
        }

        public List<DisinfectorCityReportModel> GetTownReport(DisinfectorFilterCityReportParameter filter)
        {
            return DisinfectorRepository.GetTownReport(filter);
        }
        public List<DisinfectorSummaryCityReportModel> GetSummaryCityReport()
        {
            var query = DisinfectorRepository.GetSummaryCityReport().ToDictionary(e => e.City, e => e);
            var citys = CityRepository.GetListByFilter(new CityFilterParameter
            {
                IsCounty = true
            });

            foreach (var item in citys)
            {
                if (query.ContainsKey(item.City) == false)
                {
                    query.Add(item.City, new DisinfectorSummaryCityReportModel
                    {
                        Sort = item.Sort,
                        City = item.City,
                        CityId = item.Id
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
        public List<DisinfectorSummaryCityReportModel> GetSupportCityReport()
        {
            var query = DisinfectorRepository.GetSupportCityReport().ToDictionary(e => e.City, e => e);
            var citys = CityRepository.GetListByFilter(new CityFilterParameter
            {
                IsCounty = true
            });

            foreach (var item in citys)
            {
                if (query.ContainsKey(item.City) == false)
                {
                    query.Add(item.City, new DisinfectorSummaryCityReportModel
                    {
                        Sort = item.Sort,
                        City = item.City,
                        CityId = item.Id
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

        public List<DisinfectorSummaryTownReportModel> GetSummaryTownReport(int cityId)
        {
            var query = DisinfectorRepository.GetSummaryTownReport();

            var citys = CityRepository.GetListByFilter(new CityFilterParameter
            {
                IsCounty = true,
                CityIds = cityId.ToListCollection()
            });

            if (citys.IsEmptyOrNull())
            {
                return new List<DisinfectorSummaryTownReportModel>();
            }

            var resultDic = query.Where(e => citys.Any(f => f.City == e.City)).ToDictionary(e => new { e.City, e.Town }, e => e);

            var citydic = citys.ToDictionary(e => e.Id, e => e.City);

            var towns = TownRepository.GetListByFilter(new TownFilterParameter
            {
                CityIds = citydic.Select(e => e.Key).ToList(),
            }).ToList();

            foreach (var item in towns)
            {
                if (citydic.ContainsKey(item.CityId) == false)
                {
                    continue;
                }
                var city = citydic[item.CityId];
                if (resultDic.ContainsKey(new { City = city, Town = item.Name }) == false)
                {
                    resultDic.Add(new { City = city, Town = item.Name }, new DisinfectorSummaryTownReportModel
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
        public List<int> GetROCYears()
        {
            return DisinfectorRepository.GetROCYears();
        }
    }
}