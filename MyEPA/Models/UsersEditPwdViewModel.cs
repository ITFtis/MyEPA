using System;
using System.ComponentModel;

namespace MyEPA.Models
{
    public class UsersEditPwdViewModel
    {
        [DisplayName("原密碼")]
        public string OldPwd { get; set; }
        [DisplayName("新密碼")]
        public string Pwd { get; set; }
        [DisplayName("密碼到期日")]
        public DateTime PwdUpdateDate { get; set; }
    }
}