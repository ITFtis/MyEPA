using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Services
{
    public class WaterDivisionService
    {
        WaterDivisionRepository WaterDivisionRepository = new WaterDivisionRepository();
        public List<WaterDivisionModel> GetWaterDivisions()
        {
            return WaterDivisionRepository.GetList();
        }
    }

    public class WaterCheckDetailService
    {
        WaterCheckDetailRepository WaterCheckDetailRepository = new WaterCheckDetailRepository();
        WaterCheckService WaterCheckService = new WaterCheckService();
        CityRepository CityRepository = new CityRepository();
        TownRepository TownRepository = new TownRepository();
       
        public List<WaterCheckDetailModel> GetListByWaterCheckId(UserBriefModel user ,WaterCheckModel model)
        {
            WaterCheckModel waterCheck = WaterCheckService.GetByFilter(new WaterCheckFilterParameter 
            {
                DiasterIds = model.DiasterId.ToListCollection(),
                CheckDate = model.CheckDate,
                Types = user.Duty == DutyEnum.Water ? WaterCheckTypeEnum.Water.ToListCollection() : WaterCheckTypeEnum.EPPersonnel.ToListCollection()
            }).FirstOrDefault();

            if (waterCheck == null)
            {
                waterCheck = WaterCheckService.CreateAndReturn(user,model);
            }
            waterCheck.CopyTo(model);

            var wds = CityRepository.GetWaterDivisions();

            List<int> cityIds = new List<int>();

            if (user.Duty == DutyEnum.Water)
            {
                cityIds = wds.Where(e => e.WaterDivision == user.Town)
               .Select(e => e.CityId).ToList();
            }
            else
            {
                cityIds.Add(user.CityId);
            }

            var result = WaterCheckDetailRepository.GetByFilter(new WaterCheckDetailFilterParameter
            {
                WaterCheckIds = waterCheck.Id.ToListCollection(),
                CityIds = cityIds
            });
            return result;

            
        }

        ////Brian：2024/04/08 => 改用DiasterId(查核ID)，跨縣市查詢
        public List<WaterCheckDetailModel> GetListByWaterCheckId2(UserBriefModel user, int waterCheckId)
        {
            var result = WaterCheckDetailRepository.GetByFilter(new WaterCheckDetailFilterParameter
            {
                WaterCheckIds = waterCheckId.ToListCollection(),
                UpdateUsers = new List<string>() { user.UserName }
            });
            return result;
        }

        public List<WaterCheckDetailModel> GetListByWaterCheckId(UserBriefModel user, int waterCheckId)
        {
            var wds = CityRepository.GetWaterDivisions();

            List<int> cityIds =
                wds.Where(e => e.WaterDivision == user.Town)
                .Select(e => e.CityId).ToList();

            var result = WaterCheckDetailRepository.GetByFilter(new WaterCheckDetailFilterParameter
            {
                WaterCheckIds = waterCheckId.ToListCollection(),
                CityIds = cityIds
            });
            return result;
        }
        public WaterCheckDetailModel GetById(int id)
        {
            WaterCheckDetailModel detail = WaterCheckDetailRepository.Get(id);
            if (detail == null)
            {
                return detail;
            }
            return detail;
        }
        public void Update(UserBriefModel user, WaterCheckDetailModel model)
        {
            WaterCheckDetailModel detail = WaterCheckDetailRepository.Get(model.Id);

            if(detail == null)
            {
                return;
            }
            model.Address = model.Address ?? string.Empty;
            CityModel city = CityRepository.Get(model.CityId);
            TownModel town = TownRepository.Get(model.TownId);
            model.CityId = city.Id;
            model.CityName = city.City;
            model.TownId = town.Id;
            model.TownName = town.Name;
            model.UpdateUser = user.UserName;
            model.CheckTime = DateTimeHelper.GetCurrentTime();
            model.Recheck += 1;
            model.Status = GetDetailStatus(model);
            WaterCheckDetailRepository.Update(model);
        }
        public WaterCheckDetailStatusEnum GetDetailStatus(WaterCheckDetailModel model)
        {
            //檢驗中
            if ((EColiTypeEnum)model.EColiType == EColiTypeEnum.Testing)
            {
                return WaterCheckDetailStatusEnum.Testing;
            }
            //大腸桿菌檢查
            bool isEColiOK = IsEColiOK(model.EColiType, model.EColiStand, model.EColi);
            if(isEColiOK == false)
            {
                return WaterCheckDetailStatusEnum.Failed;
            }
            //PH值檢查
            bool isHydrogenOK = IsStandOK(model.HydrogenStand, model.Hydrogen);
            if (isHydrogenOK == false)
            {
                return WaterCheckDetailStatusEnum.Failed;
            }
            //餘氯檢查 
            bool isChlorineOK = IsStandOK(model.ChlorineStand, model.Chlorine);
            if (isChlorineOK == false)
            {
                return WaterCheckDetailStatusEnum.Failed;
            }
            //濁度檢查 
            bool isTurbidityOK = IsStandOK(model.TurbidityStand, model.Turbidity);
            if (isTurbidityOK == false)
            {
                return WaterCheckDetailStatusEnum.Failed;
            }
            return WaterCheckDetailStatusEnum.Success;
        }
        private bool IsEColiOK(int ecoliType,int ecoliStand,decimal? v)
        {
            decimal ecoli = v ?? 0;

            EColiTypeEnum type = (EColiTypeEnum)ecoliType;
            switch(type)
            {
                //未檢出/不檢驗/<1
                case EColiTypeEnum.LessThan1:
                case EColiTypeEnum.Untested:
                case EColiTypeEnum.Zero:
                    return true;

                case EColiTypeEnum.Other:
                case EColiTypeEnum.Testing:
                case EColiTypeEnum.TNTC:
                    return IsStandOK(ecoliStand, ecoli);
                default:
                    throw new NotImplementedException();
            }
        }
        private bool IsStandOK(int stand, decimal? v)
        {
            decimal value = v ?? 0;

            return IsStandOK((WaterCheckDetailStandEnum)stand, value);
        }
        private bool IsStandOK(WaterCheckDetailStandEnum stand, decimal value)
        {
            switch (stand)
            {
                case WaterCheckDetailStandEnum.ChlorineOption001:
                    {
                        return value >= 0.2M && value <= 1.0M ;
                    }
                case WaterCheckDetailStandEnum.ChlorineOption011:
                    {
                        return value >= 0.2M && value <= 2.0M;
                    }
                case WaterCheckDetailStandEnum.EColiOption001:
                case WaterCheckDetailStandEnum.EColiOption011:
                    {
                        return value <= 6;
                    }
                case WaterCheckDetailStandEnum.HydrogenOption001:
                    {
                        return value >= 6.0M && value <= 8.5M;
                    }
                case WaterCheckDetailStandEnum.TurbidityOption002:
                case WaterCheckDetailStandEnum.TurbidityOption004:
                case WaterCheckDetailStandEnum.TurbidityOption010:
                case WaterCheckDetailStandEnum.TurbidityOption030:
                    {
                        return stand.GetDescription().TryToInt().Value >= value;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }
        public void Create(UserBriefModel user, WaterCheckDetailModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Address))
            {
                model.Address = string.Empty;
            }
            CityModel city = CityRepository.Get(model.CityId);
            TownModel town = TownRepository.Get(model.TownId);
            model.CityId = city.Id;
            model.CityName = city.City;
            model.TownId = town.Id;
            model.TownName = town.Name;
            model.UpdateUser = user.UserName;
            model.CheckTime = DateTimeHelper.GetCurrentTime();
            model.Recheck = 0;
            model.Status = GetDetailStatus(model);
            WaterCheckDetailRepository.Create(model);
        }
        public List<WaterCheckDetailViewModel> GetListByDiasterId(int diasterId,int? waterDivisionId, UserBriefModel user)
        {
            List<int> cityIds = null;
            if(waterDivisionId.HasValue)
            {
                cityIds = CityRepository.GetListByFilter(new CityFilterParameter 
                { 
                    WaterDivisionIds = waterDivisionId.Value.ToListCollection()
                }).Select(e=>e.Id).ToList();
            }
            List<WaterCheckModel> waterChecks = 
                WaterCheckService.GetByFilter(new WaterCheckFilterParameter 
                {
                    DiasterIds = diasterId.ToListCollection(),
                    Types = WaterCheckTypeEnum.Water.ToListCollection()
                });

            if(waterChecks.IsEmptyOrNull())
            {
                return new List<WaterCheckDetailViewModel>();
            }
            
            List<WaterCheckDetailModel> details = WaterCheckDetailRepository.GetByFilter(new WaterCheckDetailFilterParameter
            {
                WaterCheckIds = waterChecks.Select(e => e.Id).ToList(),
                CityIds = cityIds
                
            });

            var waterDivisions = CityRepository.GetWaterDivisions().ToDictionary(e => e.CityId, e => e.WaterDivision);

            Action<WaterCheckDetailModel,WaterCheckDetailViewModel> action = (f,g) => 
            {
                g.WaterDivision = waterDivisions.GetValue(f.CityId);
            };

            return details.Select(e => e.ConvertToModel(action)).ToList();
        }
        public AdminResultModel Delete(int id)
        {
            var isSuccess = WaterCheckDetailRepository.Delete(id);
            return new AdminResultModel
            {
                IsSuccess = isSuccess
            };
        }
    }
}