using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models.FilterParameter
{
    public class DiasterFilterParameter
    {
        public List<int> StartYears { get; set; }
        public bool? IsRunning { get; set; }
        public IEnumerable<int> Ids { get; internal set; }
    }
}