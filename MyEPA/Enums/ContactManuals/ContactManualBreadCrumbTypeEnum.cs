using System.ComponentModel;

namespace MyEPA.Enums
{
    public enum ContactManualBreadCrumbTypeEnum
    {
        /// <summary>
        /// 手冊資料單位維護
        /// </summary>
        [Description("手冊資料單位維護")]
        ContactManualDepartment,
        /// <summary>
        /// 手冊資料環保署相關名單
        /// </summary>
        [Description("手冊資料環保署相關名單")]
        ContactManualEPA,
        /// <summary>
        /// 各業務單位緊急應變通聯表
        /// </summary>
        [Description("各業務單位緊急應變通聯表")]
        ContactManualEPAOther,
        /// <summary>
        /// 應變名冊
        /// </summary>
        [Description("應變名冊")]
        ContactManualEPARole,
        /// <summary>
        /// 手冊資料環保局相關名單
        /// </summary>
        [Description("手冊資料環保局相關名單")]
        ContactManualEPB,
        /// <summary>
        /// 手冊資料維護角色
        /// </summary>
        [Description("手冊資料維護角色")]
        ContactManualRole,
        /// <summary>
        /// 督導責任區劃分表
        /// </summary>
        [Description("督導責任區劃分表")]
        ContactManualSupervise,
        /// <summary>
        /// 使用者資料維護
        /// </summary>
        [Description("使用者資料維護")]
        UserEPA,
        [Description("春節期間督導責任區域劃分表")]
        ContactManualEPASupervise,
        [Description("回收基管會手冊資料維護")]
        ContactManualRecycle,
        [Description("值班資料維護")]
        ContactManualOnDuty,
        [Description("化學物質管理署24小時勤情單位")]
        ContactManual24OnDuty,
        ContactManualFileData,
        ContactManualSupervisionFileData
    }
}