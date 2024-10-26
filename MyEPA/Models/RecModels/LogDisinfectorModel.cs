using MyEPA.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class LogDisinfectorModel
    {
        [AutoKey]
        public int Id { get; set; }

        [DisplayName("對應災害主題Id")]
        public int DiasterId { get; set; }

        [DisplayName("縣市")]
        public String City { get; set; }
        [DisplayName("鄉鎮名")]
        public String Town { get; set; }
        [DisplayName("部門")]
        public String ContactUnit { get; set; }
        [DisplayName("消毒設備名稱")]
        public String DisinfectInstrument { get; set; }
        [DisplayName("規格")]
        public String Standard { get; set; }
        [DisplayName("數量")]
        public String Amount { get; set; }
        [DisplayName("購置年份")]
        public String ROCyear { get; set; }
        [DisplayName("是否跨縣市調度")]
        public bool? IsSupportCity { get; set; }
        [DisplayName("跨縣市調度數量")]
        public int? SupportCityNum { get; set; }
        [DisplayName("資料更新日期")]
        public DateTime? UpdateTime { get; set; }
        [DisplayName("資料更新者")]
        public string UpdateUser { get; set; }
        public DateTime? ConfirmTime { get; set; }
        public int? UseType { get; set; }

        [DisplayName("閥值")]
        public float CtPoint { get; set; }

        [DisplayName("Log建檔日")]
        public DateTime LogBDate { get; set; }

        [DisplayName("Log更新者")]
        public string LogBUser { get; set; }
    }
}