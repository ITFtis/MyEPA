using MyEPA.Models;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA
{
    public class SysFunc
    {
        static CityService CityService = new CityService();

        /// <summary>
        /// (權限)縣市清單
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static List<CityModel> GetCitysF1(UserBriefModel user)
        {             
            List<CityModel> citys = new List<CityModel>();
            if (!user.IsAdmin)
            {
                citys.Add(CityService.Get(user.CityId));
            }
            else
            {
                citys = CityService.GetAll().Select(e => new CityModel
                {
                    City = e.City,
                    Id = e.Id,
                }).ToList();
            }

            return citys;
        }
    }
}