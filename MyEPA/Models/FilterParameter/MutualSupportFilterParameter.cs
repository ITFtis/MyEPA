using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class MutualSupportFilterParameter
    {
        public List<int> ResourceTypeIds { get; set; }
        public int? Year { get; set; }
        public List<int> SupportType { get; set; }
        public string Section { get; set; }
        public string AcceptSection { get; set; }
    }
}