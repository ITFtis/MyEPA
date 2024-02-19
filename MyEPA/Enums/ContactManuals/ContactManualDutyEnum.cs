using System.ComponentModel;

namespace MyEPA.Enums
{
    public enum ContactManualDutyEnum
    {
        None = 0,
        /// <summary>
        /// 電子手冊-業務窗口
        /// </summary>
        [Description("業務窗口")]
        Business = 1,
        /// <summary>
        /// 電子手冊-管理者
        /// </summary>
        [Description("管理者")]
        Administrator = 2,
        /// <summary>
        /// 電子手冊-使用者
        /// </summary>
        [Description("使用者")]
        User = 3,
    }
}