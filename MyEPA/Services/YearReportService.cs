using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MyEPA.Services
{
    public class ApplyReportService
    {
        CityRepository CityRepository = new CityRepository();
        ApplyReportRepository ApplyReportRepository = new ApplyReportRepository();
        public List<ApplyStatusReportViewModel> GetApplyReport(int diasterId)
        {
            var citys = CityRepository.GetListByFilter(new CityFilterParameter
            {
                IsCounty = true
            }).OrderBy(e => e.Sort);

            var statusReport = 
                ApplyReportRepository
                .GetApplyStatusReport(diasterId)
                .GroupBy(e=>e.CityId)
                .ToDictionary(e=>e.Key,e=> e.ToList());

            return citys.Select(city => 
            {
                var cityData = statusReport.GetValue(city.Id, new List<ApplyStatusReportModel> { });
                return new ApplyStatusReportViewModel
                {
                    CityId = city.Id,
                    CityName = city.City,
                    EPAConfrimCount = cityData.Where(e=>e.EPAConfirmStatus == ApplyStatusEnum.Confrim).Sum(e => e.EPAConfirmStatusCount),
                    EPAPendingCount = cityData.Where(e => e.EPAConfirmStatus == ApplyStatusEnum.Pending).Sum(e => e.EPAConfirmStatusCount),
                    EPAProcessingCount = cityData.Where(e => e.EPAConfirmStatus == ApplyStatusEnum.Processing).Sum(e => e.EPAConfirmStatusCount),
                    EPARejectCount = cityData.Where(e => e.EPAConfirmStatus == ApplyStatusEnum.Reject).Sum(e => e.EPAConfirmStatusCount),
                    EPBConfrimCount = cityData.Where(e => e.EPBConfirmStatus == ApplyStatusEnum.Confrim).Sum(e => e.EPBConfirmStatusCount),
                    EPBPendingCount = cityData.Where(e => e.EPBConfirmStatus == ApplyStatusEnum.Pending).Sum(e => e.EPBConfirmStatusCount),
                    EPBProcessingCount = cityData.Where(e => e.EPBConfirmStatus == ApplyStatusEnum.Processing).Sum(e => e.EPBConfirmStatusCount),
                    EPBRejectCount = cityData.Where(e => e.EPBConfirmStatus == ApplyStatusEnum.Reject).Sum(e => e.EPBConfirmStatusCount)
                };
            }).ToList();
        }
    }
    public class YearReportService
    {
        DamageRepository DamageRepository = new DamageRepository { };
        WaterCheckDetailRepository WaterCheckDetailRepository = new WaterCheckDetailRepository();
        DiasterRepository DiasterRepository = new DiasterRepository();

        public List<int> GetYearReportYears()
        {
            return DiasterRepository.GetDiasterYears();
        }
        /// <summary>
        /// 年度統計報表
        /// </summary>
        /// <param name="year">民國年</param>
        /// <returns></returns>
        public List<YearReportViewModel> GetYearReport(int year)
        {
            //轉換成西元
            int adYear = year + 1911;

            List<DiasterModel> diasters = DiasterRepository.GetByFilter(new DiasterFilterParameter
            {
                StartYears = adYear.ToListCollection()
            });

            Dictionary<int, DamageYearReportModel> damageYearReport =
                DamageRepository
                .GetYearReport(adYear)
                .ToDictionary(e => e.DiasterId, e => e);

            Dictionary<int,WaterCheckYearReportModel> epaWaterCheckYearReport =
                WaterCheckDetailRepository
                .GetYearReport(WaterCheckTypeEnum.EPPersonnel, adYear)
                .ToDictionary(e => e.DiasterId, e => e);

            Dictionary<int, WaterCheckYearReportModel> waterWaterCheckYearReport = 
                WaterCheckDetailRepository
                .GetYearReport(WaterCheckTypeEnum.Water, adYear)
                .ToDictionary(e => e.DiasterId, e => e);

            return diasters.Select(diaster => 
            {
                DamageYearReportModel damageYear =
                    damageYearReport.GetValue(diaster.Id);

                WaterCheckYearReportModel epaWaterCheckYear = 
                    epaWaterCheckYearReport.GetValue(diaster.Id);

                WaterCheckYearReportModel waterWaterCheckYear =
                    waterWaterCheckYearReport.GetValue(diaster.Id);

                return new YearReportViewModel 
                { 
                    DiasterName = diaster.DiasterName,
                    FloodArea = (damageYear?.FloodArea).GetValueOrDefault(),
                    NationalArmyQuantity = (damageYear?.NationalArmyQuantity).GetValueOrDefault(),
                    CleaningMemberQuantity = (damageYear?.CleaningMemberQuantity).GetValueOrDefault(),
                    CLE_Disinfect = (damageYear?.CLE_Disinfect).GetValueOrDefault(),
                    CLE_Garbage = (damageYear?.CLE_Garbage).GetValueOrDefault(),
                    CLE_MUD = (damageYear?.CLE_MUD).GetValueOrDefault(),
                    EPCount = (epaWaterCheckYear?.Count).GetValueOrDefault(),
                    EPFailureCount = (epaWaterCheckYear?.FailureCount).GetValueOrDefault(),
                    WaterCount = (waterWaterCheckYear?.Count).GetValueOrDefault(),
                    WaterFailureCount = (waterWaterCheckYear?.FailureCount).GetValueOrDefault(),
                }; 
            }).ToList();
        }
    }
}
