using MyEPA.Enums;

namespace MyEPA.Models
{
    public class UserBriefModel
    {
        public int UserId { get; set; }
        /// <summary>
        /// 帳號
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public int CityId { get; set; }
        public int TownId { get; set; }

        public DutyEnum Duty { get; set; }
        public bool IsAdmin { get; set; }
        /// <summary>
        /// 督察大隊區域
        /// </summary>
        public AreaEnum? Area { get; set; }
        /// <summary>
        /// 電子手冊權限
        /// </summary>
        public ContactManualDutyEnum ContactManualDuty { get; set; }
        /// <summary>
        /// 手冊單位
        /// </summary>
        public int? ContactManualDepartmentId { get; set; }
        public string ContactManualDepartment { get; set; }
    }
}