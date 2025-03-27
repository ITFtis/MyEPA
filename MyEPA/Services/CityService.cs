using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Models.FilterParameter;
using MyEPA.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Services
{
    public class CityService
    {
        CityRepository CityRepository = new CityRepository();
        public List<CityModel> GetAll()
        {
            return CityRepository.GetList();
        }
        public CityModel Get(int cityId)
        {
            return CityRepository.Get(cityId);
        }
        public List<CityModel> GetCountyOrderBySort()
        {
            return CityRepository.GetListByFilter(new CityFilterParameter
            {
                IsCounty = true
            }).OrderBy(e => e.Sort).ToList();
        }

        /// <summary>
        /// (權限)縣市清單 => 判斷單位(duty)
        /// (23筆)22個縣市+1個環境部
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static List<CityModel> GetCitysF1(UserBriefModel user)
        {
            CityRepository CityRepository = new CityRepository();
            
            List<int> cityIds = new List<int>();
            if (user.Duty == Enums.DutyEnum.EPA
                || user.Duty == Enums.DutyEnum.Team
                || user.Duty == Enums.DutyEnum.Corps
                )
            {
                //全部                
            }
            else
            {
                cityIds.Add(user.CityId);
            }

            List<CityModel> citys = CityRepository.GetListByFilter(new CityFilterParameter
            {
                IsCity23 = true,
                CityIds = cityIds,
            }).OrderByDescending(a => a.IsCounty).ThenBy(e => e.Sort).ToList();

            ////if (!user.IsAdmin)
            ////{
            ////    citys.Add(CityRepository.Get(user.CityId));
            ////}
            ////else
            ////{
            ////    citys = CityRepository.GetList().ToList();
            ////}

            return citys;
        }

        public List<CityWaterDivisionJoinModel> GetCountyOrderBySort(string town)
        {
            return CityRepository.GetWaterDivisions(town);
        }
        public List<CityModel> GetListByFilter(CityFilterParameter filter)
        {
            return CityRepository.GetListByFilter(filter);
        }

        public CityModel GetByCityName(string city)
        {
            return CityRepository.GetByFilter(new CityFilterParameter 
            {
                City = city
            });
        }

        /// <summary>
        /// 由 cityId 取得 city 名稱
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public string GetCiyNameByCityId(int cityId)
        {
            return CityRepository.Get(cityId)?.City;
        }
    }
}