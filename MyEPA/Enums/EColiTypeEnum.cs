using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Enums
{
    public enum EColiTypeEnum
    {
        /// <summary>
        /// 檢驗中
        /// </summary>
        [Description("檢驗中")]
        Testing = 0,
        /// <summary>
        /// 未檢出
        /// </summary>
        [Description("未檢出")]
        Zero = 1,
        /// <summary>
        /// 小於1
        /// </summary>
        [Description("<1")]
        LessThan1 = 2,
        /// <summary>
        /// TNTC
        /// </summary>
        [Description("過多無法計數(TNTC)")]
        TNTC = 3,
        /// <summary>
        /// 其他數值
        /// </summary>
        [Description("其他數值")]
        Other = 4,
        /// <summary>
        /// 不檢驗
        /// </summary>
        [Description("不檢驗")]
        Untested = 5
    }
}