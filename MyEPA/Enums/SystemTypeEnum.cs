using System.ComponentModel;

namespace MyEPA.Enums
{
    public enum SystemTypeEnum
    {
        [Description("災害管理系統")]
        MyEPA = 1,
        [Description("署補助車輛管理系統")]
        EPACar = 2,
        [Description("緊急應變摘要及通聯手冊電子化系統")]
        ContactManual = 3
    }
}