using MyEPA.ViewModels;
using System.ComponentModel;

namespace MyEPA.Models.SearchViewModel
{
    public class ContactManualTeamViewModel : ContactManualEPARoleViewModel
    {
        [DisplayName("部門")]
        public string DepartmentName { get; set; }
    }
}