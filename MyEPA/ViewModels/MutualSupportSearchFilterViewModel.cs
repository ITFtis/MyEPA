using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.ViewModels
{
    public class MutualSupportSearchFilterViewModel
    {
        public int? Year { get; set; }
        public string AcceptSection { get; set; }
        public string Section { get; set; }
        public int? ResourceTypeId { get; set; }
        public int? SupportType { get; set; }
    }
}