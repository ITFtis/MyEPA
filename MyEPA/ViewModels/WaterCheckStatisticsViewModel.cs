using System.Collections.Generic;
using System.ComponentModel;

namespace MyEPA.ViewModels
{
    public class WaterCheckStatisticsViewModel
    {
        public int WaterDivisionId { get; set; }
        [DisplayName("供水單位")]
        public string WaterDivision { get; set; }

        public int CityId { get; set; }
        [DisplayName("單位別")]
        public string City { get; set; }

        //自來水
        [DisplayName("抽驗件數")]
        public int WaterCount { get; set; }
        [DisplayName("不合格件數")]
        public int WaterDisqualifiedCount { get; set; }

        // 環保單位
        [DisplayName("抽驗件數")]
        public int EPsCount { get; set; }
        [DisplayName("不合格件數")]
        public int EPsDisqualifiedCount { get; set; }
    }

    public class WaterCheckStatisticsEasyViewModel
    {
        public int CityId { get; set; }

        [DisplayName("單位別")]
        public string City { get; set; }

        [DisplayName("抽驗件數")]
        public int Count { get; set; }

        [DisplayName("不合格件數")]
        public int DisqualifiedCount { get; set; }

        [DisplayName("不合格淨水廠")]
        public string DisqualifiedAddress { get; set; }

        [DisplayName("合格件數")]
        public int SuccessCount { get; set; }

        [DisplayName("檢驗中件數")]
        public int TestingCount { get; set; }
    }
}