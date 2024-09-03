using MyEPA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.ViewModels
{
    public class FacilityDamageViewModel
    {
        public int Id { get; set; }
        [DisplayName("縣市")]
        public string CityName { get; set; }
        [DisplayName("縣市排序")]
        public int CitySort { get; set; }
        [DisplayName("鄉鎮編號")]
        public int? TownId { get; set; }
        [DisplayName("鄉鎮")]
        public string TownName { get; set; }
        [DisplayName("災害日期")]
        public DateTime? ReportDay { get; set; }

        /// <summary>
        /// 主要受災(區村里)
        /// </summary>
        [DisplayName("主要受災(區村里)")]
        public string DamagePlace { get; set; }
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

        [DisplayName("通報時間")]
        public DateTime? CreateDate { get; set; }
        [DisplayName("場所名稱")]
        public List<string> Places { get; set; }
        [DisplayName("損壞說明")]
        public string DamageDesc { get; set; }
        [DisplayName("總隊處理情形")]
        public string ProcessDesc { get; set; }
        [DisplayName("備註")]
        public string Note { get; set; }

        [DisplayName("圖片")]
        public List<FileDataModel> Images { get; set; }

        [DisplayName("檔案")]
        public List<FileDataModel> Files { get; set; }
    }
}