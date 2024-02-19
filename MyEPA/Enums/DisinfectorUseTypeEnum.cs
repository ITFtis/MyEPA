using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Enums
{
    public enum DisinfectorUseTypeEnum
    {
        /// <summary>
        /// 環境消毒
        /// </summary>
        [Description("環境消毒")]
        Environment = 1,
        /// <summary>
        /// 登革熱
        /// </summary>
        [Description("登革熱")]
        Dengue = 2,
        /// <summary>
        /// 共用
        /// </summary>
        [Description("共用")]
        Common = 3,
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 4
    }
}