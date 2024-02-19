using System;
using System.ComponentModel;

namespace MyEPA.ViewModels
{
    public class ContactManualOnDutyReportViewModel
    {
        [DisplayName("日期")]
        public string DateStr { get; set; }
        [DisplayName("星期")]
        public string Week { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; }
        [DisplayName("住宅電話")]
        public string HomeNumber { get; set; }
        [DisplayName("行動電話")]
        public string MobilePhone { get; set; }
    }
    public class ContactManualOnDutyViewModel
    {
        public int Id { get; set; }
        [DisplayName("日期")]
        public DateTime Date { get; set; }
        [DisplayName("星期")]
        public string Week { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; }
        [DisplayName("住宅電話")]
        public string HomeNumber { get; set; }
        [DisplayName("行動電話")]
        public string MobilePhone { get; set; }
        [DisplayName("排序")]
        public int Sort { get; set; }
    }
    public class ContactManualOnDutyCreateViewModel : ContactManualBaseViewModel
    {
        [DisplayName("日期")]
        public DateTime Date { get; set; }
    }
}