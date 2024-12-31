using System.Collections.Generic;

namespace MyEPA.Models.FilterParameter
{
    public class UsersFilterParameter
    {
        public string Name { get; set; }
        public IEnumerable<int> PositionIds { get; set; }
        public IEnumerable<int> TownIds { get; set; }
        public IEnumerable<int> CityIds { get; set; }
        public IEnumerable<int> DutyIds { get; set; }
        public IEnumerable<int> DepartmentIds { get; set; }
        public IEnumerable<int> UserIds { get; set; }
        public IEnumerable<int> ContactManualDepartmentIds { get; set; }
        public IEnumerable<int> ContactManualDutys { get; set; }
        public string MainContacter { get; set; }
        public string HumanType { get; set; }
        /// <summary>
        /// n天以上未登入(0=>有未登入天數)
        /// </summary>
        public int? LoginRange { get; set; }
    }
}