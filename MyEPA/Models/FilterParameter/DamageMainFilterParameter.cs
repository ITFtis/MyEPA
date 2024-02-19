using MyEPA.Enums;
using System;
using System.Collections.Generic;

namespace MyEPA.Models.FilterParameter
{
    public class DamageMainFilterParameter
    {
        public List<int> CityIds { get; set; }
        public List<int> DiasterIds { get; set; }
    }
}