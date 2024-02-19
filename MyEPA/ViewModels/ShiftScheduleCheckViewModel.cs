using System;
using System.ComponentModel;

namespace MyEPA.ViewModels
{
    public class ShiftScheduleCheckViewModel
    {
        [DisplayName("工作人員")]
        public string Name { get; set; }
        [DisplayName("簽到")]
        public DateTime? CheckinTime { get; set; }
        [DisplayName("簽退")]

        public DateTime? Checkout { get; set; }
        [DisplayName("行動電話")]
        public string MobilePhone { get; set; }
    }
}