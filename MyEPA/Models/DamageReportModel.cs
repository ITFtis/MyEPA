using MyEPA.Enums;
using System;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class DamageReportModel
    {
        public int CityId { get; set; }
        [DisplayName("縣市")]
        public string CityName { get; set; }
        [DisplayName("督察大隊")]
        public string AreaName { get; set; }
        [DisplayName("督察大隊未確認")]
        public int TeamWaitingCount { get; set; }
        [DisplayName("督察大隊已確認")]
        public int TeamConfirmCount { get; set; }
        [DisplayName("縣市未確認數")]
        public int WaitingCount { get; set; }
        [DisplayName("鄉鎮市通報數")]
        public int ConfirmCount { get; set; }
        [DisplayName("縣市通報負責人姓名")]
        public string UserName { get; set; }
        [DisplayName("縣市通報負責人電話")]
        public string OfficePhone { get; set; }
        [DisplayName("縣市通報時間")]
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        [DisplayName("環保局確認時間")]
        public DateTime? ConfirmTime { get; set; }
        
        [DisplayName("區大隊確認時間")]
        public DateTime? TeamConfirmTime { get; set; }
        [DisplayName("是否結案")]
        public bool IsDone { get; set; }
    }
    public class DamageTownReportModel
    {
        public int TownId { get; set; }
        [DisplayName("鄉鎮")]
        public string TownName { get; set; }
        [DisplayName("狀態")]
        public DamageStatusEnum Status { get; set; }
        [DisplayName("通報負責人姓名")]
        public string UserName { get; set; }
        [DisplayName("通報負責人電話")]
        public string OfficePhone { get; set; }
        [DisplayName("通報時間")]
        public DateTime? CreateDate { get; set; }
        [DisplayName("更新時間")]
        public DateTime? UpdateDate { get; set; }
        [DisplayName("縣市確認時間")]
        public DateTime? ConfirmTime { get; set; }
    }
}