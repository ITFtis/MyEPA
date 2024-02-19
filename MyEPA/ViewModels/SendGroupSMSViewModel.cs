using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.ViewModels
{
    public class SendGroupSMSViewModel
    {
        public int GroupId { get; set; }
        [DisplayName("群組名稱")]
        public string GroupName { get; set; }
        [DisplayName("標題")]
        public string Subject { get; set;}
        [DisplayName("訊息")]
        public string Message { get; set; }
    }
}