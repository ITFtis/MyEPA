using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class OpenContractJoinDetailSearchModel
    {
        [DisplayName("縣市")]
        public string CityName { get; set; }
        [DisplayName("鄉鎮市")]
        public string TownName { get; set; }
        [DisplayName("合約種類")]
        public string TypeName { get; set; }
        [DisplayName("合約資料")]
        public string OpenContractName { get; set; }
        [DisplayName("項目")]
        public string Items { get; set; }
        [DisplayName("單位")]
        public string Unit { get; set; }
        [DisplayName("數量")]
        public string Count { get; set; }
        [DisplayName("單價")]
        public string Price { get; set; }
        [DisplayName("預算")]
        public decimal Budge { get; set; }
    }
}