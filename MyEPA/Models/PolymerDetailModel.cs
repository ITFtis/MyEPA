using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class PolymerDetailModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int PolymerId { get; set; }
        [DisplayName("使用日期")]
        public DateTime UseDate { get; set; }
        [DisplayName("當日使用量(Kg)")]
        public decimal UseAmount { get; set; }
        [DisplayName("處理水量(M3)")]
        public decimal WaterAmount { get; set; }
        [DisplayName("藥劑添加劑量(mg/L)")]
        public decimal DrugAmount { get; set; }
        [DisplayName("原水濁度(NTU)")]
        public decimal Turbidity { get; set; }
        [DisplayName("申報前庫存量(Kg)")]
        public decimal Inventory { get; set; }
        [DisplayName("剩餘庫存藥劑量(Kg)")]
        public decimal OriginalInventory { get; set; }

        public DateTime CreateDate { get; set; }

        public string CreateUser { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateUser { get; set; }

    }
}