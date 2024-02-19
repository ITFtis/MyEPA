namespace MyEPA.Models
{
    public class Every8DResultModel
    {
        /// <summary>
        /// 發送後剩餘點數。負值代表發送失敗，系統無法處理該命令
        /// </summary>
        public decimal Credit { get; set; }
        /// <summary>
        /// 發送通數
        /// </summary>
        public decimal Sended { get; set; }
        /// <summary>
        /// 本次發送扣除點數
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// 無額度時發送的通數，當該值大於0而剩餘點數等於0時表示有部份的簡訊因無額度而無法被發送。
        /// </summary>
        public decimal Unsend { get; set; }
        /// <summary>
        /// 批次識別代碼。為一唯一識別碼，可藉由本識別碼查詢發送狀態。格式範例：220478cc-8506-49b2-93b7-2505f651c12e*/
        /// </summary>
        public string BatchId { get; set; }
        public bool IsAllSuccess { get; set; }
    }
}