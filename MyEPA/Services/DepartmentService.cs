using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using MyEPA.ViewModels;
using System.Collections.Generic;

namespace MyEPA.Services
{
    public class DepartmentService
    {
        DepartmentRepository DepartmentRepository = new DepartmentRepository();
        public List<DepartmentModel> GetAll()
        {
            return DepartmentRepository.GetList();
        }

        public List<DepartmentModel> GetByIds(List<int> ids)
        {
            return DepartmentRepository.GetByFilter(new DepartmentParameter
            {
                Ids = ids
            });
        }
        public DepartmentModel Get(int id)
        {
            return DepartmentRepository.Get(id);
        }

        public void Update(DepartmentViewModel model)
        {
            var entity = DepartmentRepository.Get(model.Id);

            entity.Name = model.Name;

            DepartmentRepository.Update(entity);
        }

        public void Create(DepartmentViewModel model)
        {
            var entity = new DepartmentModel 
            {
                Name = model.Name
            };
            DepartmentRepository.Create(entity);
        }
    }
}