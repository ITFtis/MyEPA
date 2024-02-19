using System;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class WaterEquipmentViewModel : WaterEquipmentModel
    {
        public string CityName { get; set; }
        [DisplayName("單位")]
        public string TownName { get; set; }
    }
    public class WaterEquipmentModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int DiasterId { get; set; }

        public int CityId { get; set; }
        public int TownId { get; set; }
        [DisplayName("淨水場名稱(加壓站)")]

        public string Name { get; set; }
        [DisplayName("天然災害受損情形")]

        public string Description { get; set; }
        [DisplayName("正常供水量(CMD)")]

        public decimal NormalAmount { get; set; }
        [DisplayName("減少供水量(CMD)")]

        public decimal AbnormalAmount { get; set; }
        [DisplayName("正常供水戶數")]

        public int NormalCount { get; set; }
        [DisplayName("影響供水戶數")]

        public int AbnormalCount { get; set; }
        [DisplayName("供水正常地區")]

        public string NormalArea { get; set; }
        [DisplayName("供水受影響地區")]

        public string AbnormalArea { get; set; }
        [DisplayName("預定完成搶修時間")]
        public DateTime DoneDate { get; set; }
        [DisplayName("備註")]
        public string Remark { get; set; }
        [DisplayName("填表日期")]
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
    }

}