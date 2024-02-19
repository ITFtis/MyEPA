using System.ComponentModel;

namespace MyEPA.Enums
{
    public enum SupportTypeEnum
    {
        [Description("單向")]
        Single = 1,
        [Description("雙向")]
        Mutual = 2,
    }
}