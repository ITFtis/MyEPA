﻿using MyEPA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyEPA.Repositories;
using NPOI.SS.Formula.Functions;

namespace MyEPA
{
    public class Code
    {

        /// <summary>
        /// 資源調度類型
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<int, string>> GetRecType()
        {
            List<KeyValuePair<int, string>> result = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "調度需求"),
                new KeyValuePair<int, string>(2, "提供資源"),
                new KeyValuePair<int, string>(3, "可提供調度"),
            };

            return result;
        }

        /// <summary>
        /// 資源調度配置狀態
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<int, string>> GetRecStatus()
        {
            List<KeyValuePair<int, string>> result = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "未結案"),
                new KeyValuePair<int, string>(2, "已結案"),
            };

            return result;
        }

        /// <summary>
        /// 資源調度類別
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<int, string>> GetRecTypeItems()
        {
            List<KeyValuePair<int, string>> result = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "車輛"),
                new KeyValuePair<int, string>(2, "設備"),
                new KeyValuePair<int, string>(3, "消毒藥劑"),
            };

            return result;
        }

        /// <summary>
        /// (單筆)資源調度項目
        /// </summary>
        /// <returns></returns>
        public static string GetOneRecItems(int typeItems, string items)
        {
            string result = items;

            //3選1
            if (typeItems == 1)
            {
                VehicleTypeRepository VehicleTypeRepository = new VehicleTypeRepository();
                var dd1 = VehicleTypeRepository.GetList().Where(a => a.Type == items).FirstOrDefault();
                if (dd1 != null)
                    result = dd1.Name;
            }
            else if (typeItems == 2)
            {
                DisinfectorTypeRepository DisinfectorTypeRepository = new DisinfectorTypeRepository();
                var dd2 = DisinfectorTypeRepository.GetList().Where(a => a.Type == items).FirstOrDefault();
                if (dd2 != null)
                    result = dd2.Name;
            }
            else if (typeItems == 3)
            {
                DisinfectantTypeRepository DisinfectantTypeRepository = new DisinfectantTypeRepository();
                var dd3 = DisinfectantTypeRepository.GetList().Where(a => a.Type == items).FirstOrDefault();
                if (dd3 != null)
                    result = dd3.Name;
            }

            return result;
        }

        /// <summary>
        /// 通報層級驗證(環保局->清潔隊)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<int, string>> GetValidDefend()
        {
            List<KeyValuePair<int, string>> result = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "無"),
                new KeyValuePair<int, string>(2, "有"),
            };

            return result;
        }

        /// <summary>
        /// 區大隊
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<int, string>> GetCityArea()
        {
            List<KeyValuePair<int, string>> result = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "環境管理署北區環境管理中心"),
                new KeyValuePair<int, string>(2, "環境管理署中區環境管理中心"),
                new KeyValuePair<int, string>(3, "環境管理署南區環境管理中心"),
            };

            return result;
        }

        /// <summary>
        /// 閾值
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<int, string>> GetDDLCt()
        {
            List<KeyValuePair<int, string>> result = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "低於閾值"),
                new KeyValuePair<int, string>(2, "高於閾值(正常)"),
            };

            return result;
        }

        /// <summary>
        /// 台灣各區分類(縣市)
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<int, string>> GetTWTypeCity()
        {
            List<KeyValuePair<int, string>> result = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "1,2,3,19"),//北基宜地區
                new KeyValuePair<int, string>(2, "4,5,6,7"),//桃竹苗地區
                new KeyValuePair<int, string>(3, "8,9,10"),//中彰投地區
                new KeyValuePair<int, string>(4, "11,12,13,14"),//雲嘉南地區
                new KeyValuePair<int, string>(5, "15,16"),//高屏地區
                new KeyValuePair<int, string>(6, "17,18"),//花東地區
                new KeyValuePair<int, string>(7, "20,21,22"),//澎金馬地區
            };

            return result;
        }
    }
}