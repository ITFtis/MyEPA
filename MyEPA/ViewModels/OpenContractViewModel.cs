using MyEPA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.ViewModels
{
    public class OpenContractViewModel
    {
        public int Id { get; set; }
        /// <summary>
        /// 合約類型
        /// ResourceTypeEnum.cs
        /// </summary>
        public int ResourceTypeId { get; set; }

        [DisplayName("合約名稱")]
        public string Name { get; set; }

        [DisplayName("簽約日期")]
        public DateTime KeyInDate { get; set; }
        [DisplayName("合約起始")]
        public DateTime OContractDateBegin { get; set; }
        [DisplayName("合約截止")]
        public DateTime OContractDateEnd { get; set; }

        [DisplayName("合約廠商")]
        public string Fac { get; set; }

        [DisplayName("負責人")]
        public string Owner { get; set; }
        [DisplayName("聯絡電話")]
        public string TEL { get; set; }

        [DisplayName("行動電話")]
        public string MobileTEL { get; set; }

        //1是0否
        [DisplayName("跨縣市支援")]
        public bool? IsSupportCity { get; set; }

        public FileDataModel FileData { get; set; }
    }
}