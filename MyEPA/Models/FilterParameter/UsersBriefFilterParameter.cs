using MyEPA.Enums;
using System.Collections.Generic;

namespace MyEPA.Models.FilterParameter
{
    public class UsersBriefFilterParameter
    {
        public IEnumerable<int> UserIds { get; set; }
        public IEnumerable<int> DepartmentIds { get; set; }
        public IEnumerable<int> CityIds { get; set; }
        public IEnumerable<int> TownIds { get; set; }
        public IEnumerable<int> DutyIds { get; set; }
        public string Duty { get; set; }
        public string MainContacter { get; set; }
        public string HumanType { get; set; }

        public string ISEnvironmentalProtectionAdministration { get; set; }
        public string ISEnvironmentalProtectionDepartment { get; set; }
        public string ISBook { get; set; }

        public IEnumerable<int> ContactManualDutys { get; set; }
    }
}