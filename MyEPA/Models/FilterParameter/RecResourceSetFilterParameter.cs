using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Models.FilterParameter
{
    public class RecResourceSetFilterParameter
    {
        [DisplayName("對應調度需求Id 需求")]
        public List<int> RecResourceIdNeeds { get; set; }

        [DisplayName("對應調度需求Id 支援")]
        public List<int> RecResourceIdHelps { get; set; }

        public List<int> CityIds { get; set; }
    }
}