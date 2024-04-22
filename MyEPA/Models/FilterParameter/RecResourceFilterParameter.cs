using MyEPA.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models.FilterParameter
{
    public class RecResourceFilterParameter
    {
        public List<int> DiasterIds { get; set; }

        public List<int> CityIds { get; set; }
        ////public List<WaterCheckTypeEnum> Types { get; set; }
        ////public DateTime? CheckDate { get; set; }
    }
}