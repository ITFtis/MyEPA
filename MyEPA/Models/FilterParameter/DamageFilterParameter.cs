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
        public List<int> CityIds { get; set; }
        public List<int> TownIds { get; set; }
        public List<int> DiasterIds { get; set; }
        public DateTime? ReportDay { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public FacilityDamageTypeEnum? Type { get; set; }

    }

}