using MyEPA.Enums;
using System;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class DefendReportModel
    {
        public int CityId { get; set; }
        public int? TownId { get; set; }
        [DisplayName("縣市")]
        public string CityName { get; set; }
        [DisplayName("督察大隊")]
        public string AreaName { get;set; }
        [DisplayName("大隊確認/縣市通報情形")]
        public DefendStatusEnum Status { get; set; }
        [DisplayName("已確認")]
        public int ConfirmCount { get; set; }
        [DisplayName("未確認")]
        public int WaitingCount { get; set; }
        [DisplayName("未通報")]
        public int UnNotificationCount { get; set; }
        [DisplayName("通報負責人姓名")]
        public string UserName { get; set; }
        [DisplayName("通報負責人電話")]
        public string OfficePhone { get; set; }
        [DisplayName("縣市通報時間")]
        public DateTime? CreateDate { get; set; }
        [DisplayName("縣市更新時間")]
        public DateTime? UpdateDate { get; set; }
        [DisplayName("區大隊確認時間")]
        public DateTime? ConfirmTime { get; set; }
    }
    public class DefendTownReportModel
    {
        public int TownId { get; set; }
        [DisplayName("鄉鎮")]
        public string TownName { get; set; }
        [DisplayName("狀態")]
        public DefendStatusEnum Status { get; set; }
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