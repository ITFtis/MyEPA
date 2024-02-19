using MyEPA.Enums;
using System.Collections.Generic;

namespace MyEPA.Models.FilterParameter
{
    public class ApplyBaseFilterParameter
    {
        public int? Id { get; set; }
        public string UserName { get; set; }
        public IEnumerable<int> DiasterIds { get; set; }
        public IEnumerable<int> CityIds { get; set; }
        public IEnumerable<int> TownIds { get; set; }
        public ApplyStatusEnum? Status { get; set; }

        /// <summary>
        /// 不包含的 StatusEnum
        /// </summary>
        public ApplyStatusEnum? NotStatus { get; set; }


        /// <summary>
        /// 環保局辦理狀態
        /// </summary>
        public List<ApplyStatusEnum> EPBConfirmStatus { get; set; }

        /// <summary>
        /// 環保署辦理狀態
        /// </summary>
        public List<ApplyStatusEnum> EPAConfirmStatus { get; set; }

        /// <summary>
        /// 是否轉送環保署
        /// </summary>
        public bool? IsToEpa { get; set; }
    }
}