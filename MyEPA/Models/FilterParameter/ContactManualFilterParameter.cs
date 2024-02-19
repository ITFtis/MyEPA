using MyEPA.Enums;
using System.Collections.Generic;

namespace MyEPA.Models.FilterParameter
{
    public class ContactManualFilterParameter
    {
        public List<ContactManualTypeEnum> Types { get; set; }
        public List<int> SourceIds { get; set; }
    }
}