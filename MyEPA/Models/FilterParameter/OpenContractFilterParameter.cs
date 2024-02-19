using System.Collections.Generic;

namespace MyEPA.Models
{
    public class OpenContractFilterParameter
    {
        public List<int> CityIds { get; set; }
        public List<int> ResourceTypeIds { get; set; }
        public int? Year { get; set; }
        public List<int> TownIds { get; set; }
        public bool? IsEffective { get; internal set; }
    }
}