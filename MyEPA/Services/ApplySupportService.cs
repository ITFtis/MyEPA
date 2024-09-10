using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.BaseModels;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.Repositories.BaseRepositories;
using MyEPA.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MyEPA.Services
{
    public class ApplySupportService
    {
        public TownRepository TownRepository = new TownRepository();
        public CityRepository CityRepository = new CityRepository();
        public ApplyCarRepository ApplyCarRepository = new ApplyCarRepository();
        public ApplyMedicineRepository ApplyMedicineRepository = new ApplyMedicineRepository();
        public ApplyDisinfectionEquipmentRepository ApplyDisinfectionEquipmentRepository = new ApplyDisinfectionEquipmentRepository();
        public ApplyOtherRepository ApplyOtherRepository = new ApplyOtherRepository();
        public ApplyPeopleRepository ApplyPeopleRepository = new ApplyPeopleRepository();
        public ApplySubsidyRepository ApplySubsidyRepository = new ApplySubsidyRepository();
        public ApplySupportRepository ApplySupportRepository = new ApplySupportRepository();
        const string TotalCountTemplate = "請求共{0}項";
        const string TotalCountToEpaTemplate = "其中有{0}筆轉呈環境部(含環保局的請求)";

        public ApplySupportReportModel GetReport(ApplySupportReportFilterModel filterModel) 
        {
            var result = new ApplySupportReportModel();
            switch ((ApplyTypeEnum)filterModel.ApplyTypeId) 
            {
                case ApplyTypeEnum.ApplyPeople:
                    result = GetApplyPeopleReport(filterModel);
                    break;
                case ApplyTypeEnum.ApplyMedicine:
                    result = GetApplyMedicineReport(filterModel);
                    break;
                case ApplyTypeEnum.ApplySubsidy:
                    result = GetApplySubsidyReport(filterModel);
                    break;
                case ApplyTypeEnum.ApplyOther:
                    result = GetApplyOtherReport(filterModel);
                    break;
                case ApplyTypeEnum.ApplyCar:
                    result = GetApplyCarReport(filterModel);
                    break;
                case ApplyTypeEnum.ApplyDisinfectionEquipment:
                    result = GetApplyDisinfectionEquipmentReport(filterModel);
                    break;
                default:
                    break;
            }

            return result;
        }

        public AdminResultModel EPBConfirm(UserBriefModel userBrief, ApplySupportConfirmViewModel confirm)
        {
            if (userBrief == null || userBrief.Duty != DutyEnum.EPB)
            {
                return new AdminResultModel()
                {
                    ErrorMessage = "沒有權限",
                    IsSuccess = false
                };
            }
            return Confirm(confirm, userBrief.Duty, userBrief.UserName);
        }

        private AdminResultModel Confirm(ApplySupportConfirmViewModel confirm, DutyEnum duty, string userName)
        {
            if (confirm == null)
            {
                return new AdminResultModel()
                {
                    ErrorMessage = "無任何參數",
                    IsSuccess = false
                };
            }

            switch (confirm.ApplyType)
            {
                case ApplyTypeEnum.ApplyPeople:
                    {
                        ChangeConfirmStatus(ApplyPeopleRepository, confirm.Id, duty, userName);
                    }
                    break;
                case ApplyTypeEnum.ApplyMedicine:
                    {
                        ChangeConfirmStatus(ApplyMedicineRepository, confirm.Id, duty, userName);
                    }
                    break;
                case ApplyTypeEnum.ApplySubsidy:
                    {
                        ChangeConfirmStatus(ApplySubsidyRepository, confirm.Id, duty, userName);
                    }
                    break;
                case ApplyTypeEnum.ApplyOther:
                    {
                        ChangeConfirmStatus(ApplyOtherRepository, confirm.Id, duty, userName);
                    }
                    break;
                case ApplyTypeEnum.ApplyCar:
                    {
                        ChangeConfirmStatus(ApplyCarRepository, confirm.Id, duty, userName);
                    }
                    break;
                case ApplyTypeEnum.ApplyDisinfectionEquipment:
                    {
                        ChangeConfirmStatus(ApplyDisinfectionEquipmentRepository, confirm.Id, duty, userName);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            return new AdminResultModel
            {
                IsSuccess = true
            };
        }

        
        
        public AdminResultModel EPAConfirm(UserBriefModel userBrief, ApplySupportConfirmViewModel confirm)
        {
            if (userBrief == null || userBrief.Duty != DutyEnum.EPA)
            {
                return new AdminResultModel()
                {
                    ErrorMessage = "沒有權限",
                    IsSuccess = false
                };
            }
            return Confirm(confirm, userBrief.Duty, userBrief.UserName);
        }

        public AdminResultModel EPBCancel(UserBriefModel userBrief, ApplySupportCancelViewModel cancel)
        {
            if (userBrief == null || userBrief.Duty != DutyEnum.EPB)
            {
                return new AdminResultModel()
                {
                    ErrorMessage = "沒有權限",
                    IsSuccess = false
                };
            }
            return Cancel(cancel, userBrief.Duty, userBrief.UserName);
        }
        public AdminResultModel EPACancel(UserBriefModel userBrief, ApplySupportCancelViewModel cancel)
        {
            if (userBrief == null || userBrief.Duty != DutyEnum.EPA)
            {
                return new AdminResultModel()
                {
                    ErrorMessage = "沒有權限",
                    IsSuccess = false
                };
            }
            return Cancel(cancel, userBrief.Duty, userBrief.UserName);
        }
        private AdminResultModel Cancel(ApplySupportCancelViewModel cancel,DutyEnum duty, string userName)
        {
            if (cancel == null)
            {
                return new AdminResultModel()
                {
                    ErrorMessage = "無任何參數",
                    IsSuccess = false
                };
            }

            switch (cancel.ApplyType)
            {
                case ApplyTypeEnum.ApplyPeople:
                    {
                        ChangeCancelStatus(ApplyPeopleRepository, cancel.Id, duty, userName);
                    }
                    break;
                case ApplyTypeEnum.ApplyMedicine:
                    {
                        ChangeCancelStatus(ApplyMedicineRepository, cancel.Id, duty, userName);
                    }
                    break;
                case ApplyTypeEnum.ApplySubsidy:
                    {
                        ChangeCancelStatus(ApplySubsidyRepository, cancel.Id, duty, userName);
                    }
                    break;
                case ApplyTypeEnum.ApplyOther:
                    {
                        ChangeCancelStatus(ApplyOtherRepository, cancel.Id, duty, userName);
                    }
                    break;
                case ApplyTypeEnum.ApplyCar:
                    {
                        ChangeCancelStatus(ApplyCarRepository, cancel.Id, duty, userName);
                    }
                    break;
                case ApplyTypeEnum.ApplyDisinfectionEquipment:
                    {
                        ChangeCancelStatus(ApplyDisinfectionEquipmentRepository, cancel.Id, duty, userName);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            return new AdminResultModel
            {
                IsSuccess = true
            };
        }
        private void ChangeConfirmStatus<T>(ApplyBaseRepositroy<T> repositroy, int id, DutyEnum duty, string userName)
            where T : ApplyBaseModel,new()
        {
            ChangeStatus(repositroy, id, duty, userName, ApplyStatusEnum.Confrim);
        }
        private void ChangeCancelStatus<T>(ApplyBaseRepositroy<T> repositroy, int id, DutyEnum duty, string userName)
            where T : ApplyBaseModel, new()
        {
            ChangeStatus(repositroy, id, duty, userName, ApplyStatusEnum.Processing);
        }
        private void ChangeStatus<T>(ApplyBaseRepositroy<T> repositroy, int id, DutyEnum duty, string userName, ApplyStatusEnum status)
            where T : ApplyBaseModel, new()
        {
            var apply = repositroy.Get(id);
            apply.Status = status;
            if (duty == DutyEnum.EPA)
            {
                apply.EPAConfirmStatus = status;
            }
            else if (duty == DutyEnum.EPB)
            {
                apply.EPBConfirmStatus = status;
            }

            apply.UpdateDate = DateTimeHelper.GetCurrentTime();
            apply.UpdateUser = userName;
            repositroy.Update(apply);
        }
        public List<ApplySupportTownStatusCountViewModel> GetTownStatusReportCountByFilter(ApplySupportReportFilterModel reportFilter)
        {
            ApplyBaseFilterParameter filter = GetApplyBaseFilter(reportFilter);
            var applyCarStatusCount = ApplyCarRepository.GetStatusCountByFilter(filter);
            var applyMedicineStatusCount = ApplyMedicineRepository.GetStatusCountByFilter(filter);
            var applyDisinfectionEquipmentStatusCount = ApplyDisinfectionEquipmentRepository.GetStatusCountByFilter(filter);
            var applyOtherStatusCount = ApplyOtherRepository.GetStatusCountByFilter(filter);
            var applyPeopleStatusCount = ApplyPeopleRepository.GetStatusCountByFilter(filter);
            var applySubsidyStatusCount = ApplySubsidyRepository.GetStatusCountByFilter(filter);
            var sumPrice =
                ApplySubsidyRepository.GetSumPriceByDiasterId(reportFilter.DiasterId)
                .GroupBy(e => e.TownId).ToDictionary(e => e.Key, e => e.Sum(f => f.SumPrice));

            var towns = TownRepository.GetByCityId(reportFilter.CityId.Value);

            return towns.Select(e =>
            {
                ApplyBaseFilterParameter epaConfrimFilter = new ApplyBaseFilterParameter
                {
                    IsToEpa = true,
                    Status = ApplyStatusEnum.Confrim,
                    TownIds = e.Id.ToListCollection()
                };
                ApplyBaseFilterParameter epbConfrimFilter = new ApplyBaseFilterParameter
                {
                    IsToEpa = false,
                    Status = ApplyStatusEnum.Confrim,
                    TownIds = e.Id.ToListCollection()
                };
                ApplyBaseFilterParameter countFilter = new ApplyBaseFilterParameter
                {
                    TownIds = e.Id.ToListCollection()
                };
                return new ApplySupportTownStatusCountViewModel
                {
                    ApplyCarCount = GetCountByFilter(applyCarStatusCount, countFilter),
                    ApplyCarEpaConfrimCount = GetCountByFilter(applyCarStatusCount, epaConfrimFilter),
                    ApplyCarEpbConfrimCount = GetCountByFilter(applyCarStatusCount, epbConfrimFilter),
                    ApplyMedicineCount = GetCountByFilter(applyMedicineStatusCount, countFilter),
                    ApplyMedicineEpaConfrimCount = GetCountByFilter(applyMedicineStatusCount, epaConfrimFilter),
                    ApplyMedicineEpbConfrimCount = GetCountByFilter(applyMedicineStatusCount, epbConfrimFilter),
                    ApplyDisinfectionEquipmentCount = GetCountByFilter(applyDisinfectionEquipmentStatusCount, countFilter),
                    ApplyDisinfectionEquipmentEpaConfrimCount = GetCountByFilter(applyDisinfectionEquipmentStatusCount, epaConfrimFilter),
                    ApplyDisinfectionEquipmentEpbConfrimCount = GetCountByFilter(applyDisinfectionEquipmentStatusCount, epbConfrimFilter),
                    ApplyOtherCount = GetCountByFilter(applyOtherStatusCount, countFilter),
                    ApplyOtherEpaConfrimCount = GetCountByFilter(applyOtherStatusCount, epaConfrimFilter),
                    ApplyOtherEpbConfrimCount = GetCountByFilter(applyOtherStatusCount, epbConfrimFilter),
                    ApplyPeopleCount = GetCountByFilter(applyPeopleStatusCount, countFilter),
                    ApplyPeopleEpaConfrimCount = GetCountByFilter(applyPeopleStatusCount, epaConfrimFilter),
                    ApplyPeopleEpbConfrimCount = GetCountByFilter(applyPeopleStatusCount, epbConfrimFilter),
                    ApplySubsidyCount = GetCountByFilter(applySubsidyStatusCount, countFilter),
                    ApplySubsidyEpaConfrimCount = GetCountByFilter(applySubsidyStatusCount, epaConfrimFilter),
                    ApplySubsidyEpbConfrimCount = GetCountByFilter(applySubsidyStatusCount, epbConfrimFilter),
                    TownId = e.Id,
                    TownName = e.Name,
                    Money = sumPrice.GetValue(e.Id)
                };
            }).ToList();
        }

        public List<ApplySupportCityStatusCountViewModel> GetCityStatusReportCountByFilter(ApplySupportReportFilterModel reportFilter)
        {
            ApplyBaseFilterParameter filter = GetApplyBaseFilter(reportFilter);

            var applyCarStatusCount = ApplyCarRepository.GetStatusCountByFilter(filter);
            var applyMedicineStatusCount = ApplyMedicineRepository.GetStatusCountByFilter(filter);
            var applyDisinfectionEquipmentStatusCount = ApplyDisinfectionEquipmentRepository.GetStatusCountByFilter(filter);
            var applyOtherStatusCount = ApplyOtherRepository.GetStatusCountByFilter(filter);
            var applyPeopleStatusCount = ApplyPeopleRepository.GetStatusCountByFilter(filter);
            var applySubsidyStatusCount = ApplySubsidyRepository.GetStatusCountByFilter(filter);

            var sumPrice =
                ApplySubsidyRepository.GetSumPriceByDiasterId(reportFilter.DiasterId)
                .GroupBy(e => e.CityId).ToDictionary(e => e.Key, e => e.Sum(f => f.SumPrice));

            var allApplySumPrice = ApplySupportRepository.GetAllPrice(reportFilter.DiasterId);

            var citys = CityRepository.GetListByFilter(new CityFilterParameter
            {
                IsCounty = true,
            });

            return citys.OrderBy(e => e.Sort).Select(e =>
              {
                  ApplyBaseFilterParameter epaConfrimFilter = new ApplyBaseFilterParameter
                  {
                      IsToEpa = true,
                      EPAConfirmStatus = ApplyStatusEnum.Confrim.ToListCollection(),
                      CityIds = e.Id.ToListCollection()
                  };
                  ApplyBaseFilterParameter epbConfrimFilter = new ApplyBaseFilterParameter
                  {
                      IsToEpa = false,
                      EPBConfirmStatus = ApplyStatusEnum.Confrim.ToListCollection(),
                      CityIds = e.Id.ToListCollection()
                  };
                  ApplyBaseFilterParameter countFilter = new ApplyBaseFilterParameter
                  {
                      CityIds = e.Id.ToListCollection()
                  };
                  decimal applyOtherEpaConfrimCount = GetCountByFilter(applyOtherStatusCount, epaConfrimFilter);
                  decimal applySubsidyEpaConfrimCount = GetCountByFilter(applySubsidyStatusCount, epaConfrimFilter);
                  return new ApplySupportCityStatusCountViewModel
                  {
                      ApplyCarCount = GetCountByFilter(applyCarStatusCount, countFilter),
                      ApplyCarEpaConfrimCount = GetCountByFilter(applyCarStatusCount, epaConfrimFilter),
                      ApplyCarEpbConfrimCount = GetCountByFilter(applyCarStatusCount, epbConfrimFilter),
                      ApplyMedicineCount = GetCountByFilter(applyMedicineStatusCount, countFilter),
                      ApplyMedicineEpaConfrimCount = GetCountByFilter(applyMedicineStatusCount, epaConfrimFilter),
                      ApplyMedicineEpbConfrimCount = GetCountByFilter(applyMedicineStatusCount, epbConfrimFilter),
                      ApplyDisinfectionEquipmentCount = GetCountByFilter(applyDisinfectionEquipmentStatusCount, countFilter),
                      ApplyDisinfectionEquipmentEpaConfrimCount = GetCountByFilter(applyDisinfectionEquipmentStatusCount, epaConfrimFilter),
                      ApplyDisinfectionEquipmentEpbConfrimCount = GetCountByFilter(applyDisinfectionEquipmentStatusCount, epbConfrimFilter),
                      ApplyOtherCount = GetCountByFilter(applyOtherStatusCount, countFilter),
                      ApplyOtherEpaConfrimCount = applyOtherEpaConfrimCount,
                      ApplyOtherEpbConfrimCount = GetCountByFilter(applyOtherStatusCount, epbConfrimFilter),
                      ApplyPeopleCount = GetCountByFilter(applyPeopleStatusCount, countFilter),
                      ApplyPeopleEpaConfrimCount = GetCountByFilter(applyPeopleStatusCount, epaConfrimFilter),
                      ApplyPeopleEpbConfrimCount = GetCountByFilter(applyPeopleStatusCount, epbConfrimFilter),
                      ApplySubsidyCount = GetCountByFilter(applySubsidyStatusCount, countFilter),
                      ApplySubsidyEpaConfrimCount = applySubsidyEpaConfrimCount,
                      ApplySubsidyEpbConfrimCount = GetCountByFilter(applySubsidyStatusCount, epbConfrimFilter),
                      CityId = e.Id,
                      CityName = e.City,
                      Money = applySubsidyEpaConfrimCount + allApplySumPrice
                  };
              }).ToList();
        }
        public ApplySupportEPBStatusCountReportViewModel GetEPBStatusCount(int diasterId ,int cityId)
        {
            var towns = TownRepository.GetListByFilter(new TownFilterParameter
            {
                CityIds = cityId.ToListCollection(),
                IsTown = true
            });
            var filter = new ApplyBaseFilterParameter()
            {
                DiasterIds = diasterId.ToListCollection(),
                TownIds = towns.Select(e => e.Id)
            };
            var applyCarStatusCount = ApplyCarRepository.GetEPBStatusCountByFilter(filter);
            var applyMedicineStatusCount = ApplyMedicineRepository.GetEPBStatusCountByFilter(filter);
            var applyDisinfectionEquipmentStatusCount = ApplyDisinfectionEquipmentRepository.GetEPBStatusCountByFilter(filter);
            var applyOtherStatusCount = ApplyOtherRepository.GetEPBStatusCountByFilter(filter);
            var applyPeopleStatusCount = ApplyPeopleRepository.GetEPBStatusCountByFilter(filter);
            var applySubsidyStatusCount = ApplySubsidyRepository.GetEPBStatusCountByFilter(filter);

            return new ApplySupportEPBStatusCountReportViewModel()
            {
                ApplyCar = GetEPBStatusCount(applyCarStatusCount),
                ApplyDisinfectionEquipment = GetEPBStatusCount(applyDisinfectionEquipmentStatusCount),
                ApplyMedicine = GetEPBStatusCount(applyMedicineStatusCount),
                ApplyOther = GetEPBStatusCount(applyOtherStatusCount),
                ApplyPeople = GetEPBStatusCount(applyPeopleStatusCount),
                ApplySubsidy = GetEPBStatusCount(applySubsidyStatusCount),
            };
        }
        private ApplySupportEPBStatusCountViewModel GetEPBStatusCount(List<ApplyBaseEPBStatusCountModel> input)
        {
            return new ApplySupportEPBStatusCountViewModel
            {
                ConfrimCount = input.Count(e=>e.EPBConfirmStatus == ApplyStatusEnum.Confrim),
                PendingCount = input.Count(e => e.EPBConfirmStatus == ApplyStatusEnum.Pending),
                ProcessingCount = input.Count(e => e.EPBConfirmStatus == ApplyStatusEnum.Processing),
                RejectCount = input.Count(e => e.EPBConfirmStatus == ApplyStatusEnum.Reject),
                SendToEpaCount = input.Count(e => e.EPBConfirmStatus == ApplyStatusEnum.SendToEpa)
            };
        }
        private decimal GetCountByFilter(List<ApplyBaseStatusCountModel> statusCount, ApplyBaseFilterParameter filter)
        {
            IEnumerable<ApplyBaseStatusCountModel> list = statusCount;
            if (filter.IsToEpa.HasValue)
            {
                list = list.Where(e => e.IsToEpa == filter.IsToEpa);
            }
            if (filter.CityIds.IsNotEmpty())
            {
                list = list.Where(e => filter.CityIds.Contains(e.CityId));
            }
            if (filter.TownIds.IsNotEmpty())
            {
                list = list.Where(e => filter.TownIds.Contains(e.TownId));
            }
            if (filter.EPAConfirmStatus.IsNotEmpty())
            {
                list = list.Where(e => filter.EPAConfirmStatus.Contains(e.EPAConfirmStatus));
            }
            if (filter.EPBConfirmStatus.IsNotEmpty())
            {
                list = list.Where(e => filter.EPBConfirmStatus.Contains(e.EPBConfirmStatus));
            }
            return list.Sum(e => e.Count);
        }
        public ApplySupportProcessingViewModel EPAProcessingViewModel(UserBriefModel userBrief, int diasterId, int? townId, int? applyTypeId, ApplyStatusEnum? status)
        {
            var filter = new ApplyBaseFilterParameter()
            {
                DiasterIds = new List<int>() { diasterId },
                IsToEpa = true,
                EPAConfirmStatus = status.HasValue ? status.Value.ToListCollection() : new List<ApplyStatusEnum>()
            };


            if (townId.HasValue)
            {
                filter.TownIds = new List<int>() { townId.Value };
            }

            var result = new ApplySupportProcessingViewModel();

            var applyTypes = new List<string>();
            if (applyTypeId.HasValue == false)
            {
                applyTypes = Enum.GetNames(typeof(ApplyTypeEnum))
                                 .ToList();
            }
            else
            {
                if (Enum.TryParse<ApplyTypeEnum>(applyTypeId.ToString(), out var parsedEnum))
                {
                    applyTypes.Add(parsedEnum.ToString());
                }
            }

            foreach (var applyTypeName in applyTypes)
            {
                var serviceName = $"MyEPA.Services.{applyTypeName}Service";
                var service = Type.GetType(serviceName);
                var instance = Activator.CreateInstance(service);
                MethodInfo method = service.GetMethods()
                                           .FirstOrDefault(c => c.Name == "GetApplyViewModelsByFilter");
                var queryResult = method.Invoke(instance, new object[] { filter });
                var prop = typeof(ApplySupportProcessingViewModel).GetProperty(applyTypeName);
                prop.SetValue(result, queryResult);
            }


            return result;
        }
        public static ApplySupportProcessingViewModel ProcessingViewModel(UserBriefModel userBrief, int diasterId, int? townId, int applyTypeId)
        {
            var filter = new ApplyBaseFilterParameter()
            {
                CityIds = new List<int>() { userBrief.CityId },
                DiasterIds = new List<int>() { diasterId }
            };


            if (townId.HasValue) 
            {
                filter.TownIds = new List<int>() { townId.Value };
            }

            var result = new ApplySupportProcessingViewModel();

            var applyTypes = new List<string>();
            if (applyTypeId == 0)
            {
                applyTypes = Enum.GetNames(typeof(ApplyTypeEnum))
                                 .ToList();
            }
            else 
            {
                if (Enum.TryParse<ApplyTypeEnum>(applyTypeId.ToString(), out var parsedEnum)) 
                {
                    applyTypes.Add(parsedEnum.ToString());
                }
            }

            foreach (var applyTypeName in applyTypes)
            {
                var serviceName = $"MyEPA.Services.{applyTypeName}Service";
                var service = Type.GetType(serviceName);
                var instance = Activator.CreateInstance(service);
                MethodInfo method = service.GetMethods()
                                           .FirstOrDefault(c => c.Name == "GetApplyViewModelsByFilter");
                var  queryResult = method.Invoke(instance, new object[] { filter });
                var prop = typeof(ApplySupportProcessingViewModel).GetProperty(applyTypeName);
                prop.SetValue(result, queryResult);
            }


            return result;
        }

        public ApplySupportCheckViewModel GetCheckViewModel(UserBriefModel userBrief, int diasterId)
        {
            var viewModel = new ApplySupportCheckViewModel();

            #region 取得 Counting 資料
            var cityId = userBrief.CityId;
            var repo = new ApplyBaseRepositroy<ApplyBaseModel>();
            var applySupports = repo.GetUnionApplySupportDatas(new ApplyBaseFilterParameter()
            {
                DiasterIds = new List<int>() { diasterId },
                CityIds = new List<int>() { cityId }
            });

            var towns = new TownService().GetByCityId(cityId)
                                         .OrderBy(c => c.Id);
            var viewModels = towns.Select(c => new ApplySupportCheckCountingViewModel()
            {
                TownId = c.Id,
                TownName = c.Name,
                Pending = applySupports.Count(k => k.TownId == c.Id && k.EPBConfirmStatus == ApplyStatusEnum.Pending),
                Processing = applySupports.Count(k => k.TownId == c.Id && k.EPBConfirmStatus == ApplyStatusEnum.Processing),
                SendToEpa = applySupports.Count(k => k.TownId == c.Id && k.EPBConfirmStatus == ApplyStatusEnum.SendToEpa),
                EpbConfrim = applySupports.Count(k => k.TownId == c.Id && k.EPBConfirmStatus == ApplyStatusEnum.Confrim),
                Reject = applySupports.Count(k => k.TownId == c.Id && k.EPBConfirmStatus == ApplyStatusEnum.Reject)
            });

            viewModel.CountingViewModels.AddRange(viewModels);
            #endregion



            return viewModel;
        }

        private static ApplyBaseFilterParameter GetApplyBaseFilter(ApplySupportReportFilterModel filterModel) 
        {
            var filter = new ApplyBaseFilterParameter()
            {
                DiasterIds = new List<int>() { filterModel.DiasterId },
            };

            if (filterModel.CityId.HasValue) 
            {
                filter.CityIds = new List<int>() { filterModel.CityId.Value };
            }
            return filter;
        }

        public ApplySupportReportModel GetApplyPeopleReport(ApplySupportReportFilterModel filter)
        {
            var result = new ApplySupportReportModel();
            result.AddDetail(ApplyPeopleRepository.GetApplySupportReportDetais(GetApplyBaseFilter(filter)));

            foreach (var detail in result.Details) 
            {
                detail.QuantityString = detail.Quantity.ToString();
            }

            result.TitleString = "人力支援";
            result.QuantityField = "數量(人/天)";
            result.TotalCountString = string.Format(TotalCountTemplate, result.Details.Count);
            result.TotalQuantityString = $"請求人力共{result.Details.Sum(c => c.Quantity)}人天";
            result.TotalCountToEpaString = string.Format(TotalCountToEpaTemplate, result.Details
                                                                                        .Count(c => c.Status == ApplyStatusEnum.SendToEpa));

            result.AbstractString = $@"●{result.TotalCountString} <br />
                                       ●{result.TotalQuantityString} <br />
                                       ●{result.TotalCountToEpaString} <br />";
            
            return result;
        }

        public List<ApplySupportProcessReport> ProcessReport(ApplySupportReportFilterModel filterModel)
        {
            ApplyTypeEnum applyType = (ApplyTypeEnum)filterModel.ApplyTypeId;
            if (applyType.IsDefinedValue() == false)
            {
                return new List<ApplySupportProcessReport>();
            }
            
            var typeString = applyType.ToString();
            var baseRepositroy = new ApplyBaseRepositroy<ApplyBaseModel>();
            var datas = baseRepositroy.GetProcessReportByFilter(typeString, GetApplyBaseFilter(filterModel));
            datas.ForEach((data) =>
            {
                data.ApplyType = typeString;
            });
            return datas; 
        }

        public ApplySupportReportModel GetApplyMedicineReport(ApplySupportReportFilterModel filter)
        {
            var result = new ApplySupportReportModel();
            result.AddDetail(ApplyMedicineRepository.GetApplySupportReportDetais(GetApplyBaseFilter(filter)));
            var totalQuantity = 0;
            if (result.Details.Any()) 
            {
                var details = ApplyMedicineRepository.GetDetailsByIds(result.Details
                                                                       .Select(c => c.Id)
                                                                       .ToList());
                foreach (var reportDetail in result.Details) 
                {
                    var id = reportDetail.Id;
                    var foundDetails = details.Where(c => c.ApplyMedicineId == id);
                    var quantityStrings = foundDetails.GroupBy(c => c.MedicineType)
                                                      .Select(c => $"{c.Key.GetDescription()} {c.Sum(y => y.Quantity)} 公升");
                    
                    reportDetail.QuantityString = string.Join("<br />", quantityStrings);
                }

                totalQuantity += details.Sum(c => c.Quantity);
            }
           


            result.TitleString = "消毒藥劑需求";
            result.QuantityField = "數量(公升)";
            result.TotalCountString = string.Format(TotalCountTemplate, result.Details.Count);
            result.TotalQuantityString = $"請求藥劑共{totalQuantity}公升";
            result.TotalCountToEpaString = string.Format(TotalCountToEpaTemplate, result.Details
                                                                                        .Count(c => c.Status == ApplyStatusEnum.SendToEpa));
            
            result.AbstractString = $@"●{result.TotalCountString} <br />
                                       ●{result.TotalQuantityString} <br />
                                       ●{result.TotalCountToEpaString} <br />";
            return result;
        }

        public ApplySupportReportModel GetApplySubsidyReport(ApplySupportReportFilterModel filter)
        {
            var result = new ApplySupportReportModel();
            result.AddDetail(ApplySubsidyRepository.GetApplySupportReportDetais(GetApplyBaseFilter(filter)));
            if (result.Details.Any())
            {
                var details = ApplySubsidyRepository.GetDetailsByIds(result.Details
                                                                           .Select(c => c.Id)
                                                                           .ToList());
                foreach (var reportDetail in result.Details)
                {
                    var id = reportDetail.Id;
                    var foundDetails = details.Where(c => c.ApplySubsidyId == id).FirstOrDefault();

                    if (foundDetails != null)
                    {
                        if (foundDetails.SubsidyType == ApplySubsidyTypeEnum.HireTemporaryWorkers
                                    || foundDetails.SubsidyType == ApplySubsidyTypeEnum.RentalCleaningEquipment
                                    || foundDetails.SubsidyType == ApplySubsidyTypeEnum.RentalDisinfectionEquipment)
                        {
                            reportDetail.Quantity = (int)(foundDetails.Quantity * foundDetails.Price * foundDetails.NeedDays);
                        }
                        else
                        {
                            reportDetail.Quantity = (int)(foundDetails.Quantity * foundDetails.Price);
                        }
                    }
                    reportDetail.QuantityString = reportDetail.Quantity.ToString();
                }
            }

            result.TitleString = "補助款需求";
            result.QuantityField = "金額(元)";
            result.TotalCountString = string.Format(TotalCountTemplate, result.Details.Count);
            result.TotalQuantityString = $"補助款共{result.Details.Sum(c => c.Quantity)}元";
            result.TotalCountToEpaString = string.Format(TotalCountToEpaTemplate, result.Details
                                                                                        .Count(c => c.Status == ApplyStatusEnum.SendToEpa));

            result.AbstractString = $@"●{result.TotalCountString} <br />
                                       ●{result.TotalQuantityString} <br />
                                       ●{result.TotalCountToEpaString} <br />";
            return result;
        }

        public ApplySupportReportModel GetApplyOtherReport(ApplySupportReportFilterModel filter)
        {
            var result = new ApplySupportReportModel();
            result.AddDetail(ApplyOtherRepository.GetApplySupportReportDetais(GetApplyBaseFilter(filter)));
            if (result.Details.Any())
            {
                var details = ApplyOtherRepository.GetDetailsByIds(result.Details
                                                                         .Select(c => c.Id)
                                                                         .ToList());
                foreach (var reportDetail in result.Details)
                {
                    var id = reportDetail.Id;
                    var foundDetails = details.Where(c => c.ApplyOtherId == id);
                    var quantityStrings = foundDetails.Select(c => $"{c.Item} {c.Quantity} {c.Unit}");

                    reportDetail.QuantityString = string.Join("<br />", quantityStrings);
                }

             
            }

            result.TitleString = "其它(包括垃圾場災損)需求";
            result.QuantityField = "數量";
            result.TotalCountString = string.Format(TotalCountTemplate, result.Details.Count);
            result.TotalCountToEpaString = string.Format(TotalCountToEpaTemplate, result.Details
                                                                                        .Count(c => c.Status == ApplyStatusEnum.SendToEpa));

            result.AbstractString = $@"●{result.TotalCountString} <br />
                                       ●{result.TotalCountToEpaString} <br />";
            return result;
        }

        public ApplySupportReportModel GetApplyCarReport(ApplySupportReportFilterModel filter)
        {
            var result = new ApplySupportReportModel();
            result.AddDetail(ApplyCarRepository.GetApplySupportReportDetais(GetApplyBaseFilter(filter)));
            var totalQuantity = 0;
            if (result.Details.Any())
            {
                var details = ApplyCarRepository.GetDetailsByIds(result.Details
                                                                  .Select(c => c.Id)
                                                                  .ToList());
                var carTypes = ApplyCarRepository.GetCarTypes();
                foreach (var reportDetail in result.Details)
                {
                    var id = reportDetail.Id;
                    var foundDetails = details.Where(c => c.ApplyCarId == id);
                    var quantityStrings = foundDetails.Select(c => $"{carTypes.FirstOrDefault(k=>k.Id == c.ApplyCarTypeId).DisplayName} {c.Quantity} 輛");

                    reportDetail.QuantityString = string.Join("<br />", quantityStrings);
                }

                totalQuantity += details.Sum(c => c.Quantity);
            }



            result.TitleString = "車輛設備需求";
            result.QuantityField = "數量(輛)";
            result.TotalCountString = string.Format(TotalCountTemplate, result.Details.Count);
            result.TotalQuantityString = $"請求車輛共{totalQuantity}輛";
            result.TotalCountToEpaString = string.Format(TotalCountToEpaTemplate, result.Details
                                                                                        .Count(c => c.Status == ApplyStatusEnum.SendToEpa));

            result.AbstractString = $@"●{result.TotalCountString} <br />
                                       ●{result.TotalQuantityString} <br />
                                       ●{result.TotalCountToEpaString} <br />";
            return result;
        }

        public ApplySupportReportModel GetApplyDisinfectionEquipmentReport(ApplySupportReportFilterModel filter)
        {
            var result = new ApplySupportReportModel();
            result.AddDetail(ApplyDisinfectionEquipmentRepository.GetApplySupportReportDetais(GetApplyBaseFilter(filter)));
            var totalQuantity = 0;
            if (result.Details.Any())
            {
                var details = ApplyDisinfectionEquipmentRepository.GetDetailsByIds(result.Details
                                                                                         .Select(c => c.Id)
                                                                                         .ToList());
                foreach (var reportDetail in result.Details)
                {
                    var id = reportDetail.Id;
                    var foundDetails = details.Where(c => c.ApplyDisinfectionEquipmentId == id);
                    var quantityStrings = foundDetails.Select(c => $"{c.Item} {c.Quantity} 個");

                    reportDetail.QuantityString = string.Join("<br />", quantityStrings);
                }

                totalQuantity += details.Sum(c => c.Quantity);
            }



            result.TitleString = "環境消毒設備需求";
            result.QuantityField = "數量(個)";
            result.TotalCountString = string.Format(TotalCountTemplate, result.Details.Count);
            result.TotalQuantityString = $"請求消毒設備共{totalQuantity}個";
            result.TotalCountToEpaString = string.Format(TotalCountToEpaTemplate, result.Details
                                                                                        .Count(c => c.Status == ApplyStatusEnum.SendToEpa));

            result.AbstractString = $@"●{result.TotalCountString} <br />
                                       ●{result.TotalQuantityString} <br />
                                       ●{result.TotalCountToEpaString} <br />";
            return result;
        }


        /// <summary>
        /// 取得統計報表
        /// </summary>
        /// <returns></returns>
        public static ApplySupportSubsidyReportViewModel GetSubsidyCountingReport(int diasterId) 
        {
            var result = new ApplySupportSubsidyReportViewModel();
            var baseRepository = new ApplyBaseRepositroy<ApplyBaseModel>();
            var ePBCountingList = baseRepository.GetEPBSubsidyReportCounting(diasterId);
            var ePACountingList = baseRepository.GetEPASubsidyReportCounting(diasterId);

            MappedResultEpbCounting(ref result, ePBCountingList);
            MappedResultEpaCounting(ref result, ePACountingList);

            return result;
        }

        private static void MappedResultEpbCounting(ref ApplySupportSubsidyReportViewModel result, List<ApplySupportSubsidyReportCountingViewModel> ePBCountingList)
        {
            foreach (ApplyTypeEnum applyType in Enum.GetValues(typeof(ApplyTypeEnum)))
            {
                var subsidyReportDetailViewModel = new ApplySupportSubsidyReportDetailViewModel() 
                {
                  ApplyType = applyType
                };


                var properties = subsidyReportDetailViewModel.GetType()
                                                             .GetProperties();
                foreach (ApplyStatusEnum applyStatus in Enum.GetValues(typeof(ApplyStatusEnum))) 
                {
                    var foundProperty = properties.FirstOrDefault(c => c.Name == applyStatus.ToString());
                    if (foundProperty != null) 
                    {
                        var foundValue = ePBCountingList.FirstOrDefault(c => c.ApplyType == applyType.ToString() && c.ConfirmStatus == applyStatus)?.Count;
                        if (foundValue.HasValue)
                        {
                            foundProperty.SetValue(subsidyReportDetailViewModel, foundValue.Value);
                        }
                        else 
                        {
                            foundProperty.SetValue(subsidyReportDetailViewModel, 0);
                        }
                    }
                }
                result.EPBCountingReport.Add(subsidyReportDetailViewModel);
            }
        }

        private static void MappedResultEpaCounting(ref ApplySupportSubsidyReportViewModel result, List<ApplySupportSubsidyReportCountingViewModel> ePACountingList)
        {
            foreach (ApplyTypeEnum applyType in Enum.GetValues(typeof(ApplyTypeEnum)))
            {
                var subsidyReportDetailViewModel = new ApplySupportSubsidyReportDetailViewModel()
                {
                    ApplyType = applyType
                };


                var properties = subsidyReportDetailViewModel.GetType()
                                                             .GetProperties();
                foreach (ApplyStatusEnum applyStatus in Enum.GetValues(typeof(ApplyStatusEnum)))
                {
                    var foundProperty = properties.FirstOrDefault(c => c.Name == applyStatus.ToString());
                    if (foundProperty != null)
                    {
                        var foundValue = ePACountingList.FirstOrDefault(c => c.ApplyType == applyType.ToString() && c.ConfirmStatus == applyStatus)?.Count;
                        if (foundValue.HasValue)
                        {
                            foundProperty.SetValue(subsidyReportDetailViewModel, foundValue.Value);
                        }
                        else
                        {
                            foundProperty.SetValue(subsidyReportDetailViewModel, 0);
                        }
                    }
                }
                result.EPACountingReport.Add(subsidyReportDetailViewModel);
            }
        }
    }
}