using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models.FilterParameter
{
    public class UserLoginLogFilterParameter
    {
        public string UserName { get; set; }
        public int? Type { get; set; }
        public bool? IsOver { get; set; }
    }
}