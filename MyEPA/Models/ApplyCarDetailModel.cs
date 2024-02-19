using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class ApplyCarDetailModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int ApplyCarId { get; set; }

        /// <summary>
        /// 車輛類別Id(對應 db ApplyCardType)
        /// </summary>
        public int ApplyCarTypeId { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 轄區調度情況描述
        /// </summary>
        public string PrefectureDescription { get; set; }

        /// <summary>
        /// 附近縣市轄區調度情況描述
        /// </summary>
        public string NearPrefectureDescription { get; set; }


        /// <summary>
        /// 開口合約調度情況描述
        /// </summary>
        public string OpenContractDescription { get; set; }
    }
}