using System.Collections.Generic;

namespace MyEPA.Models
{
    public class OpenContractFilterParameter
    {
        public List<int> CityIds { get; set; }
        public List<int> ResourceTypeIds { get; set; }
        public int? YearRange { get; set; }
        public List<int> TownIds { get; set; }
        public bool? IsEffective { get; internal set; }
        public bool? IsEPB { get; internal set; }
        
        /// <summary>
        /// 開口合約資料不齊，需信件通知
        /// </summary>
        public bool? IsNotice { get; set; }
    }
}