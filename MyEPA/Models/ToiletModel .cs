using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Web.Configuration;
using System.ComponentModel;
namespace MyEPA.Models
{
    public class ToiletModel
    {
        [AutoKey]
        public int Id { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public string Unit { get; set; }
        public decimal Amount{ get; set; }
        public string SlotNumber { get; set; }
        public string ToiletType { get; set; }
        public string ROCyear{ get; set; }
        public DateTime UpdateTime { get; set; }
        [DisplayName("資料更新者")]
        public string UpdateUser { get; set; }
        public DateTime? ConfirmTime { get; set; }
    }
}