namespace MyEPA.Models
{
    public class ApplyOtherDetailModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int ApplyOtherId { get; set; }
        /// <summary>
        /// 申請的物件名稱
        /// </summary>
        public string Item { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 單位
        /// </summary>
        public string Unit { get; set; }
    }
}