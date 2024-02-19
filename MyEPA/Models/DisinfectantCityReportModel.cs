using MyEPA.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class DisinfectantCityReportModel
    {
        public int TownId { get; set; }
        [DisplayName("鄉鎮市")]
        public string Town { get; set; }
        public int CityId { get; set; }
        [DisplayName("縣市")]
        public string City { get; set; }
        [DisplayName("用途")]
        public DisinfectantUseTypeEnum UseType { get; set; }
        [DisplayName("名稱")]
        public string DrugName { get; set; }
        [DisplayName("類別")]
        public string DrugType { get; set; }

        [DisplayName("狀態")]
        public string DrugState { get; set; }
        [DisplayName("數量(公升/公斤)")]
        public decimal Amount { get; set; }
        [DisplayName("資料更新日期")]
        public DateTime UpdateTime { get; set; }







        [DisplayName("有效成分-1")]
        public string ActiveIngredients1 { get; set; }
        [DisplayName("有效成分-2")]
        public string ActiveIngredients2 { get; set; }

    }
}