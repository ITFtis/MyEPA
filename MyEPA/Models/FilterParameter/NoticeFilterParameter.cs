using System;
using System.Collections.Generic;

namespace MyEPA.Models.FilterParameter
{
    public class NoticeFilterParameter
    {
        public List<int> DiasterIds { get; set; }
        public List<int> Ids { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Keyword { get; set; }
    }
}