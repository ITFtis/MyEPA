using MyEPA.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyEPA.Models.BaseModels
{
    public class ApplyBaseModel
    {
        [AutoKey]
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
        /// 鄉鎮ID
        /// </summary>
        public int TownId { get; set; }

        /// <summary>
        /// 需求時間
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DisplayName("需求時間")]
        [Required(ErrorMessage = "請選擇日期")]
        [DataType(DataType.Date)]
        public DateTime RequireDate { get; set; }

        /// <summary>
        /// 估算方法描述
        /// </summary>
        [DisplayName("估算方法(限1000字內)")]
        [MaxLength(1000)]
        public string EstimationMethodDescribe { get; set; }

        /// <summary>
        /// 照片描述
        /// </summary>
        [DisplayName("照片描述")]
        public string PhotoDescribe { get; set; }

        /// <summary>
        /// 聯絡人
        /// </summary>
        [DisplayName("聯絡人姓名")]
        public string ContactPerson { get; set; }

        /// <summary>
        /// 聯絡電話
        /// </summary>
        [DisplayName("聯絡人電話")]
        public string ContactPhone { get; set; }

        /// <summary>
        /// 聯絡手機
        /// </summary>
        [DisplayName("聯絡人手機")]
        public string ContactMobilePhone { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public ApplyStatusEnum Status { get; set; } = ApplyStatusEnum.Pending;

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 建立者名稱
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 建立者名稱
        /// </summary>
        public string UpdateUser { get; set; }

        /// <summary>
        /// 環保局辦理情形
        /// </summary>
        public ApplyStatusEnum? EPBConfirmStatus { get; set; } = ApplyStatusEnum.Pending;

        /// <summary>
        /// 環保局辦理說明
        /// </summary>
        public string EPBConfirmDescribe { get; set; }

        /// <summary>
        /// 環保局辦理更新時間
        /// </summary>
        public DateTime? EPBConfirmUpdateTime { get; set; }

        /// <summary>
        /// 環保署辦理情形
        /// </summary>
        public ApplyStatusEnum? EPAConfirmStatus { get; set; }

        /// <summary>
        /// 環保署辦理說明
        /// </summary>
        public string EPAConfirmDescribe { get; set; }

        /// <summary>
        /// 環保署辦理更新時間
        /// </summary>
        public DateTime? EPAConfirmUpdateTime { get; set; }
       

        /// <summary>
        /// 申請狀態
        /// </summary>
        public int? PostStatus { get; set; }

        /// <summary>
        /// 是否轉到環保署
        /// </summary>
        public bool IsToEpa { get; set; }
    }
}