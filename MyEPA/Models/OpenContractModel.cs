using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        [DisplayName("合約起始")]
        public DateTime OContractDateBegin { get; set; }
        [DisplayName("合約截止")]
        public DateTime OContractDateEnd { get; set; }
        [Required]
        [DisplayName("合約廠商")]
        public string Fac { get; set; }
        [Required]
        [DisplayName("負責人")]
        public string Owner { get; set; }
        [DisplayName("聯絡電話")]
        public string TEL { get; set; }
     
        [DisplayName("行動電話")]
        public string MobileTEL { get; set; }
        public int Status { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateUser { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateUser { get; set; }

        public int CityId { get; set; }

        public int TownId { get; set; }
    }

}