using System.Collections.Generic;

namespace MyEPA.ViewModels
{
    public class ApplyCarUpdateStatusViewModel : ApplySupportUpdateStatusViewModel
    {
        public new List<ApplyCarHandlingSituationViewModel> HandlingSituations { get; set; }
    }
    public class ApplyCarHandlingSituationViewModel
    {
        public int Type { get; set; }
        public int CarType { get; set; }
        public int Quantity { get; set; }
        public int Day { get; set; }
        public decimal Subsidy { get; set; }
    }
}