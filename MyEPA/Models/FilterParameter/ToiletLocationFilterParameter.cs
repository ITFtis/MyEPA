using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models.FilterParameter
{
    public class ToiletLocationFilterParameter
    {
        public List<int> DiasterIds { get; set; }
        public List<int> CityIds { get; set; }
        public List<int> TownIds { get; set; }
        public List<int> ManagementTownIds { get; set; }
    }
}