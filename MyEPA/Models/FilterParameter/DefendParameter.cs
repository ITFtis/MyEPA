using System.Collections.Generic;

namespace MyEPA.Models.FilterParameter
{
    public class DefendParameter
    {
        public List<int> CityIds { get; set; }

        public List<int> TownIds { get; set; }

        public List<int> DiasterIds { get; set; }
        /// <summary>
        /// 參考 DutyEnum.cs
        /// </summary>
        public List<int> DutyIds { get; set; }
        /// <summary>
        /// 參考 DefendStatusEnum.cs
        /// </summary>
        public List<int> Status { get; set; }
    }
}