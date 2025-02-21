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
        /// (權限)縣市清單 客製化給調度資源
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isAll">特殊設定(是否全部)</param>
        /// <returns></returns>
        public static List<CityModel> GetCitysRecResource(UserBriefModel user, bool isAll = false)
        {
            List<CityModel> citys = new List<CityModel>();
            bool isAdmin = user.Town.Trim() == "環境管理署".Trim() || user.IsAdmin;

            if (isAdmin || isAll)
            {
                citys = CityService.GetAll().Select(e => new CityModel
                {
                    City = e.City,
                    Id = e.Id,
                }).ToList();
            }
            else
            {
                citys.Add(CityService.Get(user.CityId));
            }

            return citys;
        }
    }
}