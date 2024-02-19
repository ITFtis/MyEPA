using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class MainShiftScheduleModel
    {
        [AutoKey]
        public int Id { get; set; }

        public bool IsNight { get; set; }

        public int? DiasterId { get; set; }

        public DateTime Date { get; set; }

    }
    public class ShiftScheduleModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int DepartmentId { get; set; }

        public bool IsNight { get; set; }

        public int DiasterId { get; set; }

        public DateTime Date { get; set; }
    }
    public class TeamShiftScheduleModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int DepartmentId { get; set; }

        public int? DiasterId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

    }
}