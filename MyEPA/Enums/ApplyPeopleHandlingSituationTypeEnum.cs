using System.ComponentModel;

namespace MyEPA.Enums
{
    public enum ApplyPeopleHandlingSituationTypeEnum
    {
        //用來標示沒有 type 的項目
        [Description("無")]
        None = 0,
        [Description("人力支援")]
        Other = 1,
        [Description("補助款")]
        Subsidy = 2,
    }
}