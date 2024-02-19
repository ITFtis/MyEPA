using System;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class DefendTownQuestionModel
    {
        [DisplayName("鄉鎮名")]
        public string TownName { get; set; }
        [DisplayName("項目")]
        public string QuestionTitle { get; set; }
        public string Note { get; set; }
        [DisplayName("是/否")]
        public bool Ans { get; set; }
        [DisplayName("說明")]
        public string Remark { get; set; }
        public int Sort { get; set; }
        [DisplayName("最新異動時間")]
        public DateTime UpdateDate { get; set; }
    }
}