﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class RecResourceModel
    {
        [AutoKey]
        public int Id { get; set; }

        [DisplayName("對應災害主題Id")]
        public string DiasterId { get; set; }

        [DisplayName("類型(1需求2提供)")]
        public int Type { get; set; }

        [DisplayName("縣市")]
        public int CityId { get; set; }

        [DisplayName("調度事由")]
        public string Reason { get; set; }

        [DisplayName("聯絡人")]
        public string ContactPerson { get; set; }

        [DisplayName("項目")]
        public string Items { get; set; }

        [DisplayName("細項(規格)")]
        public string Spec { get; set; }

        [DisplayName("數量")]
        public int Quantity { get; set; }

        [DisplayName("單位")]
        public string Unit { get; set; }

        [DisplayName("使用時間(起)")]
        public string USDate { get; set; }

        [DisplayName("使用時間(迄)")]
        public string UEDate { get; set; }

        [DisplayName("狀態(1.未完成2.已完成)")]
        public string Status { get; set; }

        [DisplayName("建檔者")]
        public string CreateUser { get; set; }

        [DisplayName("建檔日")]
        public DateTime CreateDate { get; set; }

        [DisplayName("修改者")]
        public string UpdateUser { get; set; }

        [DisplayName("修改日")]
        public string UpdateDate { get; set; }
    }
}