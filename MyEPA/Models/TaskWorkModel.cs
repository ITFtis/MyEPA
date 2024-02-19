using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class TaskWorkModel
    {
        [AutoKey]
        public int Id { get; set; }
        [DisplayName("辦理人")]
        public string Executor { get; set; }
        [DisplayName("指派事項")]
        public string IssueDesc { get; set; }
        [DisplayName("執行狀況")]
        public string ProgressDesc { get; set; }
        [DisplayName("處理狀況")]
        public int Status { get; set; }

        public int? TownId { get; set; }
        [DisplayName("備註")]
        public string Note { get; set; }
        [DisplayName("交辦日期")]
        public DateTime PhoneTime { get; set;}

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public string UpdateUser { get; set; }

        public DateTime UpdateDate { get; set; }
        [DisplayName("完成時間")]
        public DateTime? CompleteTime { get; set; }
    }
}