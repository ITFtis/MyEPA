using System;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class Meeting_SignViewModel : Meeting_SignModel
    {
        [DisplayName("已報名人數")]
        public int Attendance { get; set; }
    }
    public class Meeting_SignModel
    {
        [AutoKey]
        public int Meeting_ID { get; set; }
        [DisplayName("會議名稱")]
        public string Meeting_name { get; set; }
        [DisplayName("會議時間")]
        public DateTime Meeting_Datetime { get; set; }
        [DisplayName("會議地點")]
        public string Meeting_Address { get; set; }
        [DisplayName("會議議程")]
        public string Meeting_Issue { get; set; }
        [DisplayName("會議備註")]
        public string Meeting_Memo { get; set; }
        [DisplayName("開始報名時間")]
        public DateTime? Meeting_Sign_BeginTime { get; set; }
        [DisplayName("結束報名時間")]
        public DateTime? Meeting_Sign_EndTime { get; set; }
        [DisplayName("餐點供應")]
        public string Meeting_Food { get; set; }
        [DisplayName("餐點供應開始")]
        public DateTime? Meeting_Food_Begin { get; set; }
        [DisplayName("餐點供應結束")]
        public DateTime? Meeting_Food_End { get; set; }

        [DisplayName("接受報名人數")]
        public int MaximumAttendance { get; set; }
        [DisplayName("交通選項")]
        public int? Meeting_Traffic { get; set; }
        public int? Meeting_people_Sum { get; set; }
        [DisplayName("新增時間")]
        public DateTime? Meeting_Keyin_date { get; set; }
        [DisplayName("新增人")]
        public string Meeting_Keyin_name { get; set; }
        public string del { get; set; }

    }
}