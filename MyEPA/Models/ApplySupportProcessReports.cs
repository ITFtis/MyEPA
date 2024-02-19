using MyEPA.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class ApplySupportProcessReport
    {
        public string ApplyType { get; set; }

        /// <summary>
        /// 紀錄 Id
        /// </summary>
        public int Id { get; set; }
        [DisplayName("縣市")]

        public string City { get; set; }
        [DisplayName("鄉鎮")]
        public string Town { get; set; }
        /// <summary>
        /// 請求日期
        /// </summary>
        [DisplayName("請求日期")]
        public DateTime CreateDate { get; set; }
        [DisplayName("需用日期")]
        public DateTime RequireDate { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        
        [DisplayName("請求說明")]
        public string EstimationMethodDescribe { get; set; }
        [DisplayName("核定")]
        public ApplyStatusEnum Status {get;set;}
        /// <summary>
        /// 環保局辦理情形
        /// </summary>
        public int EPAConfirmStatus { get; set; }
    }
}