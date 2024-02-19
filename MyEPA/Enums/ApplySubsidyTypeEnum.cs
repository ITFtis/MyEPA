using System.ComponentModel;

namespace MyEPA.Enums
{
    public enum ApplySubsidyTypeEnum
    {
        //用來標示沒有 type 的項目(如清潔工或其他)
        [Description("無")]
        None = 0,

        [Description("僱用臨時工")]
        HireTemporaryWorkers = 1,

        [Description("租賃清理機具(含費用)")]
        RentalCleaningEquipment = 2,

        [Description("租賃消毒設備(含費用)")]
        RentalDisinfectionEquipment = 3,

        [Description("購買消毒藥劑(含費用)")]
        PurchaseDisinfectant = 4,

        [Description("雜項")]
        Miscellaneous = 5,

        [Description("其他")]
        Other = 6
    }
}


