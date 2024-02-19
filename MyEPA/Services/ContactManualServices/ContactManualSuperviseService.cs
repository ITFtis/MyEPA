using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Services
{
    public class ContactManualSuperviseService
    {
        ContactManualSuperviseRepository ContactManualSuperviseRepository = new ContactManualSuperviseRepository();
        ContactManualDepartmentRepository ContactManualDepartmentRepository = new ContactManualDepartmentRepository();
        public List<ContactManualSuperviseViewModel> GetAll()
        {
            var supervises = ContactManualSuperviseRepository.GetList();
            var departments = ContactManualDepartmentRepository.GetListByFilter(new ContactManualDepartmentParameter
            {
                Ids = supervises.Select(e => e.DepartmentId).ToList()
            }).ToDictionary(e => e.Id, e => e.Name);

            return supervises.Select(e => new ContactManualSuperviseViewModel 
            {
                DepartmentId = e.DepartmentId,
                DepartmentName = departments.GetValue(e.DepartmentId),
                Describe = e.Describe,
                Id = e.Id
            }).ToList();
        }

        public ContactManualSuperviseModel Get(int id)
        {
            return ContactManualSuperviseRepository.Get(id);
        }

        public void Update(UserBriefModel user, ContactManualSuperviseViewModel model)
        {
            var entity = ContactManualSuperviseRepository.Get(model.Id);

            entity.DepartmentId = model.DepartmentId;
            entity.Describe = model.Describe;

            ContactManualSuperviseRepository.Update(user, entity);
        }

        public void Create(UserBriefModel user, ContactManualSuperviseViewModel model)
        {
            ContactManualSuperviseRepository.Create(user, new ContactManualSuperviseModel 
            {
                DepartmentId = model.DepartmentId,
                Describe = model.Describe,
            });
        }
    }
}