using MyEPA.Enums;
using System.Collections.Generic;

namespace MyEPA.Models.FilterParameter
{
    public class UsersJoinPositionFilterParameter
    {
        /// <summary>
        /// 最小職等不包含
        /// 參考 PositionRankEnum.cs
        /// </summary>
        public int? MinRank { get; set; }
        /// <summary>
        /// 最大職等不包含
        /// 參考 PositionRankEnum.cs
        /// </summary>
        public int? MaxRank { get; set; }
        /// <summary>
        /// 最小職等包含
        /// 參考 PositionRankEnum.cs
        /// </summary>
        public int? MinRankContain { get; set; }
        /// <summary>
        /// 最大職等包含
        /// 參考 PositionRankEnum.cs
        /// </summary>
        public int? MaxRankContain { get; set; }

        public IEnumerable<int> DepartmentIds { get; set; }
    }
}