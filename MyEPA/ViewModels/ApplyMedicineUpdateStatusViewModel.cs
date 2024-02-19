using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.ViewModels
{
    public class ApplyMedicineHandlingSituationViewModel
    {
        public int Type { get; set; }
        public int MedicineType { get; set; }
        public int Quantity { get; set; }
        public decimal Subsidy { get; set; }
    }
    public class ApplyMedicineUpdateStatusViewModel : ApplySupportUpdateStatusViewModel
    {
        public new List<ApplyMedicineHandlingSituationViewModel> HandlingSituations { get; set; }
    }
}