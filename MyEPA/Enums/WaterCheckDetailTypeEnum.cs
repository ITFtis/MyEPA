using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Enums
{
    /// <summary>
    /// 水樣別
    /// </summary>
    public enum WaterCheckDetailTypeEnum
    {
        [Description("直接供水")]
        Direct = 1,
        [Description("間接供水")]
        Indirect = 2,
        [Description("非自來水")]
        UnRunningWater = 3,
        [Description("簡易自來水")]
        SimpleRunningWater = 4
    }
}