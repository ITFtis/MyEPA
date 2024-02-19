using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using System.Collections.Generic;

namespace MyEPA.Services
{

    public class TownService
    {
        TownRepository TownRepository = new TownRepository();
        public List<TownModel> GetAll()
        {
            return TownRepository.GetList();
        }
        public List<TownModel> GetByCityId(int cityId)
        {
            return TownRepository.GetByCityId(cityId);
        }
        public List<TownModel> GetListByFilter(TownFilterParameter filter)
        {
            return TownRepository.GetListByFilter(filter);
        }
        public TownModel GetByFilter(TownFilterParameter filter)
        {
            return TownRepository.GetByFilter(filter);
        }
        public TownModel Get(int townId)
        {
            return TownRepository.Get(townId);
        }
        /// <summary>
        /// 由 townId 取得 town 名稱
        /// </summary>
        /// <param name="townId"></param>
        /// <returns></returns>
        public string GetTownNameByTownId(int townId) 
        {
            return TownRepository.Get(townId)?
                                 .Name;
        }
    }
}