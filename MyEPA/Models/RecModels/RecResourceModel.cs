using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace MyEPA.Models
{
    public class RecResourceViewModel : RecResourceModel
    {
        [DisplayName("對應調度需求Id 需求")]
        public int RecResourceIdNeed { get; set; }

        [DisplayName("對應調度需求Id 支援")]
        public int RecResourceIdHelp { get; set; }

        [DisplayName("支援數量")]
        public int SetQuantity { get; set; }

        /// <summary>
        /// 以Helps主表，List資料
        /// </summary>
        /// <param name="type">1調度需求,2可提供調度</param>
        /// <param name="vws"></param>
        /// <returns></returns>
        public static List<RecResourceViewModel> Copy(int type, List<RecResourceModel> vws)
        {
            if (vws == null) 
                return new List<RecResourceViewModel>();

            List<RecResourceViewModel> result = new List<RecResourceViewModel>();

            foreach (var v in vws)
            {
                RecResourceViewModel model = new RecResourceViewModel();

                PropertyInfo[] infos = typeof(RecResourceModel).GetProperties();
                foreach (PropertyInfo info in infos)
                {
                    info.SetValue(model, info.GetValue(v, null), null);
                }

                if (type == 1)
                {
                    //1調度需求
                    model.RecResourceIdNeed = v.Id;
                }
                else if (type == 2)
                {
                    //2可提供調度
                    model.RecResourceIdHelp = v.Id;
                }

                result.Add(model);
            }

            return result;
        }        
    }

    public class RecResourceModel
    {
        [AutoKey]
        public int Id { get; set; }

        [DisplayName("對應災害主題Id")]
        public int DiasterId { get; set; }

        [DisplayName("資源調度類型")]
        public int Type { get; set; }

        [DisplayName("縣市")]
        public int CityId { get; set; }

        [DisplayName("調度事由")]
        public string Reason { get; set; }

        [DisplayName("聯絡人")]
        public string ContactPerson { get; set; }

        [DisplayName("聯絡人電話")]
        public string ContactMobilePhone { get; set; }

        [DisplayName("類別")]
        public int TypeItems { get; set; }

        [DisplayName("項目")]
        public string Items { get; set; }

        [DisplayName("細項(規格)")]
        public string Spec { get; set; }

        [DisplayName("數量")]
        public int Quantity { get; set; }

        [DisplayName("單位")]
        public string Unit { get; set; }

        [DisplayName("使用期間(起)")]
        public DateTime USDate { get; set; }

        [DisplayName("使用期間(迄)")]
        public DateTime UEDate { get; set; }

        [DisplayName("資源調度配置狀態")]
        public int Status { get; set; }

        [DisplayName("建檔者")]
        public string CreateUser { get; set; }

        [DisplayName("建檔日")]
        public DateTime CreateDate { get; set; }

        [DisplayName("修改者")]
        public string UpdateUser { get; set; }

        [DisplayName("修改日")]
        public DateTime? UpdateDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm}")]
        [DisplayName("集合時間")]
        public DateTime? GatherDate { get; set; }

        [DisplayName("集合地點")]
        public string GatherPlace { get; set; }

        [DisplayName("報到人")]
        public string CheckPerson { get; set; }

        [DisplayName("報到人電話")]
        public string CheckMobilePhone { get; set; }

        [DisplayName("報到指揮官")]
        public string COPerson { get; set; }

        [DisplayName("指揮官電話")]
        public string COMobilePhone { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm}")]
        [DisplayName("出發時間")]
        public DateTime? GoDate { get; set; }
    }
}