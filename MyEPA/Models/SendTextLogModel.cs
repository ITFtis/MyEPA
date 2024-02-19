using MyEPA.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class SendTextLogModel
    {
        [AutoKey]
        public int Id { get; set; }
        [DisplayName("簡訊主旨")]
        public string Topic { get; set; }
        [DisplayName("內容")]
        public string Message { get; set; }
        [DisplayName("發送時間")]
        public DateTime CreateDate { get; set; }
    }

    public class SendTextLogDetailModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int SendTextLogId { get; set; }
        [DisplayName("手機號碼")]
        public string PhoneNumber { get; set; }
        [DisplayName("發送時間")]
        public DateTime SendTime { get; set; }
        [DisplayName("是否成功")]
        public bool IsSuccess { get; set; }
        [DisplayName("發送失敗原因")]
        public string ResultMessage { get; set; }
        /// <summary>
        /// every8D 發送碼
        /// </summary>
        public string BatchId { get; set; }
        /// <summary>
        /// every8D Status
        /// </summary>
        [DisplayName("狀態")]
        public SendTextLogDetailStatusEnum Status { get; set; }
    }
}