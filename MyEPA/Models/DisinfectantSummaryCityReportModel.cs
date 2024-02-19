
using MyEPA.EPA.Attribute;
using System;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class DisinfectantSummaryTownReportModel : DisinfectantSummaryCityReportModel
    {
        [DisplayName("鄉鎮")]
        public string Town { get; set; }
        public int TownId { get; set; }
    }
    public class DisinfectantSummaryCityReportModel
    {
        public int Sort { get; set; }
        public int CityId { get; set; }
        [DisplayName("縣市")]
        public string City { get; set; }
        [DisplayName("環境消毒")]
        [DataTableSum]
        public decimal EnvironmentCount { get; set; }
        [DisplayName("登革熱")]
        [DataTableSum]
        public decimal DengueCount { get; set; }
        [DisplayName("紅火蟻")]
        [DataTableSum]
        public decimal RIFACount { get; set; }
        [DisplayName("荔枝椿象")]
        [DataTableSum]
        public decimal TessaratomaPapillosaDruryCount { get; set; }
        [DisplayName("其他")]
        [DataTableSum]
        public decimal OtherCount { get; set; }
        [DisplayName("最新更新時間")]
        public DateTime? UpdateTime { get; set; }
        [DisplayName("最新確認時間")]
        public DateTime? ConfirmTime { get; set; }
    }    
}