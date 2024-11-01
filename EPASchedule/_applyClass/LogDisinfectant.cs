using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPASchedule
{
    internal class LogDisinfectant
    {
        [DisplayName("類型")]
        public String Type { get; set; }
        [DisplayName("縣市")]
        public String City { get; set; }
        [DisplayName("鄉鎮名")]
        public String Town { get; set; }
        [DisplayName("部門")]
        public String ContactUnit { get; set; }
        [DisplayName("消毒設備名稱")]
        public String DrugName { get; set; }
        [DisplayName("閥值")]
        public float CtPoint { get; set; }
        [DisplayName("現有設備數量")]
        public float? CurAmount { get; set; }
    }
}
