using System.ComponentModel;

namespace MyEPA.ViewModels
{
    public class ApplySupportEPBStatusCountReportViewModel
    {
        /// <summary>
        /// 請求人力支援
        /// </summary>
        public ApplySupportEPBStatusCountViewModel ApplyPeople { get; set; }

        /// <summary>
        /// 請求消毒藥劑支援
        /// </summary>
        public ApplySupportEPBStatusCountViewModel ApplyMedicine { get; set; }

        /// <summary>
        /// 請求補助款支援
        /// </summary>
        public ApplySupportEPBStatusCountViewModel ApplySubsidy { get; set; }

        /// <summary>
        /// 其他(包括垃圾場災損)
        /// </summary>
        public ApplySupportEPBStatusCountViewModel ApplyOther { get; set; }

        /// <summary>
        /// 請求車輛支援
        /// </summary>
        public ApplySupportEPBStatusCountViewModel ApplyCar { get; set; }

        /// <summary>
        /// 環境消毒設備支援
        /// </summary>
        public ApplySupportEPBStatusCountViewModel ApplyDisinfectionEquipment { get; set; }

    }
    public class ApplySupportEPBStatusCountViewModel
    {
        /// <summary>
        /// 待審核
        /// </summary>
        public int PendingCount { get; set; }


        /// <summary>
        /// 審核中
        /// </summary>
        public int ProcessingCount { get; set; }

        /// <summary>
        /// 轉呈環保署
        /// </summary>
        public int SendToEpaCount { get; set; }

        /// <summary>
        /// 局已核定
        /// </summary>
        public int ConfrimCount { get; set; }
           
        /// <summary>
        /// 退回
        /// </summary>
        public int RejectCount { get; set; }

    }
    public class ApplySupportBaseStatusCountViewModel
    {
        /// <summary>
        /// 原請求量
        /// </summary>
        public decimal ApplyPeopleCount { get; set; }
        /// <summary>
        /// 局核定
        /// </summary>
        public decimal ApplyPeopleEpbConfrimCount { get; set; }
        /// <summary>
        /// 署核定
        /// </summary>
        public decimal ApplyPeopleEpaConfrimCount { get; set; }
        /// <summary>
        /// 原請求量
        /// </summary>
        public decimal ApplyCarCount { get; set; }
        /// <summary>
        /// 局核定
        /// </summary>
        public decimal ApplyCarEpbConfrimCount { get; set; }
        /// <summary>
        /// 署核定
        /// </summary>
        public decimal ApplyCarEpaConfrimCount { get; set; }
        /// <summary>
        /// 原請求量
        /// </summary>
        public decimal ApplyDisinfectionEquipmentCount { get; set; }
        /// <summary>
        /// 局核定
        /// </summary>
        public decimal ApplyDisinfectionEquipmentEpbConfrimCount { get; set; }
        /// <summary>
        /// 署核定
        /// </summary>
        public decimal ApplyDisinfectionEquipmentEpaConfrimCount { get; set; }
        /// <summary>
        /// 原請求量
        /// </summary>
        public decimal ApplyMedicineCount { get; set; }
        /// <summary>
        /// 局核定
        /// </summary>
        public decimal ApplyMedicineEpbConfrimCount { get; set; }
        /// <summary>
        /// 署核定
        /// </summary>
        public decimal ApplyMedicineEpaConfrimCount { get; set; }

        /// <summary>
        /// 原請求量
        /// </summary>
        public decimal ApplySubsidyCount { get; set; }
        /// <summary>
        /// 局核定
        /// </summary>
        public decimal ApplySubsidyEpbConfrimCount { get; set; }
        /// <summary>
        /// 署核定
        /// </summary>
        public decimal ApplySubsidyEpaConfrimCount { get; set; }

        /// <summary>
        /// 原請求量
        /// </summary>
        public decimal ApplyOtherCount { get; set; }
        /// <summary>
        /// 局核定
        /// </summary>
        public decimal ApplyOtherEpbConfrimCount { get; set; }
        /// <summary>
        /// 署核定
        /// </summary>
        public decimal ApplyOtherEpaConfrimCount { get; set; }

        public decimal Money { get; set; }
    }
    public class ApplySupportCityStatusCountViewModel : ApplySupportBaseStatusCountViewModel
    {
       
        public int CityId { get; set; }
        public string CityName { get; set; }
    }

    public class ApplySupportTownStatusCountViewModel : ApplySupportBaseStatusCountViewModel
    {
        public int TownId { get; set; }
        public string TownName { get; set; }
    }

}