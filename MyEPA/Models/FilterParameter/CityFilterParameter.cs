using System.Collections.Generic;

namespace MyEPA.Models.FilterParameter
{
    public class CityFilterParameter
    {
        public List<int> CityIds { get; set; }
        public List<int> AreaIds { get; set; }
        public List<int> WaterDivisionIds { get; set; }
        public bool? IsCounty { get; set; }
        public string City { get; set; }
        /// <summary>
        /// CityTypeEnum.cs
        /// </summary>
        public List<int> Types { get; set; }
    }
}