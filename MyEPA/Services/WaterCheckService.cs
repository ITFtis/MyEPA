using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Models.QueryModel;
using MyEPA.Models.SearchViewModel;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Services
{
    public class WaterCheckService
    {
        WaterCheckRepository WaterCheckRepository = new WaterCheckRepository();
        WaterCheckDetailRepository WaterCheckDetailRepository = new WaterCheckDetailRepository();
        DiasterRepository DiasterRepository = new DiasterRepository();
        CityRepository CityRepository = new CityRepository();
        UsersRepository UsersRepository = new UsersRepository();
        public List<WaterCheckViewModel> GetByDiasterId(int diasterId, UserBriefModel user)
        {
            DiasterModel diaster = DiasterRepository.Get(diasterId);

            if (diaster == null)
            {
                return new List<WaterCheckViewModel>();
            }
            List<int> cityIds = new List<int>() { user.CityId };
            WaterCheckFilterParameter filter =
                new WaterCheckFilterParameter
                {
                    
                     ////CityIds =cityIds,
                    DiasterIds = diasterId.ToListCollection(),
                    Types = user.Duty == DutyEnum.Water ? WaterCheckTypeEnum.Water.ToListCollection() : WaterCheckTypeEnum.EPPersonnel.ToListCollection()
                };

            var waterChecks = WaterCheckRepository
                .GetByFilter(filter);

            ////Brian：2024/04/08 => 改用DiasterId(查核ID)，跨縣市查詢
            ////var waterCheckDics =
            ////    waterChecks
            ////    .ToDictionary(e => e.CheckDate, e => e);

            List<string> userNames = new List<string>();
            if(user.Duty == DutyEnum.Water)
            {
                userNames = UsersRepository.GetListByFilter(new UsersBriefFilterParameter
                {
                    ////CityIds = user.CityId.ToListCollection(),
                    TownIds = user.TownId.ToListCollection()
                }).Select(e => e.UserName).ToList();
            }

            var datails = WaterCheckDetailRepository.GetByFilter(new WaterCheckDetailFilterParameter
            {
                ////CityIds = GetCityIds(user),
                UpdateUsers = userNames,
                WaterCheckIds = waterChecks.Select(e => e.Id).ToList()
            }).GroupBy(e => e.WaterCheckId).ToDictionary(e => e.Key, e => e.ToList());

            List<WaterCheckViewModel> result = new List<WaterCheckViewModel>();
            WaterCheckDetailService detailService = new WaterCheckDetailService();
            ////for (DateTime date = diaster.StartTime.Date; date <= diaster.EndTime.AddDays(7); date = date.AddDays(1))
            for (DateTime date = DateTime.Parse("2024/04/08"); date <= diaster.EndTime.AddDays(7); date = date.AddDays(1))
            {
                WaterCheckViewModel vm = null;

                var waterCheck = waterChecks.Where(a => a.CheckDate.ToShortDateString() == date.ToShortDateString()).FirstOrDefault();

                ////if (waterCheckDics.ContainsKey(date))
                if (waterCheck != null)
                {
                    ////WaterCheckModel waterCheck = waterCheckDics[date];

                    Dictionary<WaterCheckDetailStatusEnum, int> detailStatus = new Dictionary<WaterCheckDetailStatusEnum, int>();
                    WaterCheckDetailStatusViewModel detailStatusVM = null;
                    bool isHasDateils = false;
                    if (datails.ContainsKey(waterCheck.Id))
                    {
                        List<WaterCheckDetailModel> waterCheckdatails = datails[waterCheck.Id];

                        isHasDateils = waterCheckdatails.IsNotEmpty();

                        foreach (var item in datails[waterCheck.Id])
                        {
                            WaterCheckDetailStatusEnum status = detailService.GetDetailStatus(item);
                            if (detailStatus.ContainsKey(status) == false)
                            {
                                detailStatus.Add(status, 0);
                            }
                            detailStatus[status] += 1;
                        }
                        detailStatusVM = new WaterCheckDetailStatusViewModel
                        {
                            Cannot = detailStatus.GetValue(WaterCheckDetailStatusEnum.Cannot, 0),
                            Failed = detailStatus.GetValue(WaterCheckDetailStatusEnum.Failed, 0),
                            NothingHappened = detailStatus.GetValue(WaterCheckDetailStatusEnum.NothingHappened, 0),
                            Success = detailStatus.GetValue(WaterCheckDetailStatusEnum.Success, 0),
                            Testing = detailStatus.GetValue(WaterCheckDetailStatusEnum.Testing, 0),
                        };
                    }
                    vm = new WaterCheckViewModel
                    {
                        IsHasDateils = isHasDateils,
                        CheckDate = waterCheck.CheckDate,
                        CityId = waterCheck.CityId,
                        CityName = waterCheck.CityName,
                        DiasterId = waterCheck.DiasterId,
                        DetailStatus = detailStatusVM,
                        Id = waterCheck.Id,
                        Memo = waterCheck.Memo,
                        Status = waterCheck.Status,
                        TownId = waterCheck.TownId,
                        TownName = waterCheck.TownName,
                        Type = waterCheck.Type,
                        UpdateTime = waterCheck.UpdateTime,
                        UpdateUser = waterCheck.UpdateUser
                    };
                }
                else
                {
                    vm = new WaterCheckViewModel
                    {
                        CheckDate = date,
                        CityId = user.CityId,
                        DiasterId = diasterId,
                        Memo = string.Empty,
                        Status = WaterCheckStatusEnum.Pending.ToInteger(),
                    };
                }
                result.Add(vm);
            }

            return result;
        }

        public List<int> GetCityIds(UserBriefModel user)
        {
            List<int> citys = new List<int>();

            switch (user.Duty)
            {
                case DutyEnum.EPB:
                case DutyEnum.Water:
                
                    citys.Add(user.CityId);
                    break;
                case DutyEnum.Cleaning:
                case DutyEnum.EPA:
                case DutyEnum.Team:
                case DutyEnum.Corps:
                    break;
            }
            return citys;
        }
        public List<WaterCheckModel> GetByFilter(WaterCheckFilterParameter filter)
        {
            return WaterCheckRepository.GetByFilter(filter);
        }

        public WaterCheckModel Get(int id)
        {
            return WaterCheckRepository.Get(id);
        }

        public AdminResultModel UpdateMemo(UserBriefModel user, WaterCheckModel waterCheck)
        {
            WaterCheckModel model = WaterCheckRepository.Get(waterCheck.Id);
            if (model == null)
            {
                model = CreateAndReturn(user, waterCheck);
            }
            else
            {
                model.Memo = waterCheck.Memo;
                model.UpdateTime = DateTimeHelper.GetCurrentTime();
                model.UpdateUser = user.UserName;
            }
            WaterCheckRepository.CreateOrUpdate(model);
            return new AdminResultModel
            {
                IsSuccess = true
            };
        }

        public List<WaterCheckReportModel> GetReport(int diasterId)
        {
            return WaterCheckDetailRepository.GetReport(diasterId, WaterCheckTypeEnum.EPPersonnel);
        }

        public WaterCheckModel CreateAndReturn(UserBriefModel user, WaterCheckModel waterCheck)
        {
            WaterCheckModel model = new WaterCheckModel
            {
                CheckDate = waterCheck.CheckDate,
                CityId = user.CityId,
                CityName = user.City,
                DiasterId = waterCheck.DiasterId,
                Memo = waterCheck.Memo,
                Status = waterCheck.Status,
                TownId = user.TownId,
                TownName = user.Town,
                UpdateTime = DateTimeHelper.GetCurrentTime(),
                UpdateUser = user.UserName,
                Type = user.Duty == DutyEnum.Water ? WaterCheckTypeEnum.Water : WaterCheckTypeEnum.EPPersonnel
            };
            var id = WaterCheckRepository.CreateAndResultIdentity<int>(model);
            model.Id = id;
            return model;
        }

        public List<WaterCheckDetailModel> GetDetailStatistics(DetailStatisticsSearchViewModel search)
        {
            var waterChecks = WaterCheckRepository.GetByFilter(new WaterCheckFilterParameter
            {
                DiasterIds = search.DiasterId.HasValue ? search.DiasterId.Value.ToListCollection() : new List<int>(),
                Types = search.Type.HasValue ? search.Type.Value.ToListCollection() : new List<WaterCheckTypeEnum>()
            });

            List<int> cityIds = search.CityId.HasValue ? search.CityId.Value.ToListCollection() : new List<int>();

            if (search.WaterDivisionId.HasValue)
            {
                cityIds = CityRepository.GetListByFilter(new CityFilterParameter
                {
                    WaterDivisionIds = search.WaterDivisionId.Value.ToListCollection()
                }).Select(e => e.Id).ToList();
            }

            return WaterCheckDetailRepository.GetByFilter(new WaterCheckDetailFilterParameter
            {
                WaterCheckIds = waterChecks.Select(e => e.Id).ToList(),
                CityIds = cityIds,
                Rechecks = search.Recheck.HasValue ? search.Recheck.Value.ToListCollection() : new List<WaterCheckDetailRecheckEnum>()
            });
        }

        public List<WaterCheckStatisticsViewModel> Statistics(int diasterId)
        {
            var waterDivisionsGroupBy = CityRepository.GetWaterDivisions().GroupBy(e => new { e.WaterDivisionId ,e.WaterDivision}, e => e);

            var waterCheckStatistics =
                WaterCheckRepository.GetWaterCheckStatistics(diasterId)
                .GroupBy(e=> new { e.CityId,e.Type})
                .ToMultiKeyDictionary(e => e.Key.CityId,e=>e.Key.Type, e => e.ToList());

            var result = new List<WaterCheckStatisticsViewModel>();

            foreach (var waterDivisions in waterDivisionsGroupBy)
            {
                var waterCount = 0;
                var waterDisqualifiedCount = 0;

                foreach (var cityId in waterDivisions.Select(e => e.CityId))
                {
                    var water = waterCheckStatistics.GetValue(cityId, WaterCheckTypeEnum.Water, new List<WaterCheckStatisticsQueryModel>());

                    waterCount += water.Sum(e => e.Count);
                    waterDisqualifiedCount += water.Where(f => f.Status == WaterCheckDetailStatusEnum.Failed).Sum(e => e.Count);
                }
               
                WaterCheckStatisticsViewModel waterCheck = new WaterCheckStatisticsViewModel
                {
                    EPs = new List<WaterCheckRecheckCountEPViewModel> { },
                    Water = new WaterCheckRecheckCountViewModel
                    {
                        Count = waterCount,
                        DisqualifiedCount = waterDisqualifiedCount
                    },
                    WaterDivision = waterDivisions.Key.WaterDivision,
                    WaterDivisionId = waterDivisions.Key.WaterDivisionId
                };

                foreach (var waterDivision in waterDivisions)
                {
                    var ep = waterCheckStatistics.GetValue(waterDivision.CityId, WaterCheckTypeEnum.EPPersonnel, new List<WaterCheckStatisticsQueryModel>());
                    waterCheck.EPs.Add(new WaterCheckRecheckCountEPViewModel
                    {
                        City = waterDivision.City,
                        CityId = waterDivision.CityId,
                        Count = ep.Sum(e => e.Count),
                        DisqualifiedCount = ep.Where(f => f.Status == WaterCheckDetailStatusEnum.Failed).Sum(e => e.Count)
                    });
                }

                result.Add(waterCheck);
            }
            return result;
        }

        public AdminResultModel UpdateStatus(UserBriefModel user, WaterCheckModel waterCheck)
        {
            WaterCheckModel model = WaterCheckRepository.Get(waterCheck.Id);
            if (model == null)
            {
                model = CreateAndReturn(user, waterCheck);
            }
            else
            {
                model.Status = waterCheck.Status;
                model.UpdateTime = DateTimeHelper.GetCurrentTime();
                model.UpdateUser = user.UserName;
            }
            WaterCheckRepository.CreateOrUpdate(model);
            return new AdminResultModel
            {
                IsSuccess = true
            };
        }
    }
}