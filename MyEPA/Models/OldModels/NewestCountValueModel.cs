using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyEPA.Models
{
    [Table("NewestCountValue")]
    public class NewestCountValueModel
    {
        [AutoKey]
        public string Id { get; set; }
        public string City{ get; set; }
        public string Disinfector { get; set; }
        public string Disinfectant { get; set; }
        public string Dump { get; set; }
        public string Toilet { get; set; }
        public string SolidDisinfectant { get; set; }
        public string LiquidDisinfectant { get; set; }
        public string Users { get; set; }
        public string Pest { get; set; }
        public string Vehicle { get; set; }
        public string Volunteer { get; set; }
        public string Reporter { get; set; }
        public string ReportPhone { get; set; }
        public string UpdateTime { get; set; }
        public string ConfirmTime { get; set; }
    }
}