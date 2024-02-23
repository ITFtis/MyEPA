using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class ResourceSetModel
    {
        [AutoKey]
        public int Id { get; set; }

        /// <summary>
        /// 1.需求 2.調度
        /// </summary>
        [DisplayName("類型")]
        public int Type { get; set; }

        [DisplayName("對應災害")]
        public int DiasterId { get; set; }

        [DisplayName("縣市")]
        public string City { get; set; }

        [DisplayName("調度事由")]
        public string Reason { get; set; }

        /// <summary>
        /// 鍵入時間
        /// </summary>
        [DisplayName("鍵入時間")]
        public DateTime CreateDate { get; set; }

        [DisplayName("聯絡人")]
        public string ContactPerson { get; set; }

        [DisplayName("聯絡電話")]
        public string ContactPhone { get; set; }

        [DisplayName("項目(機具、車輛、消毒設備、藥品、人力)\t")]
        public string SupportItem { get; set; }

        [DisplayName("細項(規格)")]
        public string Spec { get; set; }

        [DisplayName("數量")]
        public string Quantity { get; set; }

        [DisplayName("單位")]
        public string Unit { get; set; }

        [DisplayName("需用日期")]
        public DateTime RequireDate { get; set; }
    }
}