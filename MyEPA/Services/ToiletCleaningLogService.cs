using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Services
{
    public class ToiletCleaningLogService
    {
        ToiletCleaningLogRepository ToiletCleaningLogRepository = new ToiletCleaningLogRepository();
        
        public List<ToiletCleaningLogModel> GetByFilter(ToiletCleaningLogFilterParameter filter)
        {
            return ToiletCleaningLogRepository.GetByFilter(filter);
        }

        public ToiletCleaningLogModel Get(int id)
        {
            return ToiletCleaningLogRepository.Get(id);
        }

        public AdminResultModel Delete(int id)
        {
            ToiletCleaningLogRepository.Delete(id);
            return new AdminResultModel { IsSuccess = true };
        }

        public AdminResultModel CreateOrUpdate(ToiletCleaningLogModel model)
        {
            ToiletCleaningLogModel entity = ToiletCleaningLogRepository.Get(model.Id);

            if (entity == null)
            {
                entity = new ToiletCleaningLogModel
                {
                    CreateDate = DateTimeHelper.GetCurrentTime(),
                    CreateUser = model.UpdateUser,
                    ToiletLocationId = model.ToiletLocationId,
                };
            }

            entity.Date = model.Date;
            entity.Description = model.Description;
            entity.IsClean = model.IsClean;
            entity.UpdateDate = DateTimeHelper.GetCurrentTime();
            entity.UpdateUser = model.UpdateUser;

            ToiletCleaningLogRepository.CreateOrUpdate(entity);
            return new AdminResultModel 
            {
                IsSuccess = true
            };
        }
    }
}