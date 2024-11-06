using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Services
{
    public class OpenContractDetailService
    {
        OpenContractDetailRepository OpenContractDetailRepository = new OpenContractDetailRepository();
        OpenContractDetailItemCategoryRepository OpenContractDetailItemCategoryRepository = new OpenContractDetailItemCategoryRepository();
        public List<OpenContractDetailViewModel> GetList(int openContractId)
        {
            var details = OpenContractDetailRepository.GetListByOpenContractId(openContractId);

            var itemCategories = 
                OpenContractDetailItemCategoryRepository.GetList()
                .ToDictionary(e => e.Id, e => e.Category);

            return details.Select(e => new OpenContractDetailViewModel 
            {
                Budge = e.Budge,
                Count = e.Count,
                Id = e.Id,
                ItemCategory = e.ItemCategoryId.HasValue ? itemCategories.GetValue(e.ItemCategoryId.Value) : null,
                ItemCategoryId = e.ItemCategoryId,
                Items = e.Items,
                OpenContractId = e.OpenContractId,
                Price = e.Price,
                Status = e.Status,
                Unit = e.Unit
            }).ToList();
        }

        public OpenContractDetailModel Get(int id)
        {
            return OpenContractDetailRepository.Get(id);
        }

        public void Create(string user, OpenContractDetailModel model)
        {
            model.CreateDate = DateTimeHelper.GetCurrentTime();
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            model.CreateUser = user;
            model.UpdateUser = user;
            OpenContractDetailRepository.Create(model);
        }

        public AdminResultModel Delete(int id)
        {
            bool isSuccess = OpenContractDetailRepository.Delete(id);
            return new AdminResultModel
            {
                IsSuccess = isSuccess
            };
        }

        public void Update(string user, OpenContractDetailModel model)
        {
            var entity = OpenContractDetailRepository.Get(model.Id);

            if (entity == null)
                return;

            entity.Items = model.Items;
            entity.Unit = model.Unit;
            entity.Count = model.Count;
            entity.Price = model.Price;
            entity.Budge = model.Budge;
            entity.ItemCategoryId = model.ItemCategoryId;

            entity.UpdateDate = DateTimeHelper.GetCurrentTime();
            entity.UpdateUser = user;

            //舊資料錯誤(CreateDate Null)
            if (entity.CreateDate == DateTime.MinValue)
                entity.CreateDate = entity.UpdateDate;

            OpenContractDetailRepository.Update(entity);
        }
    }
}