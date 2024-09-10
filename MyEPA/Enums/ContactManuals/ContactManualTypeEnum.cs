using System.ComponentModel;

namespace MyEPA.Enums
{
    public enum ContactManualTypeEnum
    {
        /// <summary>
        /// 本署環境污染事故主政單位通聯名冊
        /// 部門
        /// </summary>
        [Description("本署環境污染事故主政單位通聯名冊")]
        EPA = 1,
        /// <summary>
        /// 綜計處－環境影響評估案件緊急應變通聯表
        /// </summary>
        [ContactManualGroup("綜計處")]
        [Description("綜計處－環境影響評估案件緊急應變通聯表")]
        EPAGeneralPlanning = 2,
        /// <summary>
        /// 秘書處－辦公大樓緊急事件主管單位
        /// </summary>
        [ContactManualGroup("秘書處")]
        [Description("環境部秘書處辦公大樓緊急事件主管單位")]
        EPASecretaryRoom = 3,
        /// <summary>
        /// 新聞公關組－重大輿論情緊急應變通聯表
        /// </summary>
        [ContactManualGroup("新聞公關組")]
        [Description("新聞公關組－重大輿情緊急應變通聯表")]
        EPANewsPublicRelationsTeam = 4,
        /// <summary>
        /// 土汙基管會－土壤及地下水環境污染事故緊急應變通聯表
        /// </summary>
        [ContactManualGroup("土汙基管會")]
        [Description("環境部土汙基管會土壤及地下水環境污染事故應變計畫主管單位")]
        EPASoilPollution = 5,
        /// <summary>
        /// 環檢所－環境部國家環境研究院災害防救緊急應變聯絡資料
        /// </summary>
        [ContactManualGroup("環檢所")]
        [Description("環境部國家環境研究院災害防救緊急應變聯絡資料")]
        EPAEnvironmentalInspection = 6,
        /// <summary>
        /// 地方環保局長通聯名冊
        /// </summary>
        [Description("地方環保局長通聯名冊")]
        EPB = 7,
        /// <summary>
        /// 空保處－各地方環保局春節期間空氣污染事故緊急通聯表
        /// </summary>
        [ContactManualGroup("空保處")]
        [Description("各地方環保局春節期間空氣污染事故緊急通連表")]
        EPBAirSecurity = 8,
        /// <summary>
        /// 水保處－各地方環保局春節期間河川污染應變通報名冊緊急通聯表
        /// </summary>
        [ContactManualGroup("水保處")]
        [Description("各地方環保局春節期間河川污染應變通報名冊緊急通聯表")]
        EPBWaterSecurityRiver = 9,
        /// <summary>
        /// 水保處－各地方環保局春節期間飲用水重大事故緊急通聯表
        /// </summary>
        [ContactManualGroup("水保處")]
        [Description("各地方環保局春節期間飲用水重大事故緊急通聯表")]
        EPBWaterSecurityDrinkingWater = 10,
        /// <summary>
        /// 廢管處－各地方環保局春節期間一般廢棄物處理緊急應變通聯表
        /// </summary>
        [ContactManualGroup("廢管處")]
        [Description("各地方環保局春節期間一般廢棄物處理緊急應變通聯表")]
        EPBWaste = 11,
        /// <summary>
        /// 環境管理署－各地方環保局春節期間天然災害事故緊急通聯表
        /// </summary>
        [ContactManualGroup("環境管理署")]
        [Description("各地方環保局春節期間天然災害事故緊急通聯表")]
        EPBEnvironmentDisaster = 12,
        /// <summary>
        /// 環境管理署－各地方環保局春節期間流感疫情爆發之環境清理緊急通聯
        /// </summary>
        [ContactManualGroup("環境管理署")]
        [Description("各地方環保局春節期間流感疫情爆發之環境清理緊急通聯表")]
        EPBEnvironmentInfluenza = 13,
        /// <summary>
        /// 管考處－各地方環保局春節期間重大緊急公害糾紛事件通聯表
        /// </summary>
        [ContactManualGroup("管考處")]
        [Description("各地方環保局春節期間重大緊急公害糾紛事件通聯表")]
        EPBControlAssessment = 14,
        /// <summary>
        /// 環境管理署－各地方環保局春節期間一般廢棄物處理設備設施緊急應變通聯表
        /// </summary>
        [ContactManualGroup("環境管理署")]
        [Description("各地方環保局春節期間一般廢棄物處理設備設施緊急應變通聯表")]
        EPBTeam = 15,
        /// <summary>
        /// 回收基管會－各地方環保局春節期間應回收廢棄物回收處理業危機處理及環保重大事件應變緊急連絡表
        /// </summary>
        [ContactManualGroup("回收基管會")]
        [Description("各地方環保局春節期間應回收廢棄物回收處理業危機處理及環保重大事件應變緊急連絡表")]
        EPBRecycle = 16,
        /// <summary>
        /// 土汙基管會－各地方環保局春節期間土壤及地下水環境污染事故緊急通聯表
        /// </summary>
        [ContactManualGroup("土汙基管會")]
        [Description("各縣市環保局春節期間土壤及地下水環境污染事故緊急通聯表")]
        EPBSoilPollution = 17,
        /// <summary>
        /// 化學物質管理署－各地方環保局春節期間毒性化學物質災害事故緊急通聯表
        /// </summary>
        [ContactManualGroup("化學物質管理署")]
        [Description("各縣市環保局春節期間毒性化學物質災害事故緊急通聯表")]
        EPBChemical = 18,
        /// <summary>
        /// 化學物質管理署－各地方環保局春節期間環境用藥重大消費事件緊急應變通聯表
        /// </summary>
        [ContactManualGroup("化學物質管理署")]
        [Description("各地方環保局春節期間環境用藥重大消費事件緊急應變通聯表")]
        EPBChemicalDrug = 19,
        /// <summary>
        /// 空保處－空氣污染緊急事故應變計畫主管單位
        /// </summary>
        [ContactManualGroup("空保處")]
        [Description("環境部空保處空氣污染緊急事故應變計畫主管單位")]
        EPARoleAirSecurity = 20,
        [ContactManualGroup("水保處")]
        [Description("環境部水保處河川水污染事故緊應變計畫主管單位")]
        EPARoleWaterSecurityRiver = 21,
        [ContactManualGroup("水保處")]
        [Description("環境部水保處飲用水重大事故應變計畫主管單位")]
        EPARoleWaterSecurityDrinkingWater = 22,
        [ContactManualGroup("廢管處")]
        [Description("環境部廢管處一般廢棄物緊急事故應變計畫主管單位")]
        EPARoleWaste = 23,
        [ContactManualGroup("環境管理署")]
        [Description("環境部環境管理署天然災害應變計畫主管單位")]
        EPARoleEnvironmentDisaster = 24,
        /// <summary>
        /// 環境管理署-環境部春節期間流感疫情爆發之環境清理應變計畫主管單位
        /// </summary>
        [ContactManualGroup("環境管理署")]
        [Description("環境部春節期間流感疫情爆發之環境清理應變計畫主管單位")]
        EPARoleEnvironmentInfluenza = 25,
        /// <summary>
        /// 管考處-環境部管考處重大緊急公害糾紛事件主管單位
        /// </summary>
        [ContactManualGroup("管考處")]
        [Description("環境部管考處重大緊急公害糾紛事件主管單位")]
        EPARoleControlAssessment = 26,
        /// <summary>
        /// 監資處－空氣品質監測站安全維護緊急應變人員名冊
        /// </summary>
        [ContactManualGroup("監資處")]
        [Description("環境部監資處空氣品質監測站安全維護緊急應變人員名冊")]
        EPARoleSupervisionAirQuality = 27,
        [ContactManualGroup("監資處")]
        [Description("監資處空品監測站各區安全維護緊急應變小組人員名冊")]
        EPARoleSupervisionAir = 28,
        [ContactManualGroup("環境管理署")]
        [Description("環境部環境管理署廢棄物處理事故緊急應變計畫主管單位")]
        EPARoleTeam = 29,
        [ContactManualGroup("回收基管會")]
        [Description("回收基管會應回收廢棄物回收處理業環境污染事故應變計畫主管單位")]
        EPARoleRecycle = 30,
        [ContactManualGroup("化學物質管理署")]
        [Description("環境部化學物質管理署毒災事故應變計畫主管單位")]
        EPARoleChemicalPoison = 31,
        [ContactManualGroup("化學物質管理署")]
        [Description("環境部化學物質管理署環境用藥重大消費事件緊急應變計畫主管單位")]
        EPARoleChemicalDrug = 32,
        /// <summary>
        /// 綜計處
        /// </summary>
        [ContactManualGroup("綜計處")]
        [Description("春節期間綜計處環境污染事故督導責任區域劃分表")]
        EPASuperviseGeneralPlanning = 33,
        /// <summary>
        /// 空保處
        /// </summary>
        [ContactManualGroup("空保處")]
        [Description("春節期間空保處環境污染事故督導責任區域劃分表")]
        EPASuperviseAirSecurity = 34,
        /// <summary>
        /// 水保處
        /// </summary>
        [ContactManualGroup("水保處")]
        [Description("春節期間水保處環境污染事故督導責任區域劃分表")]
        EPASuperviseWaterSecurity = 35,
        /// <summary>
        /// 廢管處
        /// </summary>
        [ContactManualGroup("廢管處")]
        [Description("春節期間廢管處環境污染事故督導責任區域劃分表")]
        EPASuperviseWaste = 36,
        /// <summary>
        /// 環境管理署
        /// </summary>
        [ContactManualGroup("環境管理署")]
        [Description("春節期間環境管理署環境污染事故督導責任區域劃分表")]
        EPASuperviseTeam = 37,
        /// <summary>
        /// 管考處
        /// </summary>
        [ContactManualGroup("管考處")]
        [Description("春節期間管考處重大緊急公害糾紛事件督導責任區域劃分表")]
        EPASuperviseControlAssessment = 38,
        /// <summary>
        /// 回收機管會
        /// </summary>
        [ContactManualGroup("回收機管會")]
        [Description("春節期間回收基管會環境污染事故督導責任區域劃分表")]
        EPASuperviseRecycle = 39,
        /// <summary>
        /// 土汙基管會
        /// </summary>
        [ContactManualGroup("土汙基管會")]
        [Description("春節期間土汙基管會事故督導責任區域劃分表")]
        EPASuperviseSoilPollution = 40,
        /// <summary>
        /// 化學
        /// </summary>
        [ContactManualGroup("化學物質管理署")]
        [Description("春節期間化學物質管理署環境污染事故督導責任區域劃分表")]
        EPASuperviseChemical = 41,
        [ContactManualGroup("回收基管會")]
        [Description("回收基管會回收處理業重大異常事件緊急連絡通話")]
        EPARecycle = 42,
        [ContactManualGroup("回收基管會")]
        [Description("應回收廢棄物回收處理業危機處理及環保重大事件應變通報聯絡人員名冊(稽核認證團體)")]
        EPARecycleEF = 43,
        /// <summary>
        /// 監資處值班表
        /// </summary>
        [ContactManualGroup("監資處")]
        [Description("監資處春節期間空氣品質污染指標預報值班人員通聯表(每日輪值 08:30~17:30)")]
        EPAOnDutySupervision = 44,
        [ContactManualGroup("環境管理署")]
        [Description("環境管理署春節期間一般廢棄物處理設備設施緊急應變通聯表")]
        EPATeam = 45,
        /// <summary>
        /// 化學物質管理署24小時勤情單位
        /// </summary>
        [ContactManualGroup("化學物質管理署")]
        [Description("環境部24小時勤情表")]
        EPA24OnDutyChemical = 46,
        [ContactManualGroup("監資處")]
        [Description("春節期間監資處環境污染事故督導責任區域劃分表")]
        EPASuperviseSupervision = 47,
        [ContactManualGroup("監資處")]
        [Description("環境部監資處電腦機房及各項資訊系統運轉事故緊急應變人員名冊")]
        EPARoleSupervisionInformation = 48,
        /// <summary>
        /// 監資處值班表
        /// </summary>
        [ContactManualGroup("監資處")]
        [Description("春節期間電腦機房及各項資訊系統運轉事故值班人員通聯表(週 1~6 到署輪值 09:00~13:00) 週日在家輪值")]
        EPAOnDutySupervisionInformation = 49
    }
}