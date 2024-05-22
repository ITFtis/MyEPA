using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEPA
{
    public class TestMailParam
    {
        public string ToMails { get; set; }
        public string BCCMails { get; set; }
        public string MailFrom { get; set; }
        public string MailFromName { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public bool EnableSSL { get; set; }
    }
}