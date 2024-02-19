using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using System.Collections.Generic;
using System.ComponentModel;

namespace MyEPA.Services
{
    public class PestService
    {
        PestRepository PestRepository = new PestRepository();
        public List<PestModel> GetByFilter(PestFilterParameter filter)
        {
            return PestRepository.GetByFilter(filter);
        }
    }
}