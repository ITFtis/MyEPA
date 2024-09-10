using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Enums
{
    public enum ApplyStatusEnum
    {
        [Description("待審核")]
        Pending = 0,
      
        [Description("審核中")]
        Processing = 1,

        [Description("轉呈環境部")]
        SendToEpa = 2,

        /// <summary>
        /// 環保局/環境部核定
        /// </summary>
        [Description("已核定")]
        Confrim = 3,
           
        [Description("退回")]
        Reject = 4,
    }
}