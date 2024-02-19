using System.Collections.Generic;

namespace MyEPA.Models.FilterParameter
{
    public class DisinfectorFilterCityReportParameter
    {
        public List<int> CityIds { get; set; }
        public List<int> TownIds { get; set; }
        public List<int> UseTypes { get; set; }
        public string Name { get; set; }
        public int? Year { get; set; }
    }
}