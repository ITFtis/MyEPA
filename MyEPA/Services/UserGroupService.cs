using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;

namespace MyEPA.Services
{
    public class UserGroupService
    {
        UserGroupRepository UserGroupRepository = new UserGroupRepository();
        public List<UserGroupModel> GetListByType(UserGroupTypeEnum type)
        {
            return UserGroupRepository.GetListByFilter(new UserGroupFilterParameter 
            {
                Types = type.ToInteger().ToListCollection()
            });
        }

        public void Create(UserBriefModel user, UserGroupModel model)
        {
            DateTime now = DateTimeHelper.GetCurrentTime();

            model.CreateDate = now;
            model.UpdateDate = now;
            model.CreateUser = user.UserName;
            model.UpdateUser = user.UserName;

            UserGroupRepository.Create(model);
        }

        public AdminResultModel Delete(int id)
        {
            var entity = UserGroupRepository.Get(id);

            if (entity == null)
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "資料不存在"
                };
            try
            {
                UserGroupRepository.Delete(id);
            }
            catch (Exception ex)
            {
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
            return new AdminResultModel
            {
                IsSuccess = true
            };
        }

        public void Update(UserBriefModel user, UserGroupModel model)
        {
            var entity = UserGroupRepository.Get(model.Id);
            if (entity == null)
            {
                return;
            }
            DateTime now = DateTimeHelper.GetCurrentTime();
            model.UpdateDate = now;
            model.UpdateUser = user.UserName;
            entity.GroupName = model.GroupName;
            UserGroupRepository.Update(entity);
        }

        public UserGroupModel Get(int id)
        {
            var entity = UserGroupRepository.Get(id);
            return entity;
        }
    }
}