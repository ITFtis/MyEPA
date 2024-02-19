using MyEPA.Extensions;
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
    public class IncineratorService
    {
        IncineratorRepository IncineratorRepository = new IncineratorRepository();
        public List<IncineratorModel> GetList()
        {
            return IncineratorRepository.GetList();
        }

        public IncineratorModel Get(int id)
        {
            return IncineratorRepository.Get(id);
        }

        public AdminResultModel Update(IncineratorModel model)
        {
            var entity = IncineratorRepository.Get(model.Id);
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
            entity.Town = model.Town;
            entity.UpdateTime = DateTimeHelper.GetCurrentTime();
            entity.Xpos = model.Xpos;
            entity.Ypos = model.Ypos;

            try
            {
                IncineratorRepository.Update(entity);
            }
            catch
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
            List<IncineratorModel> dumps = IncineratorRepository.GetByFilter(new IncineratorFilterParameter
            {
                City = user.City,
                Town = user.Duty == Enums.DutyEnum.Cleaning ? user.Town : string.Empty
            });
            foreach (var item in dumps)
            {
                item.ConfirmTime = DateTimeHelper.GetCurrentTime();
            }
            IncineratorRepository.Update(dumps);
            return new AdminResultModel
            {
                IsSuccess = true
            };
        }

        public List<IncineratorModel> GetByFilter(IncineratorFilterParameter filter)
        {
            return IncineratorRepository.GetByFilter(filter);
        }
    }
}