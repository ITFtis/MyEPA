using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class ToiletCleaningLogModel
    {
        [AutoKey]
        public int Id { get; set; }
        public int ToiletLocationId { get; set; }
        [DisplayName("清理記錄日期")]
        public DateTime Date { get; set; }
        [DisplayName("是否清潔")]
        public bool IsClean { get; set; }
        [DisplayName("狀況說明")]
        public string Description { get; set; }
       
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}