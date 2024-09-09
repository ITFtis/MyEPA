using MyEPA.Enums;
using System;
using System.Collections.Generic;

namespace MyEPA.Models.FilterParameter
{
    public class DamageConfirmListFilterParameter
    {
        public DamageFilterParameter DamageFilterParameter { get; set; }
        public DiasterModel Diaster{ get; set; }
    }
    public class DamageFilterParameter
    {
        public List<int> Ids { get; set; }
        public int? AreaId { get; set; }
        public List<int> CityIds { get; set; }
        public List<int> TownIds { get; set; }
        public int? TownId { get; set; }
        public List<int> DiasterIds { get; set; }
        public DateTime? ReportDay { get; set; }
        public DateTime? CleanDay { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? CleanStartTime { get; set; }
        public DateTime? CleanEndTime { get; set; }
        public FacilityDamageTypeEnum? Type { get; set; }

        /// <summary>
        /// 資料類型1.開災情通報2.環境清理
        /// </summary>
        public int? HType { get; set; }
    }

}