using MyEPA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models.SearchViewModel
{
    public class DetailStatisticsSearchViewModel
    {
        public WaterCheckTypeEnum? Type { get; set; }
        public int? WaterDivisionId { get; set; }
        public int? DiasterId { get; set; }
        public int? CityId { get; set; }
        public WaterCheckDetailRecheckEnum? Recheck { get; set; }

        public WaterCheckDetailStatusEnum? Status { get; set; }
    }
}