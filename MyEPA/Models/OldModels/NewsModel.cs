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
    public class NewsModel : BaseModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int DiasterId { get; set; }
        [DisplayName("標題")]

        public string Title { get; set; }
        [DisplayName("內容")]
        public string Content { get; set; }
    }
}