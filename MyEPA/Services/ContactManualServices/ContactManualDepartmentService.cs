using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Services
{
    public class ContactManualDepartmentService
    {
        ContactManualDepartmentRepository ContactManualDepartmentRepository = new ContactManualDepartmentRepository();
        public List<ContactManualDepartmentModel> GetByType(ContactManualDepartmentTypeEnum? type)
        {
            return ContactManualDepartmentRepository.GetListByFilter(new ContactManualDepartmentParameter 
            {
                Types = type.HasValue ? ((int)type).ToListCollection() : new List<int>()
            });
        }

        public IEnumerable<ContactManualDepartmentModel> GetContactManualDepartments(UserBriefModel user, ContactManualDepartmentTypeEnum? type = null)
        {
            var contactManualDepartment = GetByType(type).AsEnumerable();
            if (user.ContactManualDuty != ContactManualDutyEnum.Administrator)
            {
                contactManualDepartment = contactManualDepartment.Where(e => e.Id == user.ContactManualDepartmentId);
            }
            return contactManualDepartment;
        }


        public List<ContactManualDepartmentModel> GetByIds(List<int> ids)
        {
            return ContactManualDepartmentRepository.GetListByFilter(new ContactManualDepartmentParameter
            {
                Ids = ids
            });
        }
        public ContactManualDepartmentViewModel Get(int id)
        {
            var model = ContactManualDepartmentRepository.Get(id);

            if (model == null)
                return null;

            return new ContactManualDepartmentViewModel 
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public ContactManualDepartmentViewModel GetByName(string dpartmentName)
        {
            var model = ContactManualDepartmentRepository.GetByFilter(new ContactManualDepartmentParameter 
            {
                Name = dpartmentName
            });

            if(model == null)
                return null;

            return new ContactManualDepartmentViewModel
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public void Update(UserBriefModel user, ContactManualDepartmentViewModel model)
        {
            var entity = ContactManualDepartmentRepository.Get(model.Id);

            entity.Name = model.Name;

            ContactManualDepartmentRepository.Update(user,entity);
        }

        public void Create(UserBriefModel user, ContactManualDepartmentViewModel model)
        {
            var entity = new ContactManualDepartmentModel
            {
                Name = model.Name,
                Type = ContactManualDepartmentTypeEnum.General
            };
            ContactManualDepartmentRepository.Create(user,entity);
        }
    }
}