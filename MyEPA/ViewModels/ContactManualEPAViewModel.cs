using MyEPA.Enums;
using System.ComponentModel;

namespace MyEPA.ViewModels
{
    public class ContactManualBaseViewModel
    {
        public int Id { get; set; }
        [DisplayName("類別")]
        public ContactManualTypeEnum Type { get; set; }
        [DisplayName("人員")]
        public int UserId { get; set; }
        [DisplayName("排序")]
        public int Sort { get; set; }
        [DisplayName("職務")]
        public int RoleId { get; set; }
        [DisplayName("備註")]
        public string Note { get; set; }
    }
    public class ContactManualEPAViewModel : ContactManualBaseViewModel
    {
        [DisplayName("單位")]
        public int? DepartmentId { get; set; }
    }
    public class ContactManualEPBViewModel : ContactManualBaseViewModel
    {
        [DisplayName("單位")]
        public int CityId { get; set; }
    }
}