using MyEPA.Enums;
using System.Collections.Generic;

namespace MyEPA.Models.FilterParameter
{
    public class ContactManualRoleFilterParameter
    {
        public List<int> Ids { get; set; }

        public string Name { get; set; }
        public List<ContactManualRoleTypeEnum> Types { get; set; }
    }
}