using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models.SearchViewModel
{
    public class NoticeSearchViewModel
    {
        public string keyword { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DiasterId { get; set; }
    }
}