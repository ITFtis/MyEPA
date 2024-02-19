using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;

namespace MyEPA.Services
{
    public class NoticeService
    {
        NoticeRepository NoticeRepository = new NoticeRepository();
        public List<NoticeModel> GetAll()
        {
            return NoticeRepository.GetList();
        }

        public NoticeModel Get(int id)
        {
            return NoticeRepository.Get(id);
        }

        public List<NoticeModel> GetByDiasterId(int diasterId)
        {
            return NoticeRepository.GetByFilter(new NoticeFilterParameter
            {
                DiasterIds = diasterId.ToListCollection()
            });
        }
        public List<NoticeModel> GetByFilter(NoticeFilterParameter filter)
        {
            return NoticeRepository.GetByFilter(filter);
        }

        public List<NoticeModel> GetByTop(int top)
        {
            return NoticeRepository.GetPageing(new PaginationModel
            {
                Order = SortDirectionEnum.DESC,
                Page = 1,
                PerPage = 5,
                SortBy = nameof(NewsModel.Id)
            }).Items;
        }

        public void Create(UserBriefModel user, NoticeModel model)
        {
            model.CreateDate = DateTimeHelper.GetCurrentTime();
            model.CreateUser = user.UserName;
            model.UpdateDate = DateTimeHelper.GetCurrentTime();
            model.UpdateUser = user.UserName;
            NoticeRepository.Create(model);
        }

        public void Update(UserBriefModel user, NoticeModel model)
        {
            var entity = NoticeRepository.Get(model.Id);
            if (entity == null)
            {
                return;
            }
            entity.Title = model.Title;
            entity.Content = model.Content;
            entity.DiasterId = model.DiasterId;
            entity.UpdateDate = DateTimeHelper.GetCurrentTime();
            entity.UpdateUser = user.UserName;
            NoticeRepository.Update(entity);
        }

        public AdminResultModel Delete(int id)
        {
            var isSuccess = NoticeRepository.Delete(id);
            return new AdminResultModel
            {
                IsSuccess = isSuccess
            };
        }
    }  
}