using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA
{
    /// <summary>
    /// 文字轉換
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// Y/N轉是否
        /// </summary>
        /// <returns></returns>
        public static string YNToName(string str)
        {
            if (str == "Y")
            {
                str = "是";
            }
            else if (str == "N")
            {
                str = "否";
            }
            return str;
        }
    }
}