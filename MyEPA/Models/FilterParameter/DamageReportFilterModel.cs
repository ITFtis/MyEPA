using MyEPA.Enums;
using System;

namespace MyEPA.Models.FilterParameter
{
    public class DamageReportFilterModel
    {
        public int DiasterId { get; set; }
        public int? CityId { get; set; }
        public DateTime? Date { get; set; }
        public int? AreaId { get; set; }
    }

    public class DamageTownReportFilterModel
    {
        public int DiasterId { get; set; }
        public int? CityId { get; set; }
    }
}