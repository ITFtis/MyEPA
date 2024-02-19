using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models.FilterParameter
{
    public class TownFilterParameter
    {
        public List<int> Ids { get; set; }
        public List<int> CityIds { get; set; }
        public bool? IsTown { get; set; }
        public string TownName { get; set; }
    }
}