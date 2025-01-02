using MyEPA.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class UsersViewModel
    {
        public int Id { get; set; }
        [DisplayName("機關類別(角色)")]
        public string Duty { get; set; }
        [DisplayName("機關名稱(單位)")]
        public string City { get; set; }
        [DisplayName("單位名稱(鄉鎮市區)")]
        public string Town { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; }
        [DisplayName("人員類別")]
        public string HumanType { get; set; }
        [DisplayName("職稱")]
        public string PositionName { get; set; }
        [DisplayName("更新時間")]
        public DateTime? UpdateDate { get; set; }
    }
    public class UserEditViewModel
    {
        public int? Id { get; set; }
        [DisplayName("帳號")]
        public string UserName { get; set; }
        [DisplayName("姓名")]

        public string Name { get; set; }
        [DisplayName("密碼")]
        public string Pwd { get; set; }
        [DisplayName("機關類別(角色)")]
        public string Duty { get; set; }
        [DisplayName("機關名稱(單位)")]
        public string City { get; set; }
        [DisplayName("單位名稱(鄉鎮市區)")]
        public string Town { get; set; }
        [DisplayName("行動電話")]
        public string MobilePhone { get; set; }
        [DisplayName("人員類別")]
        public string HumanType { get; set; }
        [DisplayName("是否為該單位主要聯絡人")]
        public string MainContacter { get; set; }

        public string ReportPriority { get; set; }

        public int DepartmentId { get; set; }
        [DisplayName("職稱")]
        public int PositionId { get; set; }
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
        [DisplayName("機關名稱(縣市)")]
        public int? CityId { get; set; }
        [DisplayName("單位(鄉鎮區)名稱")]
        public int? TownId { get; set; }
        [DisplayName("機關類別(角色)")]
        public int? DutyId { get; set; }
    }

    public class UserLoginViewModel : UsersModel
    {
        [DisplayName("登入時間")]
        public DateTime? LoginTime { get; set; }

        [DisplayName("未登入天數")]
        public int? LoginRange { get; set; }
    }

    public class UsersModel
    {
        [AutoKey]
        public int Id { get; set; }
        public string UserName { get; set; }
        [DisplayName("姓名")]

        public string Name { get; set; }

        public string Pwd { get; set; }
        [DisplayName("密碼到期日")]
        public DateTime PwdUpdateDate { get; set; }
        public string VoicePwd { get; set; }
        [DisplayName("機關類別(角色)")]
        public string Duty { get; set; }
        [DisplayName("機關名稱(單位)")]
        public string City { get; set; }
        [DisplayName("單位名稱(鄉鎮市區)")]
        public string Town { get; set; }
        [DisplayName("行動電話")]
        public string MobilePhone { get; set; }
        [DisplayName("人員類別")]
        public string HumanType { get; set; }
        [DisplayName("是否為該單位主要聯絡人")]
        public string MainContacter { get; set; }

        public string ReportPriority { get; set; }

        public int DepartmentId { get; set; }

        public int PositionId { get; set; }
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

        public int? CityId { get; set; }
        public int? TownId { get; set; }
        public int? DutyId { get; set; }

        [DisplayName("資料更新日期")]
        public DateTime? UpdateDate { get; set; }
        [DisplayName("資料更新者")]
        public string UpdateUser { get; set; }
        public DateTime? ConfirmTime { get; set; }
        public bool IsAdmin { get; set; }
        public ContactManualDutyEnum ContactManualDuty { get; set; }
        public int? ContactManualDepartmentId { get; set; }
        public string ISEnvironmentalProtectionAdministration { get; set; }
        public string ISEnvironmentalProtectionDepartment { get; set; }
        public string ISBook { get; set; }
    }

    public class UsersInfoModel : UsersModel
    {
        [DisplayName("部門")]
        public string DepartmentName { get; set; }
        [DisplayName("職稱")]
        public string PositionName { get; set; }

    }

    public class UsersEditViewModel
    {
        public string EditingId { get; set; }
        public string EditingName { get; set; }
        public string EditingPwd { get; set; }
        public string EditingVoicePwd { get; set; }
        public string EditingDuty { get; set; }

        public string EditingCity { get; set; }

        public string EditingTown { get; set; }

        public string EditingMobilePhone { get; set; }
        public string EditingOfficePhone { get; set; }
        public string EditingEmail { get; set; }
        public List<string> EditingHumanType { get; set; }

        public string EditingMainContacter { get; set; }
        public string EditingReportPriority { get; set; }
        public int EditingPositionId { get; set; }
        public string SearchISEnvironmentalProtectionAdministration { get; set; }
        public string SearchISEnvironmentalProtectionDepartment { get; set; }
        public string SearchISBook { get; set; }
    }
}