using Microsoft.Office.Interop.Word;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MyEPA.Controllers.Rec
{
    public class TestMailController : LoginBaseController
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: TestMail
        public ActionResult Index(string sendMsg = "")
        {
            bool isAdmin = GetIsAdmin();

            EmailHelper model = TempData["_EmailHelper"] as EmailHelper;
            //限定管理者使用
            if (!isAdmin)
            {                
                return RedirectToAction("LoginRedirect", "Home", new { });
            }

            EmailHelper email = new EmailHelper();
            if (model == null || string.IsNullOrEmpty(model.Password))
            {
                TestMailParam test = new TestMailParam();
                test.iniParam();
                email.ToMails = test.ToMails;
                email.BCCMails = test.BCCMails;
                email.MailFrom = test.MailFrom;
                email.MailFromName = test.MailFromName;
                email.Account = test.Account;
                email.Password = test.Password;
                email.MailServer = test.MailServer;
                email.MailPort = test.MailPort;
                email.EnableSSL = test.EnableSSL;
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
            else if (submitButton == "DoSaveJson")
            {
                return DoSaveJson(model);
            }

            return RedirectToAction("Index", model);
        }

        private ActionResult DoSend(EmailHelper model)
        {
            ActionResult result = null;

            //EmailHelper email = (EmailHelper)model.Clone();
            EmailHelper email = model;

            //(1)設定內容 (特休結算通知)
            string body = "";
            string path = AppConfig.HtmlTemplatePath + "0_測試Mail.html";
            using (StreamReader reader = System.IO.File.OpenText(path))
            {
                body = reader.ReadToEnd();
            }

            if (string.IsNullOrEmpty(body))
            {
                logger.Error("ToSend - html body取出無內容：" + path);
                return RedirectToAction("Index", new { model, sendMsg = "html body取出無內容" });
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

            TempData["_EmailHelper"] = model;

            result = RedirectToAction("Index", new { sendMsg = sendMsg });
            
            return result;
        }

        private ActionResult DoSaveJson(EmailHelper model)
        {
            ActionResult result;

            TempData["_EmailHelper"] = model;

            try
            {
                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/FileDatas/json/TestMailParam.json");
                using (var streamWriter = System.IO.File.CreateText(path))
                {
                    var text = new JavaScriptSerializer().Serialize(model);
                    streamWriter.Write(text);
                }
            }
            catch (Exception ex)
            {                
                logger.Error("更新Mail設定值(json)失敗：" + ex.Message);
                logger.Error(ex.StackTrace);
                return RedirectToAction("Index", new { sendMsg = "更新Mail設定值(json)失敗" });
            }

            result = RedirectToAction("Index", new { sendMsg = "更新Mail設定值成功" });

            return result;
        }
    }
}