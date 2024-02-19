using System;

namespace MyEPA.Models.FilterParameter
{
    public class ShiftScheduleCheckDataFilterParameterModel
    {
        public int Hour { get; set; }
        public DateTime Time { get; set; }
        public int UserId { get; set; }
    }
}