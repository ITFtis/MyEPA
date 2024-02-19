using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class ApplySubsidySumPriceModel
    {
        public int CityId { get; set; }
        public int TownId { get; set; }
        public decimal SumPrice { get; set; }
    }
}