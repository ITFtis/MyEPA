using MyEPA.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class WaterCheckDetailViewModel: WaterCheckDetailModel
    {
        [DisplayName("管理處")]
        public string WaterDivision { get; set; }
    }
    public class WaterCheckDetailModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int WaterCheckId { get; set; }

        public int CityId { get; set; }
        [DisplayName("抽樣單位")]
        public string CityName { get; set; }

        public int TownId { get; set; }
       
        public string TownName { get; set; }
        [DisplayName("抽樣地點")]
        public string Address { get; set; }
        [DisplayName("通報時間")]
        public DateTime CheckTime { get; set; }
        [DisplayName("通報者姓名")]
        public string UpdateUser { get; set; }
        [DisplayName("水樣別")]
        public WaterCheckDetailTypeEnum Type { get; set; }
        [DisplayName("原水濁度")]
        public decimal? O_Turbidity { get; set; }
        /// <summary>
        /// 餘氯
        /// </summary>
        [DisplayName("自由有效餘氯")]
        public decimal? Chlorine { get; set; }
        /// <summary>
        /// 餘氯標準值
        /// WaterCheckDetailStandEnum.cs
        /// 1000~2000
        /// </summary>
        public int ChlorineStand { get; set; }
        /// <summary>
        /// 餘氯檢驗方法
        /// </summary>
        public int ChlorineWay { get; set; }
        /// <summary>
        /// 大腸桿菌群
        /// </summary>
        [DisplayName("大腸桿菌群")]
        public decimal? EColi { get; set; }
        /// <summary>
        /// EColiTypeEnum.cs
        /// </summary>
        public int EColiType { get; set; }
        /// <summary>
        /// 大腸桿菌群標準值
        /// WaterCheckDetailStandEnum.cs
        /// 2000~3000
        /// </summary>
        public int EColiStand { get; set; }
        /// <summary>
        /// 大腸桿菌群檢驗方法
        /// </summary>
        public int EColiWay { get; set; }
        /// <summary>
        /// PH值 
        /// </summary>
        [DisplayName("氫離子濃度指數")]
        public decimal? Hydrogen { get; set; }
        /// <summary>
        /// PH值標準值 
        /// WaterCheckDetailStandEnum.cs 
        /// 3000 ~ 4000
        /// </summary>
        public int HydrogenStand { get; set; }
        /// <summary>
        /// PH值檢驗方法
        /// </summary>
        public int HydrogenWay { get; set; }
        /// <summary>
        /// 濁度
        /// </summary>
        [DisplayName("濁度")]
        public decimal? Turbidity { get; set; }
        /// <summary>
        /// 標準值 
        /// WaterCheckDetailStandEnum.cs
        /// 4000~5000
        /// </summary>
        public int TurbidityStand { get; set; }
        /// <summary>
        /// 檢驗方法
        /// </summary>
        public int TurbidityWay { get; set; }

        public string OtherName { get; set; }

        public decimal? OtherValue { get; set; }

        public string OtherWay { get; set; }

        public string Other2Name { get; set; }

        public decimal? Other2Value { get; set; }

        public string Other2Way { get; set; }

        public string Other3Name { get; set; }

        public decimal? Other3Value { get; set; }

        public string Other3Way { get; set; }

        public decimal GpsX { get; set; }
        public decimal GpsY { get; set; }

        public WaterCheckDetailStatusEnum Status { get; set; }

        public int Recheck { get; set; }
    }

}