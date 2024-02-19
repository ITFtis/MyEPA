using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Enums
{
    public enum FacilityDamageTypeEnum
    {

        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        ALL = 0,


        /// <summary>
        /// 垃圾場
        /// </summary>
        [Description("垃圾場")]
        DumpSiteDesc = 1,
        /// <summary>
        /// 焚化廠
        /// </summary>
        [Description("焚化廠")]
        IncinerationPlantDesc = 2,
        /// <summary>
        /// 其它
        /// </summary>
        [Description("其它")]
        Other = 3
    }
}