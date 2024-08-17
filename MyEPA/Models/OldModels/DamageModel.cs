using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Web.Configuration;
using System.ComponentModel;
using MyEPA.Enums;

namespace MyEPA.Models
{
    public class DamageModel
    {
        [AutoKey]
        public int Id { get; set; }
        
        [DisplayName("災害主題")]
        public int DiasterId { get; set; }
        public int CityId { get; set; }
        public int TownId { get; set; }
        [DisplayName("災害日期")]
        public DateTime? ReportDay { get; set; }
        /// <summary>
        /// 主要受災(區村里)
        /// </summary>
        [DisplayName("主要受災(區村里)")]
        public string DamagePlace { get; set; }
        /// <summary>
        /// 災區面積(公頃)
        /// </summary>
        [DisplayName("災區面積(公頃)")]
        [DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]

        public decimal DamageArea { get; set; }
        /// <summary>
        /// 淹水面積(公頃)
        /// </summary>
        [DisplayName("淹水面積(公頃)")]
        [DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        public decimal FloodArea { get; set; }
        /// <summary>
        /// 垃圾場損壞情形
        /// </summary>
        [DisplayName("垃圾場損壞情形")]
        public string DumpSiteDesc { get; set; }
        /// <summary>
        /// 焚化廠損壞情形
        /// </summary>
        [DisplayName("焚化廠損壞情形")]
        public string IncinerationPlantDesc { get; set; }
        /// <summary>
        /// 其他損壞情形
        /// </summary>
        [DisplayName("其他損壞情形")]
        public string Other { get; set; }

        /// <summary>
        /// 環境清理通報日期
        /// </summary>
        [DisplayName("環境清理通報")]
        public DateTime? CleanDay { get; set; }

        /// <summary>
        /// 消毒日期
        /// </summary>
        [DisplayName("實際環境消毒日期")]
        public DateTime? DisinfectDate { get; set; }
        /// <summary>
        /// 消毒面積(公頃)
        /// </summary>
        [DisplayName("實際消毒面積(公頃)")]
        [DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        public decimal DisinfectArea { get; set; }
        /// <summary>
        /// 預估廢棄物量(公噸)
        /// </summary>
        [DisplayName("預估廢棄物量(公噸)")]
        [DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        public decimal PR_Garbage { get; set; }
        /// <summary>
        /// 已消毒環境(公噸)
        /// </summary>
        [DisplayName(" 已消毒環境(公噸)")]
        [DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        public decimal CLE_Disinfect { get; set; }
        /// <summary>
        /// 已清除污泥(公噸)
        /// </summary>
        [DisplayName("已清除污泥(公噸)")]
        [DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        public decimal CLE_MUD { get; set; }
        /// <summary>
        /// 已清除垃圾(公噸)
        /// </summary>
        [DisplayName("已清除垃圾(公噸)")]
        [DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        public decimal CLE_Trash { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        [DisplayName("已清除廢棄物(公噸)")]
        public decimal CLE_Garbage { get; set; }
        /// <summary>
        /// 已動用清潔人力(人)
        /// </summary>
        [DisplayName("已動用清潔人力(人)")]
        public int CleaningMemberQuantity { get; set; }
        /// <summary>
        /// 已動用國軍支援(人)
        /// </summary>
        [DisplayName("已動用國軍支援(人)")]
        public int NationalArmyQuantity { get; set; }
        [DisplayName("照片")]
        public int? ImageFileId { get; set; }
        [DisplayName("檔案")]
        public int? FileId { get; set; }
        public DateTime? ConfirmTime { get; set; }
        [DisplayName("環境清理確認時間")]
        public DateTime? CleanConfirmTime { get; set; }
        [DisplayName("災情通報狀態")]
        public DamageStatusEnum? Status { get; set; }
        [DisplayName("環境清理狀態")]
        public DamageStatusEnum? CleanStatus { get; set; }
        public DutyEnum DutyId { get; set; }
        [DisplayName("通報時間")]
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// 垃圾掩埋場
        /// </summary>
        [DisplayName("垃圾掩埋場")]
        public string LandfillIds { get; set; }
        /// <summary>
        /// 焚化爐
        /// </summary>
        [DisplayName("焚化爐")]
        public string IncineratorIds { get; set; }
        /// <summary>
        /// 總隊處理情形
        /// </summary>
        [DisplayName("總隊處理情形")]
        public string ProcessDesc { get; set; }
        /// <summary>
        /// 備註
        /// </summary>
        [DisplayName("備註")]
        public string Note { get; set; }
        /// <summary>
        /// (災情通報)有無災情
        /// </summary>
        public bool? IsDamage { get; set; }
        /// <summary>
        /// (環境清理)有無災情
        /// </summary>
        public bool? IsDamageClean { get; set; }
        public DateTime? TeamConfirmTime { get; set; }
        [DisplayName("環境清理確認時間")]
        public DateTime? CleanTeamConfirmTime { get; set; }
    }
    public class DamageConfirmList : DamageJoinModel
    {
        public new bool? IsDamage { get; set; }
    }
    public class DamageJoinModel : DamageModel
    {
        [DisplayName("縣市")]
        public string CityName { get; set; }
        [DisplayName("通報單位")]
        public string TownName { get; set; }
        
    }

    public class DamageTeamConfirmViewModel
    {
        public int? Id { get; set; }
        [DisplayName("縣市災害通報確認")]
        public DamageStatusEnum? Status { get; set; }
        [DisplayName("縣市環境清潔確認")]
        public DamageStatusEnum? CleanStatus { get; set; }
        [DisplayName("災害日期")]
        public DateTime? ReportDay { get; set; }
        [DisplayName("環境清理通報日期")]
        public DateTime? CleanDay { get; set; }
        [DisplayName("縣市名稱")]
        public string CityName { get; set; }
        public int CityId { get; set; }
        
    }

    public class DamageTownViewModel
    {
        [DisplayName("通報/確認狀態")]
        public DamageStatusEnum? Status { get; set; }
        [DisplayName("環境清理狀態")]
        public DamageStatusEnum? CleanStatus { get; set; }
        [DisplayName("通報單位")]
        public string TownName { get; set; }
        /// <summary>
        /// 主要受災(區村里)
        /// </summary>
        [DisplayName("主要受災(區村里)")]
        public string DamagePlace { get; set; }
        /// <summary>
        /// 有無災情
        /// </summary>
        [DisplayName("災害現況")]
        public bool? IsDamage { get; set; }
        /// <summary>
        /// 有無災情
        /// </summary>
        [DisplayName("(環境清理)有無災情")]
        public bool? IsDamageClean { get; set; }
        /// <summary>
        /// 災區面積(公頃)
        /// </summary>
        [DisplayName("災區面積(公頃)")]
        public decimal DamageArea { get; set; }
        /// <summary>
        /// 淹水面積(公頃)
        /// </summary>
        [DisplayName("淹水面積(公頃)")]
        public decimal FloodArea { get; set; }
        /// <summary>
        /// 預估廢棄物量(公噸)
        /// </summary>
        [DisplayName("預估廢棄物量(公噸)")]
        public decimal PR_Garbage { get; set; }
        /// <summary>
        /// 垃圾場損壞情形
        /// </summary>
        [DisplayName("垃圾場損壞情形")]
        public string DumpSiteDesc { get; set; }
        /// <summary>
        /// 焚化廠損壞情形
        /// </summary>
        [DisplayName("焚化廠損壞情形")]
        public string IncinerationPlantDesc { get; set; }
        /// <summary>
        /// 其他損壞情形
        /// </summary>
        [DisplayName("其他損壞情形")]
        public string Other { get; set; }
        /// <summary>
        /// 已清除污泥(公噸)
        /// </summary>
        [DisplayName("已清除污泥(公噸)")]
        public decimal CLE_MUD { get; set; }
        /// <summary>
        /// 已清除垃圾(公噸)
        /// </summary>
        [DisplayName("已清除垃圾(公噸)")]
        public decimal CLE_Trash { get; set; }
        [DisplayName("已清除廢棄物(公噸)")]
        public decimal CLE_Garbage { get; set; }
        /// <summary>
        /// 消毒日期
        /// </summary>
        [DisplayName("實際消毒日期")]
        public DateTime? DisinfectDate { get; set; }
        /// <summary>
        /// 已動用清潔人力(人)
        /// </summary>
        [DisplayName("已動用清潔人力(人)")]
        public int CleaningMemberQuantity { get; set; }
        /// <summary>
        /// 已動用國軍支援(人)
        /// </summary>
        [DisplayName("已動用國軍支援(人)")]
        public int NationalArmyQuantity { get; set; }
        /// <summary>
        /// 已消毒環境(公噸)
        /// </summary>
        [DisplayName("已消毒環境(公噸)")]
        public decimal CLE_Disinfect { get; set; }
        [DisplayName("更新日期")]
        public DateTime? UpdateDate { get; set; }
        [DisplayName("災情通報確認時間")]
        public DateTime? ConfirmTime { get; set; }
        [DisplayName("環境清理確認時間")]
        public DateTime? CleanConfirmTime { get; set; }
        [DisplayName("縣市")]
        public string CityName { get; set; }
        public int CityId { get; set; }
        [DisplayName("災害日期")]
        public DateTime? ReportDay { get; set; }
        [DisplayName("環境清理通報日期")]
        public DateTime? CleanDay { get; set; }
        public int TownId { get; set; }
    }
}