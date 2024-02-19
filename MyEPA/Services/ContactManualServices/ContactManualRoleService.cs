using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System.Collections.Generic;

namespace MyEPA.Services
{
    public class ContactManualRoleService
    {
        ContactManualRoleRepository ContactManualRoleRepository = new ContactManualRoleRepository();
        public List<ContactManualRoleModel> GetByType(ContactManualRoleTypeEnum? type)
        {
            return ContactManualRoleRepository.GetByFilter(new ContactManualRoleFilterParameter
            {
                Types = type?.ToListCollection()
            });
        }

        public List<ContactManualRoleModel> GetByIds(List<int> ids)
        {
            return ContactManualRoleRepository.GetByFilter(new ContactManualRoleFilterParameter
            {
                Ids = ids
            });
        }
        public ContactManualRoleViewModel Get(int id)
        {
            var model = ContactManualRoleRepository.Get(id);
            return new ContactManualRoleViewModel 
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public void Update(UserBriefModel user,ContactManualRoleViewModel model)
        {
            var entity = ContactManualRoleRepository.Get(model.Id);
            
            entity.Name = model.Name;

            ContactManualRoleRepository.Update(user, entity);
        }

        public void Create(UserBriefModel user, ContactManualRoleViewModel model)
        {
            var entity = new ContactManualRoleModel
            {
                Name = model.Name,
                Type = model.Type
            };
            ContactManualRoleRepository.Create(user, entity);
        }
    }
}