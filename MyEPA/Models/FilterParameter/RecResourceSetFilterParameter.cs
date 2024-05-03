using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models.FilterParameter
{
    public class RecResourceSetFilterParameter
    {
        public List<int> RecResourceIdNeeds { get; set; }

        public List<int> CityIds { get; set; }
    }
}