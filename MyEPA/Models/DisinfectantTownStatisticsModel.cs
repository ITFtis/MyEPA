using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class DisinfectantTownStatisticsModel
    {
        public int CityId { get; set; }
        [DisplayName("縣市")]
        public string City { get; set; }
        public int TownId { get; set; }
        [DisplayName("鄉鎮")]
        public string Town { get; set; }
        [DisplayName("固態/液態")]
        public string DrugState { get; set; }
        [DisplayName("總量")]
        public decimal Amount { get; set; }
        [DisplayName("數量")]
        public int Count { get; set; }
        [DisplayName("更新日期")]
        public DateTime UpdateTime { get; set; }
        [DisplayName("有效期限")]
        public DateTime ServiceLife { get; set;}
    }

    public class DisinfectantStatisticsTownViewModel : DisinfectantStatisticsBaseViewModel
    {
        public int CityId { get; set; }
        [DisplayName("縣市")]
        public string City { get; set; }
        public int TownId { get; set; }
        [DisplayName("鄉鎮")]
        public string Town { get; set; }
    }
    public class DisinfectantStatisticsCityViewModel : DisinfectantStatisticsBaseViewModel
    {
        public int CityId { get; set; }
        [DisplayName("縣市")]
        public string City { get; set; }
    }

    public class DisinfectantStatisticsBaseViewModel
    {
        [DisplayName("固體總量(公斤)")]
        public decimal DisinfectantSolidAmount { get; set; }
        [DisplayName("液態總量(公升)")]
        public decimal DisinfectantLiquidAmount { get; set; }
        [DisplayName("資料筆數")]
        public int Count { get; set; }
        [DisplayName("更新日期")]
        public DateTime UpdateTime { get; set; }
        [DisplayName("有效期限")]
        public DateTime ServiceLife { get; set; }
    }
}