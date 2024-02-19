using System;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class NoticeModel
    {
        [AutoKey]
        public int Id { get; set; }

        public int DiasterId { get; set; }
        [DisplayName("標題")]
        public string Title { get; set; }
        [DisplayName("內容")]
        public string Content { get; set; }
        [DisplayName("上架時間")]
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime UpdateDate { get; set; }

        public string UpdateUser { get; set; }
    }
}