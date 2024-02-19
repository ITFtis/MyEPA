using System.Collections.Generic;

namespace MyEPA.Models.FilterParameter
{
    public class PestFilterParameter
    {
        public IEnumerable<int> CityIds { get; set; }
        public IEnumerable<int> TownIds { get; set; }
    }
}