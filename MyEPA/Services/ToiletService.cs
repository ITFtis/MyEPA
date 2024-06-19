using MyEPA.Enums;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MyEPA.Services
{
    public class ToiletService
    {
        ToiletRepository ToiletRepository = new ToiletRepository();
        public ToiletModel GetById(int id)
        {
            return ToiletRepository.Get(id);
        }
        public void Create(ToiletModel model)
        {
            model.UpdateTime = DateTimeHelper.GetCurrentTime();
            model.ConfirmTime = model.UpdateTime;
            ToiletRepository.Create(model);
        }
        public void Delete(int id)
        {
            ToiletRepository.Delete(id);
        }
        public List<ToiletModel> GetByFilter(ToiletFilterParameter filter)
        {
            return ToiletRepository.GetByFilter(filter);
        }
        public List<ToiletReportModel> GetReportByFilter(ToiletFilterParameter filter)
        {
            return ToiletRepository.GetReportByFilter(filter);
        }
        public void Update(ToiletEditViewModel model, UserBriefModel user)
        {
            var entity = ToiletRepository.Get(model.EditId);
            //找不到資料暫時不處理，內部系統比較不會發生
            if (entity == null)
            {
                return ;
            }
            entity.Amount = model.EditAmount;
            entity.ROCyear = model.EditROCyear;
            entity.SlotNumber = model.EditSlotNumber;
            entity.ToiletType = model.EditToiletType;
            entity.Unit = model.EditUnit;
            entity.UpdateTime = DateTimeHelper.GetCurrentTime();
            entity.UpdateUser = user.UserName;
            entity.ConfirmTime = entity.UpdateTime;
            ToiletRepository.Update(entity);
        }
        public void Confirm(UserBriefModel user, int? townId)
        {
            List<int> cityIds = new List<int>();
            List<int> townIds = new List<int>();
            
            switch (user.Duty)
            {
                case DutyEnum.Cleaning:
                    cityIds.Add(user.CityId);
                    townIds.Add(user.TownId);
                    break;
                case DutyEnum.Water:
                    //不執行
                    return;
                case DutyEnum.Team:
                case DutyEnum.Corps:
                case DutyEnum.EPA:
                    break;
                case DutyEnum.EPB:
                    cityIds.Add(user.CityId);
                    if(townId.HasValue)
                    {
                        townIds.Add(townId.Value);
                    }
                    break;

            }
            List<ToiletModel> toilets =
                ToiletRepository.GetByFilter(new ToiletFilterParameter
                {
                    CityIds = cityIds,
                    TownIds = townIds
                });
            foreach (var item in toilets)
            {
                item.ConfirmTime = DateTimeHelper.GetCurrentTime();
                item.UpdateUser = user.UserName;
            }
            ToiletRepository.Update(toilets);
        }
    }
}