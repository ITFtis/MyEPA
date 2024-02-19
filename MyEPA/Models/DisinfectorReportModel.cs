using System;
using System.ComponentModel;
using MyEPA.Enums;
namespace MyEPA.Models
{
    public class DisinfectorReportModel
    {
        [DisplayName("消毒設備名稱")]
        public string DisinfectInstrument { get; set; }
        [DisplayName("機具數量")]
        public int Quantity { get; set; }
    }

    public class DisinfectorCityReportModel
    {
        [DisplayName("縣市")]
        public int CityId { get; set; }
        [DisplayName("縣市")]
        public string City { get; set; }
        [DisplayName("鄉鎮")]
        public int TownId { get; set; }
        [DisplayName("鄉鎮")]
        public string Town { get; set; }

        [DisplayName("用途")]
        public DisinfectorUseTypeEnum UseType { get; set; }
        [DisplayName("消毒設備名稱")]
        public string Name { get; set; }


        [DisplayName("數量")]
        public int Amount { get; set; }

       
        [DisplayName("資料更新日期")]
        public DateTime UpdateTime { get; set; }
    }
}