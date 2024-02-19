using System.Collections.Generic;

namespace MyEPA.ViewModels
{
    public class ApplySupportProcessingViewModel
    {
        //名稱故意用單數, 方便跟 ApplyTypeEnum 做對應
        //保留 set 方便做反射, 還沒想到更好的方式
        public List<ApplyPeopleViewModel> ApplyPeople { get; set; }
        public List<ApplyMedicineViewModel> ApplyMedicine { get; set; }
        public List<ApplyOtherViewModel> ApplyOther { get; set; }
        public List<ApplySubsidyViewModel> ApplySubsidy { get; set; }
        public List<ApplyDisinfectionEquipmentViewModel> ApplyDisinfectionEquipment { get; set; }
        public List<ApplyCarViewModel> ApplyCar { get; set; }
    }
}