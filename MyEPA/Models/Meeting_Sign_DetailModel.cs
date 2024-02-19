    using System;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class Meeting_Sign_DetailModel
    {
        [AutoKey]
        public int Row_ID { get; set; }

        public int Meeting_ID { get; set; }
        [DisplayName("職稱")]
        public string Duties { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; }
        [DisplayName("性別")]
        public string Sex { get; set; }
        [DisplayName("生日")]
        public string Birthday { get; set; }
        [DisplayName("身份證字號")]
        public string ID_Number { get; set; }
        [DisplayName("科/課別")]
        public string Unit_name { get; set; }
        [DisplayName("聯絡電話")]
        public string Tel { get; set; }
        [DisplayName("行動電話")]
        public string CellPhone { get; set; }
        [DisplayName("EMAIL")]
        public string Email { get; set; }
        [DisplayName("備註")]
        public string Memo { get; set; }
        [DisplayName("餐點")]
        public string Meals { get; set; }
        [DisplayName("輸入日期")]
        public DateTime? Keyin_date { get; set; }
        [DisplayName("來源IP")]
        public string Source_IP { get; set; }
        [DisplayName("縣市")]
        public string City { get; set; }
        [DisplayName("鄉鎮市區")]
        public string Town { get; set; }
    }
}