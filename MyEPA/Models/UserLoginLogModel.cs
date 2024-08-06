using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MyEPA.Models
{
    public class UserLoginLogModel
    {
        [AutoKey]
        public int Serial { get; set; }

        [DisplayName("UsersId")]
        public int Id { get; set; }

        [DisplayName("帳號")]
        public string UserName { get; set; }

        [DisplayName("登入時間")]
        public DateTime logintime { get; set; }

        //1.成功2.失敗
        [DisplayName("登入是否成功")]
        public int Type { get; set; }

        [DisplayName("登入輸入密碼")]
        public string PwdKeyIn { get; set; }

        [DisplayName("來源IP")]
        public string SourceIP { get; set; }

        [DisplayName("是否納入登入失敗次數")]
        public bool IsOver { get; set; }

    }
}