using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Enums
{
    public enum NormalActiveStatusEnum
    {
        [Description("隐藏")]
        Inactive = 0,
        [Description("显示")]
        Active = 1,
        [Description("刪除（不再顯示）")]
        InvisibleDelete = 999,

    }
}