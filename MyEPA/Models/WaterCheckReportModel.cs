using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class WaterCheckReportModel
    {
        public int CityId { get; set; }
        [DisplayName("單位名稱")]
        public string CityName { get; set; }
        public int TownId { get; set; }
        public string TownName { get; set; }
        
        /// <summary>
        /// 無法抽驗
        /// </summary>
        [DisplayName("無法抽驗")]
        public int CannotCount { get; set; }
        /// <summary>
        /// 無災情	
        /// </summary>
        [DisplayName("無災情")]
        public int NothingHappenedCount { get; set; }
        /// <summary>
        /// 抽驗數
        /// </summary>
        [DisplayName("抽驗數")]
        public int Count { get; set; }
        /// <summary>
        /// 合格數
        /// </summary>
        [DisplayName("合格數")]
        public int SuccessCount { get; set; }
       
        /// <summary>
        /// 未通過
        /// </summary>
        [DisplayName("未通過")]
        public int FailureCount { get; set; }
        [DisplayName("檢驗中")]
        public int TestingCount { get; set; }
        /// <summary>
        /// 負責人
        /// </summary>
        [DisplayName("通報負責人姓名")]
        
        public string Name { get; set; }
        /// <summary>
        /// 電話
        /// </summary>
        [DisplayName("通報負責人電話")]
        public string OfficePhone { get; set; }
        [DisplayName("通報時間")]
        public DateTime UpdateTime { get; set; }
    }
}