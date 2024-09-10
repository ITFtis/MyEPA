using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.ViewModels
{
    public class ApplyStatusReportViewModel
    {
        /// <summary>
        /// 縣市ID
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 縣市
        /// </summary>
        [DisplayName("縣市")]
        public string CityName { get; set; }
        /// <summary>
        /// 環境部待審核
        /// </summary>
        [DisplayName("環境部待審核")]
        public int EPAPendingCount { get; set; }
        /// <summary>
        /// 環境部審核中
        /// </summary>
        [DisplayName("環境部審核中")]
        public int EPAProcessingCount { get; set; }
        /// <summary>
        /// 環境部已核定
        /// </summary>
        [DisplayName("環境部已核定")]
        public int EPAConfrimCount { get; set; }
        /// <summary>
        /// 環境部退回
        /// </summary>
        [DisplayName("環境部退回")]
        public int EPARejectCount { get; set; }
        /// <summary>
        /// 環保局待審核
        /// </summary>
        [DisplayName("環保局待審核")]
        public int EPBPendingCount { get; set; }
        /// <summary>
        /// 環保局審核中
        /// </summary>
        [DisplayName("環保局審核中")]
        public int EPBProcessingCount { get; set; }
        /// <summary>
        /// 環保局已核定
        /// </summary>
        [DisplayName("環保局已核定")]
        public int EPBConfrimCount { get; set; }
        /// <summary>
        /// 環保局退回
        /// </summary>
        [DisplayName("環保局退回")]
        public int EPBRejectCount { get; set; }
    }
}