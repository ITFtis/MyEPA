using System;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class PhoneWorkModel
    {
        [AutoKey]
        public int Id { get; set; }
        [DisplayName("發話人")]

        public string Speaker { get; set; }
        [DisplayName("記錄人")]

        public string Recorder { get; set; }
        [DisplayName("辦理人")]

        public string Executor { get; set; }
        [DisplayName("紀錄(交辦)事項")]

        public string IssueDesc { get; set; }
        [DisplayName("辦理情形")]

        public string ProgressDesc { get; set; }
        [DisplayName("處理狀況")]

        public int Status { get; set; }
        [DisplayName("")]

        public int? TownId { get; set; }
        [DisplayName("備註")]

        public string Note { get; set; }
        [DisplayName("日期")]
        public DateTime PhoneTime { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string UpdateUser { get; set; }

        public DateTime UpdateDate { get; set; }
        public int DiasterId { get; set; }
    }
}