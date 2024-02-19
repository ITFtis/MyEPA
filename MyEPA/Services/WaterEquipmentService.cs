using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using MyEPA.Extensions;
using System.Linq;

namespace MyEPA.Services
{
    public class WaterEquipmentService
    {
        WaterEquipmentRepository WaterEquipmentRepository = new WaterEquipmentRepository();

        public List<WaterEquipmentViewModel> GetByDiasterId(int diasterId)
        {
            var models = WaterEquipmentRepository.GetByFilter(new WaterEquipmentFilterParameter 
            {
                DiasterIds = diasterId.ToListCollection()
            });
            return models;
        }

        public void Create(UserBriefModel user, WaterEquipmentModel model)
        {
            model.CityId = user.CityId;
            model.TownId = user.TownId;
            model.CreateDate = DateTimeHelper.GetCurrentTime();
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            model.CreateUser = user.UserName;
            model.UpdateUser = user.UserName;
            WaterEquipmentRepository.Create(model);
        }

        public void Update(UserBriefModel user, WaterEquipmentModel model)
        {
            WaterEquipmentModel entity = WaterEquipmentRepository.Get(model.Id);

            if(entity == null)
            {
                return;
            }

            entity.AbnormalAmount = model.AbnormalAmount;
            entity.AbnormalArea = model.AbnormalArea;
            entity.AbnormalCount = model.AbnormalCount;
            entity.Description = model.Description;
            entity.DoneDate = model.DoneDate;
            entity.Name = model.Name;
            entity.NormalAmount = model.NormalAmount;
            entity.NormalArea = model.NormalArea;
            entity.NormalCount = model.NormalCount;
            entity.Remark = model.Remark;
            entity.UpdateDate = DateTimeHelper.GetCurrentTime();
            entity.UpdateUser = user.UserName;

            WaterEquipmentRepository.Update(entity);
        }

        public AdminResultModel Delete(int id)
        {
            WaterEquipmentRepository.Delete(id);
            return new AdminResultModel { IsSuccess = true };
        }

        public WaterEquipmentModel Get(int id)
        {
            return WaterEquipmentRepository.Get(id);
        }
    }
}