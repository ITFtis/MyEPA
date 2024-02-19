
using MyEPA.EPA.Attribute;
using System;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class DisinfectorSummaryTownReportModel : DisinfectorSummaryCityReportModel
    {
        [DisplayName("鄉鎮")]
        public string Town { get; set; }
        public int TownId { get; set; }
    }
    public class DisinfectorSummaryCityReportModel
    {
        public int Sort { get; set; }
        public int CityId { get; set; }
        [DisplayName("縣市")]
        public string City { get; set; }
        [DisplayName("噴霧機")]
        [DataTableSum]
        public int SprayerCount { get; set; }
        [DisplayName("消毒機(器)")]
        [DataTableSum]
        public int DisinfectorCount { get; set; }
        [DisplayName("熱煙霧機")]
        [DataTableSum]
        public int HotSmokeSachineCount { get; set; }
        [DisplayName("高壓清洗機")]
        [DataTableSum]
        public int PressureWasherCount { get; set; }









        [DisplayName("車載式高壓噴霧機")]
        [DataTableSum]
        public int SprayerCAR { get; set; }
        [DisplayName("高壓噴霧機")]
        [DataTableSum]
        public int SprayeSrHI { get; set; }
        [DisplayName("超低容量噴霧機")]
        [DataTableSum]
        public int SprayeSrLO { get; set; }
        [DisplayName("車載式煙霧機")]
        [DataTableSum]
        public int SMOK { get; set; }














        [DisplayName("其他")]
        [DataTableSum]
        public int OtherCount { get; set; }
        [DisplayName("最新更新時間")]
        public DateTime? UpdateTime { get; set; }
        [DisplayName("最新確認時間")]
        public DateTime? ConfirmTime { get; set; }
    }
}