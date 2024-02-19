using System.ComponentModel;

namespace MyEPA.Enums
{
    public enum DefendStatusEnum
    {
        /// <summary>
        /// 未通報
        /// </summary>
        [Description("未通報")]
        UnNotification = 0,
        /// <summary>
        /// 未確認
        /// </summary>
        [Description("未確認")]
        Waiting = 1,
        /// <summary>
        /// 已確認
        /// </summary>
        [Description("已確認")]
        Confirm = 2,
        
    }
}