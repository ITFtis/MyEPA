using MyEPA.Enums;
using System.ComponentModel;

namespace MyEPA.ViewModels
{
    public class ContactManualPermissionViewModel
    {
        [DisplayName("人員")]
        public int UserId { get; set; }
        [DisplayName("帳號")]
        public string UserName { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; }
        public ContactManualDutyEnum ContactManualDuty { get; set; }
    }
}