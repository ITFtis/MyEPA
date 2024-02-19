using System.ComponentModel;

namespace MyEPA.ViewModels
{
    public class ContactManualEPARoleReportViewModel
    {

        [DisplayName("職務名稱")]
        public string RoleName { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; }
        [DisplayName("住宅電話")]
        public string HomeNumber { get; set; }
        [DisplayName("行動電話")]
        public string MobilePhone { get; set; }
    }
    public class ContactManualEPASuperviseReportViewModel
    {
        [DisplayName("單位別")]
        public string DepartmentName { get; set; }
        [DisplayName("督導業務")]
        public string Supervise { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; }
        [DisplayName("行動電話")]
        public string MobilePhone { get; set; }
        [DisplayName("備註")]
        public string Note { get; set; }
    }
    public class ContactManualEPAOtherReportViewModel
    {
        [DisplayName("職稱")]
        public string PositionName { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; }
        [DisplayName("總機")]
        public string OfficePhone { get; set; }
        [DisplayName("分機")]
        public string Extension { get; set; }
        [DisplayName("傳真電話")]
        public string FaxNumber { get; set; }
        [DisplayName("住宅電話")]
        public string HomeNumber { get; set; }
        [DisplayName("行動電話")]
        public string MobilePhone { get; set; }
        [DisplayName("備註")]
        public string Note { get; set; }
    }
    public class ContactManualEPAReportViewModel
    {
        [DisplayName("單位")]
        public string DepartmentName { get; set; }
        [DisplayName("職稱")]
        public string PositionName { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; }
        [DisplayName("總機")]
        public string OfficePhone { get; set; }
        [DisplayName("分機")]
        public string Extension { get; set; }
        [DisplayName("傳真電話")]
        public string FaxNumber { get; set; }
        [DisplayName("住宅電話")]
        public string HomeNumber { get; set; }
        [DisplayName("行動電話")]
        public string MobilePhone { get; set; }
        [DisplayName("備註")]
        public string Note { get; set; }
    }
    public class ContactManualEPBReportViewModel
    {
        [DisplayName("單位")]
        public string CityName { get; set; }
        [DisplayName("職稱")]
        public string PositionName { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; }
        [DisplayName("總機")]
        public string OfficePhone { get; set; }
        [DisplayName("分機")]
        public string Extension { get; set; }
        [DisplayName("傳真電話")]
        public string FaxNumber { get; set; }
        [DisplayName("住宅電話")]
        public string HomeNumber { get; set; }
        [DisplayName("行動電話")]
        public string MobilePhone { get; set; }
    }
    public class ContactManual24OnDutyViewModel
    {
        [DisplayName("單位別")]
        public string DepartmentName { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; }
        [DisplayName("電話")]
        public string HomeNumber { get; set; }
        [DisplayName("傳真")]
        public string FaxNumber { get; set; }
    }
    public class ContactManualViewModel
    {
        public int Id { get; set; }
        [DisplayName("部門名稱")]
        public string DepartmentName { get; set; }
        [DisplayName("職稱")]
        public string PositionName { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; }
        [DisplayName("總機")]
        public string OfficePhone { get; set; }
        [DisplayName("分機")]
        public string Extension { get; set; }
        [DisplayName("傳真電話")]
        public string FaxNumber { get; set; }
        [DisplayName("住宅電話")]
        public string HomeNumber { get; set; }
        [DisplayName("行動電話")]
        public string MobilePhone { get; set; }
        [DisplayName("備註")]
        public string Note { get; set; }
        [DisplayName("排序")]
        public int Sort { get; set; }
        public int SourceId { get; set; }
    }
}