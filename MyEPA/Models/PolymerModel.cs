using System;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class PolymerViewModel
    {
        public int Id { get; set; }

        [DisplayName("申報日期")]
        public DateTime CreateDate { get; set; }
        [DisplayName("使用單位名稱")]

        public string Department { get; set; }
        [DisplayName("藥劑名稱")]
        public string DrugName { get; set; }
        [DisplayName("使用天數")]
        public int UseDays { get; set; }
    }
    public class PolymerModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int DiasterId { get; set; }
        [DisplayName("使用單位名稱")]

        public string Department { get; set; }
        [DisplayName("負責人")]
        public string MainContacter { get; set; }
        [DisplayName("地址")]
        public string Address { get; set; }
        [DisplayName("電話(傳真號碼)")]
        public string Phone { get; set; }
        [DisplayName("藥劑名稱")]
        public string DrugName { get; set; }
        [DisplayName("化學式")]
        public string ChemicalFormula { get; set; }
        [DisplayName("購買廠商名稱")]
        public string Vendor { get; set; }
        [DisplayName("購買日期")]
        public DateTime BuyDate { get; set; }
        [DisplayName("藥劑量")]
        public decimal Amount { get; set; }
        [DisplayName("藥品保存期限")]
        public DateTime EffectiveDate { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }

    }
}