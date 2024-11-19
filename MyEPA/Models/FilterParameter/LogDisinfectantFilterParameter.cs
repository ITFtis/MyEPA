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

        //1.高於閾值2.低於閾值
        public int? Ct { get; set; }

        //有效天數
        public int? ServiceLifeDay { get; set; }
    }
}