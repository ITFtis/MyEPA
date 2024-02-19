using MyEPA.Models;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Services
{
    public class ResourceTypeService
    {
        ResourceTypeRepository ResourceTypeRepository = new ResourceTypeRepository();
        public List<ResourceTypeModel> GetList()
        {
            return ResourceTypeRepository.GetList();
        }
    }
}