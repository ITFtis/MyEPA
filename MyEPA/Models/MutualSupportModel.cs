using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class MutualSupportModel
    {
        [AutoKey]
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
        public DateTime CreateDate { get; set; }

        public string CreateUser { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateUser { get; set; }

    }
}