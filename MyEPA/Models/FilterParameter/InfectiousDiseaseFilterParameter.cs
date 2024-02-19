using System;
using System.Collections.Generic;

namespace MyEPA.Models.FilterParameter
{
    public class InfectiousDiseaseFilterParameter
    {
        public List<int> CityIds { get; set; }

        public List<int> TownIds { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}