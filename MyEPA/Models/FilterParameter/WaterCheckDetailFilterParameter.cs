using MyEPA.Enums;
using System.Collections.Generic;

namespace MyEPA.Models.FilterParameter
{
    public class WaterCheckDetailFilterParameter
    {
        public List<int> WaterCheckIds { get; set; }
        public List<int> CityIds { get; set; }

        public List<WaterCheckDetailRecheckEnum> Rechecks { get; set; }

        public List<string> UpdateUsers { get; set; }
    }
}