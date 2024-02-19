namespace MyEPA.Models
{
    public class ApplyDisinfectionEquipmentDetailModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int ApplyDisinfectionEquipmentId { get; set; }

        /// <summary>
        /// 申請的物件名稱
        /// </summary>
        public string Item { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 天
        /// </summary>
        public int Days { get; set; }
    }
}