using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models.FilterParameter
{
    public class BaseFilterParameter
    {
        public IEnumerable<int> CityIds { get; set; }
        public IEnumerable<int> TownIds { get; set; }
    }

    
}