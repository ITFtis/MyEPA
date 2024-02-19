using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Enums
{
    public enum ApplyOtherSubsidyTypeEnum
    {
        //用來標示沒有 type 的項目
        [Description("無")]
        None = 0,
        [Description("其他")]
        Other = 1,
        [Description("補助款")]
        Subsidy = 2,
    }
}