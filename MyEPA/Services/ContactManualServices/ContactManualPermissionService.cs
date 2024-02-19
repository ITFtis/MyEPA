using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;

namespace MyEPA.Services
{
    public class ContactManualPermissionService
    {
        UsersRepository UsersRepository = new UsersRepository();
        public List<ContactManualPermissionViewModel> GetListByDuty(ContactManualDutyEnum duty)
        {
            List<UsersBriefModel> models = UsersRepository.GetListBriefByFilter(new UsersBriefFilterParameter
            {
                ContactManualDutys = duty.ToInteger().ToListCollection()
            });

            return models.ConvertToModel<UsersBriefModel, ContactManualPermissionViewModel>((input, output) => output.UserId = input.Id);
        }

        public void Create(UserBriefModel user, ContactManualPermissionViewModel model)
        {
            var entity = UsersRepository.Get(model.UserId);
            entity.ContactManualDuty = model.ContactManualDuty;
            UsersRepository.Update(entity);
        }
        public void Delete(UserBriefModel user, int id)
        {
            var entity = UsersRepository.Get(id);
            entity.ContactManualDuty = ContactManualDutyEnum.None;
            UsersRepository.Update(entity);
        }
    }
}