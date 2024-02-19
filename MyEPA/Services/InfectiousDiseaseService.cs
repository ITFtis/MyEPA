using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Models.QueryModel;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Services
{
    public class InfectiousDiseaseService
    {
        InfectiousDiseaseRepository InfectiousDiseaseRepository = new InfectiousDiseaseRepository();
        CityRepository CityRepository = new CityRepository();
        TownRepository TownRepository = new TownRepository();
        public List<InfectiousDiseaseViewModel> GetByFilter(InfectiousDiseaseFilterParameter filter)
        {
            var models = InfectiousDiseaseRepository.GetByFilter(filter);

            if(models.IsEmptyOrNull())
            {
                return new List<InfectiousDiseaseViewModel>();
            }

            var citys = CityRepository.GetListByFilter(new CityFilterParameter 
            {
                CityIds = models.Select(e=>e.CityId).ToList()
            }).ToDictionary(e => e.Id, e => e.City);

            var towns = TownRepository.GetListByFilter(new TownFilterParameter
            {
                Ids = models.Select(e => e.TownId).ToList()
            }).ToDictionary(e => e.Id, e => e.Name);

            return models.Select(e => new InfectiousDiseaseViewModel
            {
                Id = e.Id,
                CityId = e.CityId,
                CityName = citys.GetValue(e.CityId),
                Date = e.Date,
                HomeInspectionGarbageAmount = e.HomeInspectionGarbageAmount,
                HomeQuarantineGarbageAmount = e.HomeQuarantineGarbageAmount,
                InspectionHotelGarbageAmount = e.InspectionHotelGarbageAmount,
                HomeInspectionCount = e.HomeInspectionCount,
                HomeQuarantineCount = e.HomeQuarantineCount,
                InspectionHotelCount = e.InspectionHotelCount,
                MaskCheckTimes = e.MaskCheckTimes,
                ReportTimes = e.ReportTimes,
                TownId = e.TownId,
                TownName = towns.GetValue(e.TownId)
            }).ToList();
        }

        public AdminResultModel Create(UserBriefModel user, InfectiousDiseaseModel model)
        {
            InfectiousDiseaseFilterParameter filter = new InfectiousDiseaseFilterParameter 
            { 
                Date = model.Date,
                TownIds = model.TownId.ToListCollection()
            };
            
            //已經存在
            if(InfectiousDiseaseRepository.IsExistsByFilter(filter))
            {
                return new AdminResultModel 
                {
                    IsSuccess = false,
                    ErrorMessage = "該日期已經輸入過，若要變更請點選修改"
                };
            }

            model.CreateDate = DateTimeHelper.GetCurrentTime();
            model.CreateUser = user.UserName;
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            model.UpdateUser = user.UserName;

            try
            {
                InfectiousDiseaseRepository.Create(model);
            }
            catch (Exception ex)
            {
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
            return new AdminResultModel
            {
                IsSuccess = true
            };
        }

        public AdminResultModel Delete(int id)
        {
            var entity = InfectiousDiseaseRepository.Get(id);

            if (entity == null)
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "資料不存在"
                };
            try
            {
                InfectiousDiseaseRepository.Delete(id);
            }
            catch (Exception ex)
            {
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
            return new AdminResultModel
            {
                IsSuccess = true
            };
        }

        public AdminResultModel Update(UserBriefModel user, InfectiousDiseaseModel model)
        {
            var entity = InfectiousDiseaseRepository.Get(model.Id);
            if(entity == null)
            {
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "查無此資料"
                };
            }
            entity.Date = model.Date;
            entity.HomeInspectionCount = model.HomeInspectionCount;
            entity.HomeInspectionGarbageAmount = model.HomeInspectionGarbageAmount;
            entity.HomeQuarantineCount = model.HomeQuarantineCount;
            entity.HomeQuarantineGarbageAmount = model.HomeQuarantineGarbageAmount;
            entity.InspectionHotelCount = model.InspectionHotelCount;
            entity.InspectionHotelGarbageAmount = model.InspectionHotelGarbageAmount;
            entity.MaskCheckTimes = model.MaskCheckTimes;
            entity.ReportTimes = model.ReportTimes;
            entity.UpdateDate = DateTimeHelper.GetCurrentTime();
            entity.UpdateUser = user.UserName;
            InfectiousDiseaseRepository.Update(entity);
            return new AdminResultModel
            {
                IsSuccess = true
            };
        }

        public List<InfectiousDiseaseStatisticsTownViewModel> GetTownStatistics(InfectiousDiseaseFilterParameter filter)
        {
            List<InfectiousDiseaseStatisticsModel> statistics = InfectiousDiseaseRepository.GetStatistics(filter);

            var result = statistics.GroupBy(e => new { e.TownId, e.TownName })
                .Select(e => new InfectiousDiseaseStatisticsTownViewModel
                {
                    TownId = e.Key.TownId,
                    TownName = e.Key.TownName,
                    HomeInspectionCount = e.Sum(f => f.HomeInspectionCount),
                    HomeInspectionGarbageAmount = e.Sum(f => f.HomeInspectionGarbageAmount),
                    HomeQuarantineCount = e.Sum(f => f.HomeQuarantineCount),
                    HomeQuarantineGarbageAmount = e.Sum(f => f.HomeQuarantineGarbageAmount),
                    InspectionHotelCount = e.Sum(f => f.InspectionHotelCount),
                    InspectionHotelGarbageAmount = e.Sum(f => f.InspectionHotelGarbageAmount),
                    MaskCheckTimes = e.Sum(f => f.MaskCheckTimes),
                    ReportTimes = e.Sum(f => f.ReportTimes),
                }).ToList();

            return result;
        }

        public List<InfectiousDiseaseStatisticsCityViewModel> GetCityStatistics(InfectiousDiseaseFilterParameter filter)
        {
            List<InfectiousDiseaseStatisticsModel> statistics = InfectiousDiseaseRepository.GetStatistics(filter);

            var result = statistics.GroupBy(e => new { e.CityId, e.CityName })
                .Select(e => new InfectiousDiseaseStatisticsCityViewModel
                {
                    CityId = e.Key.CityId,
                    CityName = e.Key.CityName,
                    HomeInspectionCount = e.Sum(f => f.HomeInspectionCount),
                    HomeInspectionGarbageAmount = e.Sum(f => f.HomeInspectionGarbageAmount),
                    HomeQuarantineCount = e.Sum(f => f.HomeQuarantineCount),
                    HomeQuarantineGarbageAmount = e.Sum(f => f.HomeQuarantineGarbageAmount),
                    InspectionHotelCount = e.Sum(f => f.InspectionHotelCount),
                    InspectionHotelGarbageAmount = e.Sum(f => f.InspectionHotelGarbageAmount),
                    MaskCheckTimes = e.Sum(f => f.MaskCheckTimes),
                    ReportTimes = e.Sum(f => f.ReportTimes),
                }).ToList();

            return result;
        }

        public InfectiousDiseaseModel Get(int id)
        {
            var entity = InfectiousDiseaseRepository.Get(id);
            return entity;
        }
    }
}