using MyEPA.Enums;
using System;
using System.ComponentModel;

namespace MyEPA.ViewModels
{
    public class UserEPAListViewModel : UserEPAViewModel
    {
        [DisplayName("單位別")]
        public string DepartmentName { get; set; }
        [DisplayName("職稱")]
        public string PositionName { get; set; }
    }
    public class UserEPAViewModel
    {
        [AutoKey]
        public int? Id { get; set; }
        [DisplayName("使用者帳號")]
        public string UserName { get; set; }
        [DisplayName("姓名")]

        public string Name { get; set; }
        [DisplayName("密碼")]
        public string Pwd { get; set; }
        [DisplayName("行動電話")]
        public string MobilePhone { get; set; }
        [DisplayName("辦公室電話")]
        public string OfficePhone { get; set; }
        [DisplayName("傳真")]
        public string FaxNumber { get; set; }
        [DisplayName("電子郵件信箱")]
        public string Email { get; set; }
        [DisplayName("備註")]
        public string Remark { get; set; }
        [DisplayName("住宅電話")]
        public string HomeNumber { get; set; }
        /// <summary>
        /// 鄉鎮 ID
        /// </summary>
        public int TownId { get; set; }
        /// <summary>
        /// 職稱 ID
        /// </summary>
        [DisplayName("職稱")]
        public int PositionId { get; set; }
        [DisplayName("職務")]
        public ContactManualDutyEnum ContactManualDuty { get; set; }
        [DisplayName("手冊單位")]
        public int? DepartmentId { get; set; }
    }
}