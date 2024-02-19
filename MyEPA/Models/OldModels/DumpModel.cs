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
    public class DumpModel
    {
        [AutoKey]
        public int Id { get; set; }
        [DisplayName("處所名稱")]
        public String ContactUnit { get; set; }
        [DisplayName("X")]
        public decimal Xpos { get; set; }
        [DisplayName("Y")]
        public decimal Ypos { get; set; }
        [DisplayName("縣市")]
        public String City { get; set; }
        [DisplayName("鄉鎮名")]
        public String Town { get; set; }
        [DisplayName("更新時間")]
        public DateTime UpdateTime { get; set; }
        [DisplayName("地址")]
        public String Address { get; set; }
        [DisplayName("面積(平方公尺)")]
        public String Area { get; set; }
        [DisplayName("緊急聯絡人")]
        public String EmergencyContactPerson { get; set; }
        [DisplayName("職稱")]
        public String EmergencyContactPersonTitle { get; set; }
        [DisplayName("緊急聯絡電話(行動電話)")]
        public String EmergencyMobilePhone { get; set; }
        [DisplayName("緊急聯絡電話(日)")]
        public String EmergencyPhoneDay { get; set; }
        [DisplayName("緊急聯絡電話(夜)")]
        public String EmergencyPhoneNight { get; set; }
        [DisplayName("資料確認時間")]
        public DateTime? ConfirmTime { get; set; }
 
    }
}