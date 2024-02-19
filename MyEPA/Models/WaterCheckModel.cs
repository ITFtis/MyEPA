using MyEPA.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class WaterCheckDetailStatusViewModel
    {
        /// <summary>
        /// 不合格
        /// </summary>
        [Description("不合格")]
        public int Failed { get; set; }
        /// <summary>
        /// 無災情
        /// </summary>
        [Description("無災情")]
        public int NothingHappened { get; set; }
        /// <summary>
        /// 無法抽驗
        /// </summary>
        [Description("無法抽驗")]
        public int Cannot { get; set; }
        /// <summary>
        /// 無異常
        /// </summary>
        [Description("無異常")]
        public int Success { get; set; }
        [Description("檢驗中")]
        /// <summary>
        /// 檢驗中
        /// </summary>
        public int Testing { get; set; }
    }
    public class WaterCheckViewModel : WaterCheckModel
    {
        public WaterCheckDetailStatusViewModel DetailStatus { get; set; }
        public bool IsHasDateils { get; set; }
    }
    public class WaterCheckModel
    {
        [AutoKey]
        public int Id { get; set; }
        public WaterCheckTypeEnum Type { get; set; }

        public int DiasterId { get; set; }

        public int CityId { get; set; }

        public string CityName { get; set; }

        public int TownId { get; set; }

        public string TownName { get; set; }
        [DisplayName("是否合格")]
        public int Status { get; set; }
        [DisplayName("應抽驗日期")]

        public DateTime CheckDate { get; set; }

        public DateTime UpdateTime { get; set; }

        public string UpdateUser { get; set; }
        [DisplayName("備註")]
        public string Memo { get; set; }

    }

}