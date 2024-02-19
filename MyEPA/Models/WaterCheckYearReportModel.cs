using System.ComponentModel;

namespace MyEPA.Models
{
    public class WaterCheckYearReportModel
    {
        public int DiasterId { get; set; }
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
    }
}