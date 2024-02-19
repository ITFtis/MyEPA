using MyEPA.EPA.Attribute;
using System.ComponentModel;

namespace MyEPA.ViewModels
{
    public class YearReportViewModel
    {
        [DisplayName("災害名稱")]
        public string DiasterName { get; set; }
        /// <summary>
        /// 居家環境淹水面積(公頃)
        /// </summary>
        [DataTableSum]
        [DisplayName("居家環境淹水面積(公頃)")]
        public decimal FloodArea { get; set; }
        /// <summary>
        /// 已清除污泥(公噸)
        /// </summary>
        [DataTableSum]
        [DisplayName("已清除污泥(公噸)")]
        public decimal CLE_MUD { get; set; }
        /// <summary>
        /// 已清除廢棄物(公噸)
        /// </summary>
        [DataTableSum]
        [DisplayName("已清除廢棄物(公噸)")]
        public decimal CLE_Garbage { get; set; }
        /// <summary>
        /// 已動用清潔人力(人)
        /// </summary>
        [DataTableSum]
        [DisplayName("已動用清潔人力(人)")]
        public int CleaningMemberQuantity { get; set; }
        /// <summary>
        /// 已動用國軍支援(人)
        /// </summary>
        [DataTableSum]
        [DisplayName("已動用國軍支援(人)")]
        public int NationalArmyQuantity { get; set; }
        /// <summary>
        /// 已消毒環境(公噸)
        /// </summary>
        [DataTableSum]
        [DisplayName("已消毒環境(公頃)")]
        public decimal CLE_Disinfect { get; set; }
        [DataTableSum]
        [DisplayName("環保單位水質抽檢不合格數")]
        public int EPFailureCount { get; set; }
        [DataTableSum]
        [DisplayName("環保單位水質抽檢總數")]
        public int EPCount { get; set; }
        [DataTableSum]
        [DisplayName("自來水單位水質抽檢不合格數")]
        public int WaterFailureCount { get; set; }
        [DataTableSum]
        [DisplayName("自來水單位水質抽檢總數")]
        public int WaterCount { get; set; }
    }
}