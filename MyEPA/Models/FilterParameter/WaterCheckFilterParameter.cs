using MyEPA.Enums;
using System;
using System.Collections.Generic;

namespace MyEPA.Models.FilterParameter
{
    public class WaterCheckFilterParameter
    {
        public List<int> DiasterIds { get; set; }

        public List<int> CityIds { get; set; }
        public List<WaterCheckTypeEnum> Types { get; set; }
        public DateTime? CheckDate { get; set; }
    }
}