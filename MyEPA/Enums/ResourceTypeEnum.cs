using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Enums
{
    public enum ResourceTypeEnum
    {
        [Description("救災清理機具")]
        Car = 1,
        [Description("藥品")]
        Drug = 2,
        [Description("流動廁所")]
        Toilet = 3,
        [Description("其他")]
        Other = 4,

    }
}