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
    public class PolymerService
    {
        PolymerRepository PolymerRepository = new PolymerRepository();
        PolymerDetailRepository PolymerDetailRepository = new PolymerDetailRepository();

        public bool IsExists(int polymerId)
        {
            return PolymerRepository.IsExistsByFilter(new PolymerFilterParameter 
            {
                PolymerIds = polymerId.ToListCollection()
            });
        }
        public List<PolymerViewModel> GetByDiasterId(int diasterId)
        {
            var models = PolymerRepository.GetByFilter(new PolymerFilterParameter 
            {
                DiasterIds = diasterId.ToListCollection()
            });

            Dictionary<int,int> details =
                PolymerDetailRepository.GetByFilter(new PolymerDetailFilterParameter
                {
                    PolymerIds = models.Select(e => e.Id).ToList()
                }).GroupBy(e=>e.PolymerId).ToDictionary(e=>e.Key,e=>e.Count());

            return models.Select(e => new PolymerViewModel
            {
                CreateDate = e.CreateDate,
                Department = e.Department,
                DrugName = e.DrugName,
                UseDays = details.GetValue(e.Id),
                Id = e.Id
            }).ToList();
        }

        public void Create(UserBriefModel user, PolymerModel model)
        {
            model.CreateDate = DateTimeHelper.GetCurrentTime();
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            model.CreateUser = user.UserName;
            model.UpdateUser = user.UserName;
            PolymerRepository.Create(model);
        }

        public void Update(UserBriefModel user, PolymerModel model)
        {
            PolymerModel entity = PolymerRepository.Get(model.Id);

            if(entity == null)
            {
                return;
            }
            
            entity.Address = model.Address;
            entity.Amount = model.Amount;
            entity.BuyDate = model.BuyDate;
            entity.ChemicalFormula = model.ChemicalFormula;
            entity.Department = model.Department;
            entity.DrugName = model.DrugName;
            entity.EffectiveDate = model.EffectiveDate;
            entity.MainContacter = model.MainContacter;
            entity.Phone = model.Phone;
            entity.Vendor = model.Vendor;
            entity.UpdateDate = DateTimeHelper.GetCurrentTime();
            entity.UpdateUser = user.UserName;

            PolymerRepository.Update(entity);
        }

        public AdminResultModel Delete(int id)
        {
            PolymerRepository.Delete(id);
            PolymerDetailRepository.DeleteByFilter(new PolymerDetailFilterParameter
            {
                PolymerIds = id.ToListCollection()
            });
            return new AdminResultModel { IsSuccess = true };
        }

        public PolymerModel Get(int id)
        {
            return PolymerRepository.Get(id);
        }
    }
}