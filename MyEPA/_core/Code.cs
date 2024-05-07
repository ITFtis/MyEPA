using MyEPA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        /// 資源調度項目
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<int, string>> GetRecItems()
        {
            List<KeyValuePair<int, string>> result = new List<KeyValuePair<int, string>>()
            {
                new KeyValuePair<int, string>(1, "子母式垃圾車"),
                new KeyValuePair<int, string>(2, "水肥車"),
                new KeyValuePair<int, string>(3, "抓斗車"),
                new KeyValuePair<int, string>(4, "挖土機"),
                new KeyValuePair<int, string>(5, "消毒車"),
                new KeyValuePair<int, string>(6, "密封式壓縮垃圾車"),
                new KeyValuePair<int, string>(7, "掃(洗)街車"),
                new KeyValuePair<int, string>(8, "清溝(溝泥)車"),
                new KeyValuePair<int, string>(9, "鏟裝車(山貓)"),
                new KeyValuePair<int, string>(10, "資源(含廚餘)回收垃圾車"),
                new KeyValuePair<int, string>(11, "轉運車"),
                new KeyValuePair<int, string>(12, "推土機"),
                new KeyValuePair<int, string>(13, "框式垃圾車"),
            };

            return result;
        }
    }
}