using System.Collections.Generic;
using System.ComponentModel;

namespace MyEPA.ViewModels
{
    public class WaterCheckStatisticsViewModel
    {
        public int WaterDivisionId { get; set; }
        [DisplayName("供水單位")]
        public string WaterDivision { get; set; }
        /// <summary>
        /// 自來水
        /// </summary>
        [DisplayName("自來水單位")]
        public WaterCheckRecheckCountViewModel Water { get; set; }
        /// <summary>
        /// 環保單位
        /// </summary>
        [DisplayName("環保單位")]
        public List<WaterCheckRecheckCountEPViewModel> EPs { get; set; }
    }
    public class WaterCheckRecheckCountViewModel
    {
        [DisplayName("抽驗件數")]
        public int Count { get; set; }
        [DisplayName("不合格件數")]
        public int DisqualifiedCount { get; set; }
    }
    public class WaterCheckRecheckCountEPViewModel : WaterCheckRecheckCountViewModel
    {
        public int CityId { get; set; }
        [DisplayName("單位別")]
        public string City { get; set; }
    }
}