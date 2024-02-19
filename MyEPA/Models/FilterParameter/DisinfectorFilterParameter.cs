using System.Collections.Generic;

namespace MyEPA.Models.FilterParameter
{
    public class DisinfectorFilterParameter
    {
        public IEnumerable<int> CityIds { get; set; }
        public IEnumerable<int> TownIds { get; set; }
    }
}