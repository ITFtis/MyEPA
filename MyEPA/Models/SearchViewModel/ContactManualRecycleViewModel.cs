using MyEPA.ViewModels;
using System.ComponentModel;

namespace MyEPA.Models.SearchViewModel
{
    public class ContactManualRecycleViewModel : ContactManualEPARoleViewModel
    {
        [DisplayName("部門")]
        public string DepartmentName { get; set; }
    }
    public class ContactManualRecycleReportModel
    {
        [DisplayName("單位")]
        public string DepartmentName { get; set; }
        [DisplayName("職務")]
        public string RoleName { get; set; }
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
}