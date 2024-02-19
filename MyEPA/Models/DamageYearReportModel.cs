using System.ComponentModel;

namespace MyEPA.Models
{
    public class DamageYearReportModel
    {
        /// <summary>
        /// 災害ID
        /// </summary>
        public int DiasterId { get; set; }
        /// <summary>
        /// 居家環境淹水面積(公頃)
        /// </summary>
        public decimal FloodArea { get; set; }
        /// <summary>
        /// 已清除污泥(公噸)
        /// </summary>
        public decimal CLE_MUD { get; set; }
        /// <summary>
        /// 已清除廢棄物(公噸)
        /// </summary>
        public decimal CLE_Garbage { get; set; }
        /// <summary>
        /// 已動用清潔人力(人)
        /// </summary>
        public int CleaningMemberQuantity { get; set; }
        /// <summary>
        /// 已動用國軍支援(人)
        /// </summary>
        public int NationalArmyQuantity { get; set; }
        /// <summary>
        /// 已消毒環境(公噸)
        /// </summary>
        public decimal CLE_Disinfect { get; set; }

    }
}