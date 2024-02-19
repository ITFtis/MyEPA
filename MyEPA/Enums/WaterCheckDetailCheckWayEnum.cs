using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Enums
{
    public enum WaterCheckDetailCheckWayEnum
    {
        [Description("NIEA_W408.51A")]
        NIEA_W408_51A = 1001,
        [Description("NIEA_E230.54B")]
        NIEA_E230_54B = 2001,
        [Description("NIEA_E201.53B")]
        NIEA_E201_53B = 2002,
        [Description("NIEA_E202.52B")]
        NIEA_E202_52B = 2003,
        [Description("NIEA_E215.50C")]
        NIEA_E215_50C = 2004,
        [Description("NIEA_E230.52B")]
        NIEA_E230_52B = 2005,
        [Description("NIEA_E231.52B")]
        NIEA_E231_52B = 2006,
        [Description("NIEA_W424.52A")]
        NIEA_W424_52A = 3001,
        [Description("NIEA_W219.52C")]
        NIEA_W219_52C = 4001
    }
}