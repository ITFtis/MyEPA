using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MyEPA.Models
{
    public class DamageStatisticsModel : DamageStatisticsBaseModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TownId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TownName { get; set; }
        /// <summary>
        /// 行政區域(公頃)
        /// </summary>
        public decimal CityArea { get; set; }
        /// <summary>
        /// 行政區域(公頃)
        /// </summary>
        public decimal TownArea { get; set; }
    }
    public class DamageStatisticsBaseModel
    {
        /// <summary>
        /// 居家環境淹水面積(公頃)
        /// </summary>
        [DisplayName("居家環境淹水面積(公頃)")]
        public decimal FloodArea { get; set; }
        /// <summary>
        /// 已清除污泥(公噸)
        /// </summary>
        [DisplayName("已清除污泥(公噸)")]
        public decimal CLE_MUD { get; set; }
        /// <summary>
        /// 預估廢棄物量(公噸)
        /// </summary>
        [DisplayName("預估廢棄物量(公噸)")]
        public decimal PR_Garbage { get; set; }
        /// <summary>
        /// 已清除廢棄物(公噸)
        /// </summary>
        [DisplayName("已清除垃圾(公噸)")]
        public decimal CLE_Trash { get; set; }
        [DisplayName("已清除廢棄物(公噸)")]
        public decimal CLE_Garbage { get; set; }
        /// <summary>
        /// 已動用清除人力(人次)
        /// </summary>
        [DisplayName("已動用清除人力(人次)")]
        public int CleaningMemberQuantity { get; set; }
        /// <summary>
        /// 已動用國軍人力(人次)
        /// </summary>
        [DisplayName("已動用國軍人力(人次)")]
        public int NationalArmyQuantity { get; set; }
        /// <summary>
        /// 已消毒面積(公頃)
        /// </summary>
        [DisplayName("實際消毒面積(公頃)")]
        public decimal DisinfectArea { get; set; }
        /// <summary>
        /// 災情通報異動時間
        /// </summary>
        [DisplayName("災情通報異動時間")]
        public DateTime? UpdateDate { get; set; }
        /// <summary>
        /// 庫存消毒藥品數量液體(公升)
        /// </summary>
        [DisplayName("庫存消毒藥品數量液體(公升)")]
        public decimal DisinfectantSolidAmount { get; set; }
        /// <summary>
        /// 庫存消毒藥品數量固體(公斤)
        /// </summary>
        [DisplayName("庫存消毒藥品數量固體(公斤)")]
        public decimal DisinfectantLiquidAmount { get; set; }
        /// <summary>
        /// 是否結案
        /// </summary>
        [DisplayName("是否結案")]
        public bool? IsDone { get; set; }
    }
    public class DamageStatisticsTownViewModel : DamageStatisticsBaseModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int TownId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("鄉鎮名稱")]
        public string TownName { get; set; }
        /// <summary>
        /// 行政區域(公頃)
        /// </summary>
        [DisplayName("行政區域(公頃)")]
        public decimal TownArea { get; set; }
    }
    public class DamageStatisticsCityViewModel : DamageStatisticsBaseModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("縣市名稱")]
        public string CityName { get; set; }
        /// <summary>
        /// 行政區域(公頃)
        /// </summary>
        [DisplayName("行政區域(公頃)")]
        public decimal CityArea { get; set; }
    }
}