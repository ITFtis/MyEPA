using System.Collections.Generic;

namespace MyEPA.Models.FilterParameter
{
    public class UsersInfoFilterParameter
    {
        public IEnumerable<int> UserIds { get; set; }
        public IEnumerable<int> DepartmentIds { get; set; }
        public IEnumerable<int> DutyIds { get; set; }
        public IEnumerable<int> CityIds { get; set; }
        public IEnumerable<int> TownIds { get; set; }
    }
}