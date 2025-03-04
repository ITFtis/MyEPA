using MyEPA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.ViewModels
{
    public class OpenContractDetailViewModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int OpenContractId { get; set; }

        [DisplayName("項目")]

        public string Items { get; set; }
        [DisplayName("單位")]
        public string Unit { get; set; }
        [DisplayName("數量")]
        public string Count { get; set; }
        [DisplayName("價錢")]
        public string Price { get; set; }
        [DisplayName("預算")]
        public double Budge { get; set; }
        [DisplayName("項目類別")]

        public int? ItemCategoryId { get; set; }

        [DisplayName("項目類別")]

        public string ItemCategory { get; set; }

        public int Status { get; set; }

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
    }
}