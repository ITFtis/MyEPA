using System.ComponentModel;

namespace MyEPA.Models
{
    public class ToiletReportModel
    {
        [DisplayName("部門")]
        public string ContactUnit { get; set; }
        [DisplayName("縣市")]
        public string City { get; set; }
        [DisplayName("鄉鎮名")]
        public string Town { get; set; }
        [DisplayName("購置年份")]
        public string ROCyear { get; set; }
        [DisplayName("數量")]
        public int Count { get; set; }
        [DisplayName("每座便器數")]
        public decimal Amount { get; set; }
        [DisplayName("資料更新日期")]
        public string UpdateTime { get; set; }
    }
}