using System.Collections.Generic;

namespace MyEPA.Models.FilterParameter
{
    public class DepartmentParameter
    {
        public IEnumerable<int> Ids { get; set; }

        public string Name { get; set; }
    }
}