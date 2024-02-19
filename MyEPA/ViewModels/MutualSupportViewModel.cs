using MyEPA.Models;
using System;
using System.ComponentModel;

namespace MyEPA.ViewModels
{
    public class MutualSupportSearchViewModel
    {
        public int Id { get; set; }

        public int ResourceTypeId { get; set; }

        [DisplayName("支援協定種類")]
        public string ResourceType { get; set; }

        [DisplayName("被支援單位")]
        public string AcceptSection { get; set; }

        [DisplayName("支援單位")]
        public string Section { get; set; }

        [DisplayName("支援協定時間(起)")]
        public DateTime StartDate { get; set; }

        [DisplayName("支援協定時間(訖)")]
        public DateTime EndDate { get; set; }

        [DisplayName("支援協定類型")]
        public int SupportType { get; set; }

        [DisplayName("支援協定類型")]
        public string SupportTypeName { get; set; }

        [DisplayName("支援協定事項")]
        public string SupportContent { get; set; }

        [DisplayName("備註")]
        public string Memo { get; set; }
    }
    public class MutualSupportViewModel
    {
        public int Id { get; set; }

        public int ResourceTypeId { get; set; }
        [DisplayName("被支援單位")]
        public string AcceptSection { get; set; }
        [DisplayName("支援單位")]

        public string Section { get; set; }
        [DisplayName("支援協定時間(起)")]

        public DateTime StartDate { get; set; }
        [DisplayName("支援協定時間(訖)")]

        public DateTime EndDate { get; set; }
        [DisplayName("支援協定類型")]

        public int SupportType { get; set; }
        [DisplayName("支援協定事項")]

        public string SupportContent { get; set; }
        [DisplayName("備註")]

        public string Memo { get; set; }
        public FileDataModel FileData { get; set; }
    }
}