using MyEPA.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyEPA.Models
{
    public class ApplySupportReportDetailModel
    {
        /// <summary>
        /// 對應支援 Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 災害ID
        /// </summary>
        public int DiasterId { get; set; }

        /// <summary>
        /// 城市 ID
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// 城市名稱
        /// </summary>
        [DisplayName("縣市")]
        public string CityName { get; set; }

        /// <summary>
        /// 鄉鎮ID
        /// </summary>
        public int TownId { get; set; }

        /// <summary>
        /// 鄉鎮名稱
        /// </summary>
        [DisplayName("鄉鎮")]
        public string TownName { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [DisplayName("請求日期")]
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 需求時間
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DisplayName("需用日期")]
        [Required(ErrorMessage = "請選擇日期")]
        [DataType(DataType.Date)]
        public DateTime RequireDate { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 數量, 顯示用
        /// </summary>
        [DisplayName("數量")]
        public string QuantityString { get; set; }
        /// <summary>
        /// 估算方法描述
        /// </summary>
        [DisplayName("請求說明")]
        [MaxLength(1000)]
        public string EstimationMethodDescribe { get; set; }
        /// <summary>
        /// 聯絡人
        /// </summary>
        public string ContactPerson { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        public ApplyStatusEnum EPBConfirmStatus { get; set; }
        [DisplayName("環保局辦理情形")]
        public string EPBConfirmDescribe { get; set; }
        [DisplayName("環保局辦理日期")]
        public DateTime? EPBConfirmUpdateTime { get; set; }
        /// <summary>
        /// 是否轉到環保署
        /// </summary>
        [DisplayName("轉呈環保署")]
        public bool IsToEpa { get; set; }
        /// <summary>
        /// 狀態
        /// </summary>
        [DisplayName("核定")]
        public ApplyStatusEnum Status { get; set; }
        
        
        /// <summary>
        /// 狀態
        /// </summary>
        public ApplyStatusEnum EPAConfirmStatus { get; set; }
        [DisplayName("本署辦理情形")]
        public string EPAConfirmDescribe { get; set; }
        [DisplayName("環保署辦理日期")]
        public DateTime? EPAConfirmUpdateTime { get; set; }
        /// <summary>
        /// 通報人名稱
        /// </summary>
        [DisplayName("通報人員")]
        public string CreateUser { get; set; }
        /// <summary>
        /// 聯絡電話
        /// </summary>
        [DisplayName("聯絡電話")]
        public string ContactPhone { get; set; }
    }
}