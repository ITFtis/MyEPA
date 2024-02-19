using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models.FilterParameter
{
    public class UserGroupMappFilterParameter
    {
        public List<int> GroupIds { get; set; }
        public List<int> UserIds { get; set; }
    }
}