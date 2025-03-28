﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Web;

namespace MyEPA.Models
{
    public class OpenContractModel
    {
        [AutoKey]
        public int Id { get; set; }
        /// <summary>
        /// 合約類型
        /// ResourceTypeEnum.cs
        /// </summary>
        public int ResourceTypeId { get; set; }
        [Required]
        [DisplayName("合約名稱")]
        public string Name { get; set; }
        [Required]
        [DisplayName("簽約日期")]
        public DateTime KeyInDate { get; set; }
        [Required]
        [DisplayName("合約起始")]
        public DateTime OContractDateBegin { get; set; }
        [Required]
        [DisplayName("合約截止")]
        public DateTime OContractDateEnd { get; set; }
        [Required]
        [DisplayName("合約廠商")]
        public string Fac { get; set; }
        [Required]
        [DisplayName("負責人")]
        public string Owner { get; set; }
        [Required]
        [DisplayName("聯絡電話")]
        public string TEL { get; set; }

        //1是0否
        [DisplayName("跨縣市支援")]
        public bool? IsSupportCity { get; set; }

        [Required]
        [DisplayName("行動電話")]
        public string MobileTEL { get; set; }
        
        //-1:Copy主約未修改,0:一般資料
        public int Status { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateUser { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateUser { get; set; }

        public int CityId { get; set; }

        public int TownId { get; set; }
    }

    public class OpenContractCountModel : OpenContractModel
    {
        /// <summary>
        /// 縣市
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 鄉鎮
        /// </summary>
        public string TownName { get; set; }

        /// <summary>
        /// 細目數量
        /// </summary>
        public int? DetailCount { get; set; }

        /// <summary>
        /// 合約種類
        /// </summary>
        public string ResourceTypeName { get; set; }

        /// <summary>
        /// 是否有編輯權限
        /// </summary>
        public bool? CanEdit { get; set; }
    }

    /// <summary>
    /// 下一步Model
    /// </summary>
    public class OpenContractNextModel : OpenContractModel
    {
        public Dictionary<string, List<HttpPostedFileBase>> Files { get; set; }
    }
}