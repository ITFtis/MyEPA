using System.ComponentModel;

namespace MyEPA.Enums
{
    public enum WaterCheckStatusEnum
    {
        [Description("未檢測")]
        Pending = 0,
        [Description("本日無災情")]
        NothingHappened = 1,
        [Description("無法抽驗")]
        Failure = 2,
        [Description("無異常")]
        Success = 3,
        /// <summary>
        /// 不合格
        /// </summary>
        [Description("不合格")]
        Disqualified = 4,
    }

    public enum WaterCheckDetailStatusEnum
    {
        /// <summary>
        /// 未檢測
        /// </summary>
        [Description("未檢測")]
        Pending = 0,
        /// <summary>
        /// 不合格
        /// </summary>
        [Description("不合格")]
        Failed = 1,
        /// <summary>
        /// 無災情
        /// </summary>
        [Description("無災情")]
        NothingHappened = 2,
        /// <summary>
        /// 無法抽驗
        /// </summary>
        [Description("無法抽驗")]
        Cannot = 3,
        /// <summary>
        /// 無異常
        /// </summary>
        [Description("無異常")]
        Success = 4,
        [Description("檢驗中")]
        /// <summary>
        /// 檢驗中
        /// </summary>
        Testing = 5,
    }
}