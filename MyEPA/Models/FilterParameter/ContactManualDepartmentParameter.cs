using System.Collections.Generic;

namespace MyEPA.Models.FilterParameter
{
    public class ContactManualDepartmentParameter
    {
        public IEnumerable<int> SourceIds { get; set; }
        public IEnumerable<int> Ids { get; set; }
        public IEnumerable<int> Types { get; set; }
        public string Name { get; set; }
    }
}