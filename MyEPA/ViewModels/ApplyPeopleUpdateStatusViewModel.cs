using System.Collections.Generic;

namespace MyEPA.ViewModels
{
    public class ApplyPeopleUpdateStatusViewModel : ApplySupportUpdateStatusViewModel
    {
        public new List<ApplyPeopleHandlingSituationViewModel> HandlingSituations { get; set; }
    }
    public class ApplyPeopleHandlingSituationViewModel
    {
        public int Type { get; set; }
        public int PeopleType { get; set; }
        public int Quantity { get; set; }
        public int Day { get; set; }
        public decimal Subsidy { get; set; }
    }
}