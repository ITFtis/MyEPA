using System;

namespace MyEPA
{
    public class ContactManualGroupAttribute : Attribute
    {
        public string GroupName { get; }
        public ContactManualGroupAttribute(string departmentName)
        {
            GroupName = departmentName;
        }
    }
}