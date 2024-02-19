using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.ViewModels
{
    public class FacilityDamageViewModel
    {
        public int Id { get; set; }
        [DisplayName("縣市")]
        public string CityName { get; set; }
        [DisplayName("鄉鎮")]
        public string TownName { get; set; }
        [DisplayName("災害日期")]
        public DateTime ReportDay { get; set; }
        [DisplayName("通報時間")]
        public DateTime CreateDate { get; set; }
        [DisplayName("場所名稱")]
        public List<string> Places { get; set; }
        [DisplayName("損壞說明")]
        public string DamageDesc { get; set; }
        [DisplayName("總隊處理情形")]
        public string ProcessDesc { get; set; }
        [DisplayName("備註")]
        public string Note { get; set; }
    }
}