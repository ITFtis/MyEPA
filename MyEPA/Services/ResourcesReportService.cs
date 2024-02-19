using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Services
{
    public class ResourcesReportService
    {
        ResourcesReportRepository ResourcesReportRepository = new ResourcesReportRepository();

        public List<ResourcesConfirmUpdateTimeDataModel> GetResourcesConfirmUpdateTimeDatas(ResourcesConfirmUpdateTimeDataFilterParameter filter)
        {
            return ResourcesReportRepository.GetResourcesConfirmUpdateTimeDatas(filter);
        }
        public List<ResourcesReportCityModel> GetResourcesCityReport(string orderBy = null, bool isDesc = false)
        {
            var result = ResourcesReportRepository.GetResourcesCityReport();
            if(string.IsNullOrWhiteSpace(orderBy) == false)
            {
                if(isDesc)
                {
                    result = result.OrderByDescending(orderBy).ToList();
                }
                else
                {
                    result = result.OrderBy(orderBy).ToList();
                }
            }
            return result;
        }

        public List<ResourcesReportTownModel> GetResourcesTownReport(int cityId, string orderBy = null, bool isDesc = false)
        {
            var result = ResourcesReportRepository.GetResourcesTownReport(cityId);
            if (string.IsNullOrWhiteSpace(orderBy) == false)
            {
                if (isDesc)
                {
                    result = result.OrderByDescending(orderBy).ToList();
                }
                else
                {
                    result = result.OrderBy(orderBy).ToList();
                }
            }
            return result;
        }
    }
}