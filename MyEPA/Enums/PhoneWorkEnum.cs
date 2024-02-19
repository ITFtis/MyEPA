using System.ComponentModel;

namespace MyEPA.Enums
{
    public enum PhoneWorkEnum
    {
        /// <summary>
        /// 辦理中
        /// </summary>
        [Description("辦理中")]
        Doing = 1,
        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Done = 2,
        [Description("轉其他單位")]
        Forward = 3,
    }
}