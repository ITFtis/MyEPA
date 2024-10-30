using MyEPA.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MyEPA.Models
{
    public class LogDisinfectantModel
    {
        [AutoKey]
        public int Id { get; set; }

        [DisplayName("對應災害主題Id")]
        public int DiasterId { get; set; }

        [DisplayName("縣市")]
        public string City { get; set; }
        [DisplayName("鄉鎮名")]
        public string Town { get; set; }
        [DisplayName("部門")]
        public string ContactUnit { get; set; }
        [DisplayName("藥品名稱")]
        public string DrugName { get; set; }
        [DisplayName("類別")]
        public string DrugType { get; set; }
        [DisplayName("狀態	")]
        public string DrugState { get; set; }
        [DisplayName("數量")]
        public decimal Amount { get; set; }
        [DisplayName("濃度")]
        public string Density { get; set; }
        [DisplayName("可消毒面積")]
        public decimal Area { get; set; }
        [DisplayName("使用年限")]
        public DateTime? ServiceLife { get; set; }

        [DisplayName("是否跨縣市調度")]
        public bool? IsSupportCity { get; set; }
        [DisplayName("跨縣市調度數量")]
        public int? SupportCityNum { get; set; }
        [DisplayName("資料更新日期")]
        public DateTime? UpdateTime { get; set; }
        [DisplayName("資料更新者")]
        public string UpdateUser { get; set; }
        public DateTime? ConfirmTime { get; set; }
        /// <summary>
        /// 參考 DisinfectantTypeEnum.cs
        /// </summary>
        [DisplayName("用途")]
        public int UseType { get; set; }
        public string ActiveIngredients1 { get; set; }
        public string ActiveIngredients2 { get; set; }

        [DisplayName("閥值")]
        public float CtPoint { get; set; }

        [DisplayName("Log建檔日")]
        public DateTime LogBDate { get; set; }

        [DisplayName("Log更新者")]
        public string LogBUser { get; set; }
    }

    public class LogDisinfectantViewModel : LogDisinfectantModel
    {      
        [DisplayName("序號")]
        public string SerialNo { get; set; }


        [DisplayName("現有設備使用年限說明")]
        public string CurYearDesc { get; set; }

        [DisplayName("現有設備數量")]
        public int? CurAmount { get; set; }   
    }
}