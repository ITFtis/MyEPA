using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MyEPA.Controllers.Rec
{
    public class TestMailController : LoginBaseController
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: TestMail
        public ActionResult Index(EmailHelper model = null, string sendMsg = "")
        {
            bool isAdmin = GetIsAdmin();

            //限定管理者使用
            if (!isAdmin)
            {                
                return RedirectToAction("LoginRedirect", "Home", new { });
            }

            EmailHelper email = new EmailHelper();
            if (model == null || string.IsNullOrEmpty(model.Password))
            {
                using (StreamReader sr = new StreamReader(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(("~/FileDatas/json")), "TestMailParam.json")))
                {
                    string text = sr.ReadToEnd().Replace("\r\n", "");
                    TestMailParam obj = Newtonsoft.Json.JsonConvert.DeserializeObject<TestMailParam>(text);

                    email.ToMails = obj.ToMails;
                    email.BCCMails = obj.BCCMails;
                    email.MailFrom = obj.MailFrom;
                    email.MailFromName = obj.MailFromName;
                    email.Account = obj.Account;
                    email.Password = obj.Password;
                    email.MailServer = obj.MailServer;
                    email.MailPort = obj.MailPort;
                    email.EnableSSL = obj.EnableSSL;
                }
            }
            else
            {
                email = model;
            }

            ViewBag.SendMsg = sendMsg;

            return View(email);
        }

        public ActionResult MyAction(string submitButton, EmailHelper model)
        {
            if (submitButton == "DoSend")
            {
                return DoSend(model);
            }

            return RedirectToAction("Index", model);
        }

        private ActionResult DoSend(EmailHelper model)
        {
            ActionResult result = null;

            EmailHelper email = (EmailHelper)model.Clone();

            //(1)設定內容 (特休結算通知)
            string body = "";
            string path = AppConfig.HtmlTemplatePath + "0_測試Mail.html";
            using (StreamReader reader = System.IO.File.OpenText(path))
            {
                body = reader.ReadToEnd();
            }

            if (string.IsNullOrEmpty(body))
            {
                logger.Error("ToSend - body取出無內容：" + path);
                return RedirectToAction("Index", model);                
            }

            var user = GetUserBrief();
            body = body.Replace("[DateNow]", DateFormat.ToDate4(DateTime.Now))
                    .Replace("[LoginUser]", user.Name);


            //(2)設定Helper            
            string subject = "測試信件(TestMail)";
            email.Subject = subject;
            email.Body = body;

            foreach (string addr in email.ToMails.Split(','))
            {
                if (addr != "")
                {
                    email.AddTo(addr, "");
                }
            }

            foreach (string addr in email.BCCMails.Split(','))
            {
                if (addr != "")
                {
                    email.AddBCC(addr, "");
                }
            }

            email.IsSendEmail = true;
            bool success = email.SendBySmtp();
            if (!success)
            {
                logger.Error("ToSend - 信件寄發失敗:" + email.ToMails);
            }

            string sendMsg;
            if (success)
            {
                sendMsg = "信件已寄出";
            }
            else
            {
                sendMsg = "信件寄發失敗";
            }

            result = RedirectToAction("Index", new { model, sendMsg = sendMsg });
            
            return result;
        }
    }
}