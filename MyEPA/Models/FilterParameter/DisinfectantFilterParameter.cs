using MyEPA.Enums;
using System.Collections.Generic;

namespace MyEPA.Models.FilterParameter
{
    public class DisinfectantFilterParameter
    {
        public IEnumerable<int> CityIds { get; set; }
        public IEnumerable<int> TownIds { get; set; }

        public DrugStateEnum? DrugState { get; set; }
    }
}