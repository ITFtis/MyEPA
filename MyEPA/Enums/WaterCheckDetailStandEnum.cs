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
    public enum WaterCheckDetailStandEnum
    {
        [Description("0.2-1.0")]
        ChlorineOption001 = 1001,
        [Description("0.2-2.0")]
        ChlorineOption011 = 1011,
        [Description("6MPN/100毫升")]
        EColiOption001 = 2001,
        [Description("6CFU/100毫升")]
        EColiOption011 = 2011,
        [Description("6.0-8.5")]
        HydrogenOption001 = 3001,
        [Description("2")]
        TurbidityOption002 = 4002,
        [Description("4")]
        TurbidityOption004 = 4004,
        [Description("10")]
        TurbidityOption010 = 4010,
        [Description("30")]
        TurbidityOption030 = 4030,

    }
}