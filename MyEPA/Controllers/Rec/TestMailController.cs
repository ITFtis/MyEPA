using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers.Rec
{
    public class TestMailController : LoginBaseController
    {
        // GET: TestMail
        public ActionResult Index()
        {
            bool isAdmin = GetIsAdmin();

            //限定管理者使用
            if (!isAdmin)
            {                
                return RedirectToAction("LoginRedirect", "Home", new { });
            }

            EmailHelper email = new EmailHelper();            
            email.MailFrom = "ftis@meet.ftis.org.tw";
            email.MailFromName = "產基會測試信";
            email.Account = "ftis@meet.ftis.org.tw";
            email.Password = "Ftis01801726";
            email.MailServer = "120.100.100.240";
            email.MailPort = 25;
            email.EnableSSL = false;

            return View(email);
        }
        
        public ActionResult TestSend(EmailHelper email)
        {
            return RedirectToAction("Index");
        }
    }
}