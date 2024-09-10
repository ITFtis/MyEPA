using System.ComponentModel;

namespace MyEPA.Enums
{
    public enum DutyEnum
    {
        /// <summary>
        /// 清潔隊
        /// </summary>
        [Description("清潔隊")]
        Cleaning = 1,
        /// <summary>
        /// 環保局
        /// </summary>
        [Description("環保局")]
        EPB = 2,
        /// <summary>
        /// 環境部
        /// </summary>
        [Description("環境部")]
        EPA = 3,
        /// <summary>
        /// 自來水公司
        /// </summary>
        [Description("自來水公司")]
        Water = 4,
        /// <summary>
        /// 北中南督察大隊
        /// </summary>
        [Description("北中南督察大隊")]
        Team = 5,
        /// <summary>
        /// 環管署
        /// </summary>
        [Description("環管署")]
        Corps = 6,
        [Description("電子手冊")]
        ContactManual = 10,
        [Description("署補助車輛管理系統")]
        EPACar = 11,
    }
}