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

        /// <summary>
        /// (23筆)22個縣市+1個環境部
        /// </summary>
        public bool? IsCity23 { get; set; }
    }
}