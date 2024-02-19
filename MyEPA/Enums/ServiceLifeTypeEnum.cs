using System.ComponentModel;

namespace MyEPA.Enums
{
    public enum ServiceLifeTypeEnum
    {
        /// <summary>
        /// 三個月
        /// </summary>
        [Description("三個月")]
        ThreeMonths,
        /// <summary>
        /// 半年
        /// </summary>
        [Description("半年")]
        HalfYear,
        /// <summary>
        /// 一年
        /// </summary>
        [Description("一年")]
        OneYear,
    }
}