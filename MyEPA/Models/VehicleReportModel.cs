using System.ComponentModel;

namespace MyEPA.Models
{
    public class VehicleReportModel
    {
        [DisplayName("車輛類別")]
        public string VehicleType { get; set; }
        [DisplayName("車輛數量")]
        public int VehicleQuantity { get; set; }
        /*
        [DisplayName("總載重(噸)")]
        public decimal Load { get; set; }
        [DisplayName("總馬力(HP)")]
        public decimal EnginePower { get; set; }*/
    }
}