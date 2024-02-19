using MyEPA.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyEPA.ViewModels
{
    public class ContactManualDepartmentViewModel
    {
        [DisplayName("ID")]
        public int? Id { get; set; }
        [DisplayName("單位名稱")]
        [Required]
        public string Name { get; set; }
        public ContactManualDepartmentTypeEnum Type { get; set; }
    }
}