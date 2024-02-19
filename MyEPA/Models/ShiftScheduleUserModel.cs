using System;

namespace MyEPA.Models
{
    public class MainShiftScheduleUserModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int MainShiftScheduleId { get; set; }

        public int DepartmentId { get; set; }

        public int UserId { get; set; }

        public int Sort { get; set; }

        public DateTime? CheckinTime { get; set; }

        public DateTime? Checkout { get; set; }

    }
    public class ShiftScheduleUserModel
    {
        [AutoKey]
        public int Id { get; set; }
        public int ShiftScheduleId { get; set; }

        public int UserId { get; set; }

        public int Sort { get; set; }

        public DateTime? CheckinTime { get; set; }

        public DateTime? Checkout { get; set; }

    }

    public class ShiftScheduleUserJoinUsersModel
    {
        public int ShiftScheduleId { get; set; }

        public int UserId { get; set; }

        public int Sort { get; set; }

        public DateTime? CheckinTime { get; set; }

        public DateTime? Checkout { get; set; }

        public string MobilePhone { get; set; }

        public string Name { get; set; }

    }

    public class MainShiftScheduleUserJoinUsersModel
    {
        public int Id { get; set; }
        public int MainShiftScheduleId { get; set; }
        public int DepartmentId { get; set; }

        public int UserId { get; set; }

        public int Sort { get; set; }

        public DateTime? CheckinTime { get; set; }

        public DateTime? Checkout { get; set; }

        public string MobilePhone { get; set; }

        public string Name { get; set; }

    }

    public class TeamShiftScheduleUserModel
    {
        [AutoKey]
        public int Id { get; set; }
        public int TeamShiftScheduleId { get; set; }

        public int UserId { get; set; }

        public int Sort { get; set; }

        public DateTime? CheckinTime { get; set; }

        public DateTime? Checkout { get; set; }

    }

    public class TeamShiftScheduleUserJoinUsersModel
    {
        public int TeamShiftScheduleId { get; set; }

        public int UserId { get; set; }

        public int Sort { get; set; }

        public DateTime? CheckinTime { get; set; }

        public DateTime? Checkout { get; set; }

        public string MobilePhone { get; set; }

        public string Name { get; set; }

    }
}