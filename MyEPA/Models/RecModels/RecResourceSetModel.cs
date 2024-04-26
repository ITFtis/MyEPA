using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class RecResourceSetModel
    {
        public RecResourceSetModel() { }
        public RecResourceSetModel(RecResourceModel rec)
        {
            if (rec != null)
            {
                RecResourceId = rec.Id;
                SetCityId = rec.CityId;
                SetContactPerson = rec.ContactPerson;
                SetItems = rec.Items;
                SetSpec = rec.Spec;
                SetQuantity = rec.Quantity;
                SetUnit = rec.Unit;
            }
        }

        [AutoKey]
        public int Id { get; set; }

        [DisplayName("對應調度需求Id")]
        public int RecResourceId { get; set; }

        [DisplayName("縣市")]
        public int SetCityId { get; set; }

        [DisplayName("聯絡人")]
        public string SetContactPerson { get; set; }

        [DisplayName("項目")]
        public string SetItems { get; set; }

        [DisplayName("細項(規格)")]
        public string SetSpec { get; set; }

        [DisplayName("數量")]
        public int SetQuantity { get; set; }

        [DisplayName("單位")]
        public string SetUnit { get; set; }

        [DisplayName("建檔者")]
        public string CreateUser { get; set; }

        [DisplayName("建檔日")]
        public DateTime CreateDate { get; set; }

        [DisplayName("修改者")]
        public string UpdateUser { get; set; }

        [DisplayName("修改日")]
        public DateTime? UpdateDate { get; set; }
    }
}