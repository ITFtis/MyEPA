using System.Collections.Generic;

namespace MyEPA.ViewModels
{
    public class ApplyDisinfectionEquipmentUpdateStatusViewModel : ApplySupportUpdateStatusViewModel
    {
        public new List<ApplyDisinfectionEquipmentHandlingSituationViewModel> HandlingSituations { get; set; }
    }
    public class ApplyDisinfectionEquipmentHandlingSituationViewModel
    {
        public int Type { get; set; }
        public string Name { get; set; }
        public int Day { get; set; }
        public decimal Subsidy { get; set; }
    }
}