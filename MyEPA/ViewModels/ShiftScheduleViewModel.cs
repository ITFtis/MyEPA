using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.ViewModels
{
    public class ShiftScheduleViewModel
    {
        public int Id { get; set; }
        public int DiasterId { get; set; }
        public DateTime Date { get; set; }
        
        [DisplayName("早晚班")]
        public bool IsNight { get; set; }
        public int? LeaderUserId { get; set; }
        [DisplayName("人員1")]
        public string LeaderName { get; set; }
        [DisplayName("人員1電話")]
        public string LeaderUserPhone { get; set; }
        public int? WorkerUserId { get; set; }
        [DisplayName("人員2")]
        public string WorkerName { get; set; }
        [DisplayName("人員2電話")]
        public string WorkerUserPhone { get; set; }

        public int? ManagerUserId { get; set; }
        [DisplayName("科長以上")]
        public string ManagerName { get; internal set; }
        [DisplayName("科長以上電話")]
        public string ManagerUserPhone { get; internal set; }
    }
    public class TeamShiftScheduleViewModel
    {
        public int Id { get; set; }
        public int DiasterId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? LeaderUserId { get; set; }
        [DisplayName("人員1")]
        public string LeaderName { get; set; }
        [DisplayName("人員1電話")]
        public string LeaderUserPhone { get; set; }
    }

    public class MainShiftScheduleViewModel
    {
        public int Id { get; set; }
        public int DiasterId { get; set;}

        public DateTime Date { get; set; }

        [DisplayName("日/夜")]
        public bool IsNight { get; set; }
        [DisplayName("處室名稱")]
        public int? LeaderDepartmentId { get; set; }
        [DisplayName("專責人員")]
        public int? LeaderUserId { get; set; }
        [DisplayName("電話")]
        public string LeaderUserPhone { get; set; }
        [DisplayName("處室名稱")]
        public int? WorkerDepartmentId { get; set; }
        [DisplayName("幕僚人員")]
        public int? WorkerUserId { get; set; }
        [DisplayName("電話")]
        public string WorkerUserPhone { get; set; }
    }
}