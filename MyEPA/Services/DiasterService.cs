using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Services
{
    public class DiasterService
    {
        DiasterRepository DiasterRepository = new DiasterRepository();
        public List<DiasterModel> GetAll()
        {
            return DiasterRepository.GetList().OrderByDescending(e => e.Id).ToList();
        }
        public List<DiasterModel> GetByFilter(DiasterFilterParameter filter)
        {
            return DiasterRepository.GetByFilter(filter).OrderByDescending(e => e.Id).ToList();
        }
        public bool IsExistsRuning()
        {
            string key = CacheKeyHelper.GetIsExistsRuning();

            bool isExistsRuning = CacheHelper.GetCacheOrQuery<bool>(key, () =>
             {
                 return DiasterRepository.IsExistsRuning();
             }, new TimeSpan(0, 0, 5));

            return isExistsRuning;
        }
    }
}