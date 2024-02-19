using System.ComponentModel;

namespace MyEPA.Enums
{
    /// <summary>
    /// 從 SourceTypeEnum 擷取, 為了讓編費相同故非從 1 開始
    /// </summary>
    public enum ApplyTypeEnum
    {
        /// <summary>
        /// 請求人力支援
        /// </summary>
        [Description("人力支援")]
        ApplyPeople = 3,

        /// <summary>
        /// 請求消毒藥劑支援
        /// </summary>
        [Description("消毒藥劑支援")]
        ApplyMedicine = 4,

        /// <summary>
        /// 請求補助款支援
        /// </summary>
        [Description("補助款支援")]
        ApplySubsidy = 5,

        /// <summary>
        /// 其他(包括垃圾場災損)
        /// </summary>
        [Description("其他支援")]
        ApplyOther = 6,

        /// <summary>
        /// 請求車輛支援
        /// </summary>
        [Description("車輛支援")]
        ApplyCar = 7,

        /// <summary>
        /// 環境消毒設備支援
        /// </summary>
        [Description("環境消毒設備支援")]
        ApplyDisinfectionEquipment = 8,
    }
}