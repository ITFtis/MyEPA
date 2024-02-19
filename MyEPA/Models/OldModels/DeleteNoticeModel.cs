using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyEPA.Models
{
    [Table("Notice")]
    public class DeleteNoticeModel
    {
        [AutoKey]
        public string Id { get; set; }
        public string Topic { get; set; }
        public string Content { get; set; }
        public string Time { get; set; }
   
    }
}