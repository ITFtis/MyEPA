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
    }
}