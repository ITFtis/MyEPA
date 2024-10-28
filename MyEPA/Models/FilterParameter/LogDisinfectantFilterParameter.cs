using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models.FilterParameter
{
    public class LogDisinfectantFilterParameter
    {
        public List<int> DiasterIds { get; set; }

        public List<int> CityIds { get; set; }
    }
}