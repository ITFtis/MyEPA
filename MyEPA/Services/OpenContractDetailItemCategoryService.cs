using MyEPA.Models;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Services
{
    public class OpenContractDetailItemCategoryService
    {
        OpenContractDetailItemCategoryRepository OpenContractDetailItemCategoryRepository = new OpenContractDetailItemCategoryRepository();
        public List<OpenContractDetailItemCategoryModel> GetAll()
        {
            return OpenContractDetailItemCategoryRepository.GetList();
        }
    }
}