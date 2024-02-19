using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Services
{
    public class LandfillService
    {
        LandfillRepository LandfillRepository = new LandfillRepository();
        public List<LandfillModel> GetByFilter(LandfillFilterParameter filter)
        {
            return LandfillRepository.GetByFilter(filter);
        }

        public LandfillModel Get(int id)
        {
            return LandfillRepository.Get(id);
        }

        public AdminResultModel Update(LandfillModel model)
        {
            var entity = LandfillRepository.Get(model.Id);
            if(entity == null)
            {
                return new AdminResultModel() 
                {
                    IsSuccess = false,
                    ErrorMessage = "查無資料"
                };
            }
            entity.Address = model.Address;
            entity.City = model.City;
            entity.ContactPerson = model.ContactPerson;
            entity.ContactPersonTitle = model.ContactPersonTitle;
            entity.ContactPhone = model.ContactPhone;
            entity.ContactUnit = model.ContactUnit;
            entity.DesignCapacity = model.DesignCapacity;
            entity.ResidualCapacity = model.ResidualCapacity;
            entity.Town = model.Town;
            entity.UpdateTime = DateTimeHelper.GetCurrentTime();
            entity.Xpos = model.Xpos;
            entity.Ypos = model.Ypos;

            try
            {
                LandfillRepository.Update(entity);
            }
            catch(Exception ex)
            {
                return new AdminResultModel 
                {
                    ErrorMessage = "更新失敗",
                    IsSuccess = false
                };
            }

            return new AdminResultModel 
            {
                IsSuccess = true
            };
        }

        public AdminResultModel Confirm(UserBriefModel user)
        {
            List<LandfillModel> dumps = LandfillRepository.GetByFilter(new LandfillFilterParameter
            {
                City = user.City,
                Town = user.Duty == Enums.DutyEnum.Cleaning ? user.Town : string.Empty
            });
            foreach (var item in dumps)
            {
                item.ConfirmTime = DateTimeHelper.GetCurrentTime();
            }
            LandfillRepository.Update(dumps);
            return new AdminResultModel
            {
                IsSuccess = true
            };
        }
    }
}