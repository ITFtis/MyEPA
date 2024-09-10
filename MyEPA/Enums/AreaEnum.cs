using System.ComponentModel;

namespace MyEPA.Enums
{
    public enum AreaEnum
    {
        [Description("環境管理署北區環境管理中心")]
        North = 1,
        [Description("環境管理署中區環境管理中心")]
        Middle = 2,
        [Description("環境管理署南區環境管理中心")]
        South = 3,
    }
}