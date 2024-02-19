using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Enums
{
    public enum DisinfectantUseTypeEnum
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
        /// 紅火蟻
        /// </summary>
        [Description("紅火蟻")]
        RIFA = 3,
        /// <summary>
        /// 荔枝椿象
        /// </summary>
        [Description("荔枝椿象")]
        TessaratomaPapillosaDrury = 4,
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 5

    }
}