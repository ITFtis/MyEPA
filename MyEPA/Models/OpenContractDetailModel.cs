using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyEPA.Models
{
    public class OpenContractDetailModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int OpenContractId { get; set; }
        [Required]
        [DisplayName("項目")]
        public string Items { get; set; }
        [Required]
        [DisplayName("單位")]
        public string Unit { get; set; }
        [Required]
        [DisplayName("數量")]
        public string Count { get; set; }
        [DisplayName("價錢")]
        [Required]
        public string Price { get; set; }
        [Required]
        [DisplayName("預算")]
        
        public decimal Budge { get; set; }
        [Required]
        [DisplayName("項目類別")]
        
        public int? ItemCategoryId { get; set; }

        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateUser { get; set; }

        public int Status { get; set; }

    }

}