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
            for (DateTime date = diaster.StartTime.Date; date <= diaster.EndTime.AddDays(7); date = date.AddDays(1))
            //for (DateTime date = DateTime.Parse("2024/04/09"); date <= diaster.EndTime.AddDays(7); date = date.AddDays(1))                                                
            {
                WaterCheckViewModel vm = null;

                var datas = waterChecks.Where(a => a.CheckDate.ToShortDateString() == date.ToShortDateString()).ToList();

                //WaterCheckIds (已檢驗WaterDetail)
                List<int> WaterCheckIds = new List<int>();
                foreach (var v in datails.Values)
                {
                    var z = v.Where(a => a.UpdateUser == user.UserName)
                                .Select(a => a.WaterCheckId)
                                .Distinct().ToList();

                    WaterCheckIds.AddRange(z);
                }
                WaterCheckIds = WaterCheckIds.OrderBy(a => a).ToList();

                //水質檢驗的id
                WaterCheckModel waterCheck = null;
                if (WaterCheckIds.Count > 0)
                {
                    //(已檢驗                    
                    waterCheck = datas.Where(a => WaterCheckIds.Contains(a.Id)).FirstOrDefault();
                }

                //本日無災情(自己勾選)
                var noDisaster = datas.Where(a => a.UpdateUser == user.UserName).FirstOrDefault();
                if (noDisaster != null)
                {                    
                    waterCheck = noDisaster;
                }

                //var waterCheck = waterChecks.Where(a => a.CheckDate.ToShortDateString() == date.ToShortDateString()).FirstOrDefault();

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

                        //綁自己
                        waterCheckdatails = waterCheckdatails.Where(a => a.UpdateUser == user.UserName).ToList();

                        isHasDateils = waterCheckdatails.IsNotEmpty();

                        foreach (var item in waterCheckdatails)
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
                Status = search.Status.HasValue ? search.Status.Value.ToListCollection() : new List<WaterCheckDetailStatusEnum>(),
            });
        }

        public List<WaterCheckStatisticsViewModel> Statistics(int diasterId)
        {
            var citys = CityRepository.GetWaterDivisions();

            var waterCheckStatistics =
                WaterCheckRepository.GetWaterCheckStatistics(diasterId)
                .GroupBy(e=> new { e.CityId,e.Type})
                .ToMultiKeyDictionary(e => e.Key.CityId,e=>e.Key.Type, e => e.ToList());

            var result = new List<WaterCheckStatisticsViewModel>();

            //每個縣市都有管理處(故可用管理處做foreach)
            foreach (var city in citys)
            {
                //自來水人員
                var water = waterCheckStatistics.GetValue(city.CityId, WaterCheckTypeEnum.Water, new List<WaterCheckStatisticsQueryModel>());

                //環保人員
                var ep = waterCheckStatistics.GetValue(city.CityId, WaterCheckTypeEnum.EPPersonnel, new List<WaterCheckStatisticsQueryModel>());

                WaterCheckStatisticsViewModel waterCheck = new WaterCheckStatisticsViewModel
                {
                    WaterDivision = city.WaterDivision,
                    WaterDivisionId = city.WaterDivisionId,
                    City = city.City,
                    CityId = city.CityId,
                    WaterCount = water.Sum(e => e.Count),
                    WaterDisqualifiedCount = water.Where(f => f.Status == WaterCheckDetailStatusEnum.Failed).Sum(e => e.Count),
                    EPsCount = ep.Sum(e => e.Count),
                    EPsDisqualifiedCount = ep.Where(f => f.Status == WaterCheckDetailStatusEnum.Failed).Sum(e => e.Count),
                };

                result.Add(waterCheck);
            }
            return result;
        }

        public List<WaterCheckStatisticsDetailViewModel> StatisticsDetail(int diasterId)
        {
            var citys = CityRepository.GetWaterDivisions();

            var details = WaterCheckRepository.GetWaterCheckStatisticsDetail(diasterId);

            var result = new List<WaterCheckStatisticsDetailViewModel>();

            //每個縣市都有管理處(故可用管理處做foreach)
            details = details.Where(a => a.Type == WaterCheckTypeEnum.Water || a.Type == WaterCheckTypeEnum.EPPersonnel).ToList();
            foreach (var city in citys)
            {
                var datas = details.Where(a => a.CityId == city.CityId).ToList();

                //自來水人員 + 環保人員
                WaterCheckStatisticsDetailViewModel waterCheck = new WaterCheckStatisticsDetailViewModel
                {
                    City = city.City,
                    CityId = city.CityId,
                    Count = datas.Count,
                    DisqualifiedCount = datas.Where(f => f.Status == WaterCheckDetailStatusEnum.Failed).Count(),
                    DisqualifiedAddress = string.Join("\n", datas.Where(f => f.Status == WaterCheckDetailStatusEnum.Failed)
                                                                .Select((a, index) => "(" + (index + 1).ToString() + ")" + a.Address))
                };

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