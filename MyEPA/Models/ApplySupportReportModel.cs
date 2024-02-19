using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class ApplySupportReportModel
    {
        public string TitleString { get; set; }
        public string QuantityField { get; set; }
        
        /// <summary>
        /// 用來顯示摘要部分
        /// </summary>
        public string AbstractString { get; set; }
        public string TotalCountString { get; set; }
        public string TotalQuantityString { get; set; }
        public string TotalCountToEpaString { get; set; }
        public List<ApplySupportReportDetailModel> Details { get; } = new List<ApplySupportReportDetailModel>();

        public void AddDetail(List<ApplySupportReportDetailModel> detailModels) 
        {
            Details.AddRange(detailModels);
        }

        public void ClearDetail()
        {
            Details.Clear();
        }
    }
}