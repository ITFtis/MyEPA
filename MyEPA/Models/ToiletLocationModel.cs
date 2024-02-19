using System;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class ToiletLocationModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int DiasterId { get; set; }

        public int CityId { get; set; }

        public int ManagementTownId { get; set; }

        public int TownId { get; set; }

        public string Address { get; set; }

        public decimal Xpos { get; set; }

        public decimal Ypos { get; set; }
        [DisplayName("廁所數量(座)")]

        public int ToiletQuantity { get; set; }
        [DisplayName("廁所型式")]

        public string ToiletType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        [DisplayName("管理人")]

        public string ContactPerson { get; set; }

        public string ContactMethod { get; set; }

        public string Note { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}