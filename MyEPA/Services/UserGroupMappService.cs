using MyEPA.Enums;
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
    public class UserGroupMappService
    {
        UserGroupMappRepository UserGroupMappRepository = new UserGroupMappRepository();
        UsersRepository UsersRepository = new UsersRepository();
        SendMessageService SendMessageService = new SendMessageService();
        public List<UserGroupMappBriefModel> GetBriefAll()
        {
            return UserGroupMappRepository.GetBriefAll();
        }

        public void Create(UserBriefModel user, UserGroupMappBriefModel model)
        {
            DateTime now = DateTimeHelper.GetCurrentTime();

            UserGroupMappModel insert = new UserGroupMappModel 
            {
                CreateDate = now,
                CreateUser = user.UserName,
                UpdateDate = now,
                UpdateUser = user.UserName,
                GroupId = model.GroupId,
                UserId = model.UserId
            };

            UserGroupMappRepository.Create(insert);
        }

        public AdminResultModel Delete(int userId,int groupId)
        {
            var entity = UserGroupMappRepository.Get(userId, groupId);

            if (entity == null)
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "資料不存在"
                };
            try
            {
                UserGroupMappRepository.Delete(entity.Id);
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

        public void SendGroupSMS(SendGroupSMSViewModel model)
        {
            List<int> userIds = UserGroupMappRepository.GetBriefListByFilter(new UserGroupMappFilterParameter
            {
                GroupIds = model.GroupId.ToListCollection()
            }).Select(e => e.UserId).ToList();

            IEnumerable<string> mobiles = UsersRepository.GetListBriefByFilter(new UsersBriefFilterParameter
            {
                UserIds = userIds
            }).Select(e => e.MobilePhone);

            SendMessageService.SendMessageByMobiles(mobiles, model.Subject, model.Message);
        }
    }
}