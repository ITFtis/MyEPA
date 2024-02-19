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
    public class PolymerDetailService
    {
        PolymerDetailRepository PolymerDetailRepository = new PolymerDetailRepository();
       
        public void Create(UserBriefModel user, PolymerDetailModel model)
        {
            model.CreateDate = DateTimeHelper.GetCurrentTime();
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            model.CreateUser = user.UserName;
            model.UpdateUser = user.UserName;
            PolymerDetailRepository.Create(model);
        }

        public List<PolymerDetailModel> GetByPolymerId(int polymerId)
        {
            return PolymerDetailRepository.GetByFilter(new PolymerDetailFilterParameter 
            {
                PolymerIds = polymerId.ToListCollection()
            });
        }

        public void Update(UserBriefModel user, PolymerDetailModel model)
        {
            PolymerDetailModel entity = PolymerDetailRepository.Get(model.Id);

            if(entity == null)
            {
                return;
            }
            entity.DrugAmount = model.DrugAmount;
            entity.Inventory = model.Inventory;
            entity.OriginalInventory = model.OriginalInventory;
            entity.Turbidity = model.Turbidity;
            entity.WaterAmount = model.WaterAmount;
            entity.UseDate = model.UseDate;
            entity.UseAmount = model.UseAmount;
            entity.UpdateDate = DateTimeHelper.GetCurrentTime();
            entity.UpdateUser = user.UserName;

            PolymerDetailRepository.Update(entity);
        }

        public PolymerDetailModel Get(int id)
        {
            return PolymerDetailRepository.Get(id);
        }

        public AdminResultModel Delete(int id)
        {
            PolymerDetailRepository.Delete(id);
            return new AdminResultModel
            {
                IsSuccess = true
            };
        }
    }
}