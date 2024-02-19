using MyEPA.Enums;
using System.ComponentModel;

namespace MyEPA.ViewModels
{
    public class ContactManualRoleViewModel
    {
        [DisplayName("ID")]
        public int? Id { get; set; }
        [DisplayName("單位名稱")]
        public string Name { get; set; }
        public ContactManualRoleTypeEnum Type { get; set; }
    }
}