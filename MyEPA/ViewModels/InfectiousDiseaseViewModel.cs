using MyEPA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEPA.ViewModels
{
    public class InfectiousDiseaseViewModel : InfectiousDiseaseModel
    {
        [DisplayName("縣市")]
        public string CityName { get; set; }
        [DisplayName("鄉鎮")]
        public string TownName { get; set; }
    }

    public class InfectiousDiseaseStatisticsTownViewModel : InfectiousDiseaseStatisticsBaseModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int TownId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("鄉鎮名稱")]
        public string TownName { get; set; }
    }
    public class InfectiousDiseaseStatisticsCityViewModel : InfectiousDiseaseStatisticsBaseModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("縣市名稱")]
        public string CityName { get; set; }
    }
    public class InfectiousDiseaseStatisticsBaseModel
    {
        [DisplayName("居家隔離清運戶數")]
        public int HomeQuarantineCount { get; set; }
        [DisplayFormat(DataFormatString = "{0}", ApplyFormatInEditMode = true)]
        [DisplayName("居家隔離重量(公斤)")]
        public decimal HomeQuarantineGarbageAmount { get; set; }
        [DisplayName("居家檢疫清運戶數")]
        public int HomeInspectionCount { get; set; }
        [DisplayFormat(DataFormatString = "{0}", ApplyFormatInEditMode = true)]
        [DisplayName("居家檢疫清運重量(公斤)")]
        public decimal HomeInspectionGarbageAmount { get; set; }
        [DisplayName("檢疫旅館清運間數")]
        public int InspectionHotelCount { get; set; }
        [DisplayFormat(DataFormatString = "{0}", ApplyFormatInEditMode = true)]
        [DisplayName("檢疫旅館清運重量(公斤)")]
        public decimal InspectionHotelGarbageAmount { get; set; }
        [DisplayName("口罩棄置稽查點次")]
        public int MaskCheckTimes { get; set; }
        [DisplayName("告發件數")]
        public int ReportTimes { get; set; }
    }
}