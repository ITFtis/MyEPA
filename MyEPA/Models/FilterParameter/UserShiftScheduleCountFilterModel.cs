using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models.FilterParameter
{
    public class UserShiftScheduleCountFilterModel
    {
        public int DiasterId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? DepartmentId { get; set; }
    }
}