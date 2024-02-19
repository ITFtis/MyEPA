using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class ToiletLocationStatisticsModel
    {
        public int Id { get; set; }
        [DisplayName("縣市")]
        public string CityName { get; set; }
        [DisplayName("鄉鎮")]
        public string TownName { get; set; }
        [DisplayName("廁所位置")]
        public string Address { get; set; }
        [DisplayName("廁所數量")]
        public int ToiletQuantity { get; set; }
        [DisplayName("廁所型式")]
        public string ToiletType { get; set; }
        [DisplayName("租約起日期")]
        public DateTime StartDate { get; set; }
        [DisplayName("租約迄日期")]
        public DateTime EndDate { get; set; }
        [DisplayName("管理人")]
        public string ContactPerson { get; set; }
        [DisplayName("管理人聯絡方式")]
        public string ContactMethod { get; set; }
        [DisplayName("備註")]
        public string Note { get; set; }
        [DisplayName("最後清理日期")]
        public DateTime? LastCleanDate { get; set; }
    }
}