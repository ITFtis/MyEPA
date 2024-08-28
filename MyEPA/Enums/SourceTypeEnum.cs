using System.ComponentModel;

namespace MyEPA.Enums
{
    public enum SourceTypeEnum
    {
        /// <summary>
        /// 無
        /// </summary>
        None = 0,

        /// <summary>
        /// 開放合約
        /// </summary>
        [Description("開放合約")]
        OpenContract = 1,

        /// <summary>
        /// 支援協議
        /// </summary>
        [Description("支援協議")]
        MutualSupport = 2,

        /// <summary>
        /// 請求人力支援
        /// </summary>
        [Description("人力支援")]
        ApplyPeople = 3,

        /// <summary>
        /// 請求消毒藥劑支援
        /// </summary>
        [Description("消毒藥劑支援")]
        ApplyMedicine = 4,

        /// <summary>
        /// 請求補助款支援
        /// </summary>
        [Description("補助款支援")]
        ApplySubsidy = 5,

        /// <summary>
        /// 其他(包括垃圾場災損)
        /// </summary>
        [Description("其他支援")]
        ApplyOther = 6,

        /// <summary>
        /// 請求車輛支援
        /// </summary>
        [Description("車輛支援")]
        ApplyCar = 7,

        /// <summary>
        /// 環境消毒設備支援
        /// </summary>
        [Description("環境消毒設備支援")]
        ApplyDisinfectionEquipment = 8,


        /// <summary>
        ///  災情通報
        /// </summary>
        [Description("災情通報")]
        Damage = 10,


        [Description("署補助車輛管理追蹤表")]
        VehicleManagement = 11,

        [Description("任務編組")]
        TaskGrouping = 12,

        /// <summary>
        ///  災情通報檔案
        /// </summary>
        [Description("災情通報檔案")]
        DamageFile = 21,
        /// <summary>
        ///  災情通報圖片
        /// </summary>
        [Description("災情通報圖片")]
        DamageImage = 22,

        /// <summary>
        ///  災情通報檔案
        /// </summary>
        [Description("環境清理檔案")]
        DamageCCFile = 23,
        /// <summary>
        ///  災情通報圖片
        /// </summary>
        [Description("環境清理圖片")]
        DamageCCImage = 24,
        /// <summary>
        ///  三區回報處理圖片
        /// </summary>
        [Description("三區回報處理圖片")]
        DamageProcessImage = 25,
        /// <summary>
        ///  三區回報處理檔案
        /// </summary>
        [Description("三區回報處理檔案")]
        DamageProcessFile = 26,
        /// <summary>
        /// 各業務單位春節因應環境污染事故緊急應變摘要
        /// </summary>
        [Description("各業務單位春節因應環境污染事故緊急應變摘要")]
        ContactManual = 101,
        /// <summary>
        /// 各業務單位春節因應環境污染事故緊急應變作業流程圖
        /// </summary>
        [Description("各業務單位春節因應環境污染事故緊急應變作業流程圖")]
        ContactManualFlow = 102,
        /// <summary>
        /// 監資處 (SupervisionSourceEnum)
        /// </summary>
        [Description("監資處委外")]
        Supervision = 103,
    }
}