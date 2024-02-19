using System.ComponentModel;

namespace MyEPA.ViewModels
{
    public class ContactManualEPARoleViewModel : ContactManualViewModel
    {
        [DisplayName("職務")]
        public string RoleName { get; set; }
    }
}