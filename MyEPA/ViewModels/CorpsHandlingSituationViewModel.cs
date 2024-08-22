using MyEPA.Enums;
using System.ComponentModel;

namespace MyEPA.ViewModels
{
    public class CorpsHandlingSituationViewModel
    {
        public int DamageId { get; set; }
        //總隊處理情形
        [DisplayName("處理情形")]
        public string CorpsHandlingSituation { get; set; }
        public FacilityDamageTypeEnum Type { get; set; }
    }
    public class DamageMemoViewModel
    {
        public int DamageId { get; set; }
        [DisplayName("備註")]
        public string Note { get; set; }
        public FacilityDamageTypeEnum Type { get; set; }
    }

}