using MyEPA.Models;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace EPASchedule
{
    internal class SysNotice
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute()
        {
            try
            {
                //1.開口合約通知
                logger.Info("1.開口合約通知");
                if (!DoOpenContract())
                {
                    logger.Error("執行失敗");
                }
                else
                {
                    logger.Info("執行成功");
                }

                //2.xxxxxxxxxxx
                
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
        }

        private bool DoOpenContract()
        {
            try
            {
                OpenContractService OpenContractService = new OpenContractService();                

                OpenContractFilterParameter filter = new OpenContractFilterParameter()
                {
                    IsNotice = true,
                };

                List<OpenContractCountModel> datas = OpenContractService.GetCountListByFilter(filter);

                var accounts = new UsersService().GetAll().ToList();

                foreach (OpenContractCountModel v in datas)
                {                    
                    var account = accounts.Where(a => a.UserName == v.CreateUser).FirstOrDefault();                                    ;
                    //紀錄查無主要聯絡人資訊
                    if (account == null)
                    {
                        string errors = string.Format("***無法通知，此帳號查無資料：{0}***", v.CreateUser);
                        logger.Error(errors);
                        continue;
                    }
                    else
                    {
                        string subject = "開口合約資料補齊通知";
                        string content = string.Format(@"

{0}{1}，{2}({3})您好：<br/>
開口合約「{4}」尚有資料未齊全，請儘速補齊。<br/>
如有問題請聯絡EMIS客服專員或曾淑俐小姐（02-2383-2389分機59906）。
",
v.CityName,
v.TownName,
account.Name,
account.UserName,
v.Name);

                        ToSendOpenContract(subject, content, account);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("執行錯誤 - Do");
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);

                return false;
            }

            return true;
        }

        private bool ToSendOpenContract(string subject, string content, UsersModel account)
        {
            bool result = false;

            try
            {
                EmailHelper emailHelper = new EmailHelper();
                MailParam p = new MailParam();
                p.iniParam();
                emailHelper.MailFrom = p.MailFrom;
                emailHelper.MailFromName = p.MailFromName;
                emailHelper.Account = p.Account;
                emailHelper.Password = p.Password;
                emailHelper.MailServer = p.MailServer;
                emailHelper.MailPort = p.MailPort;
                emailHelper.EnableSSL = p.EnableSSL;

                emailHelper.Subject = subject;
                emailHelper.Body = content;

                //收件者
                string addr1 = AppConfig.TestEmailAddress != "" ? AppConfig.TestEmailAddress : account.Email;
                //addr1 = "123";  //xxxxxxxxxxxxxxxxxxxxxxxxx
                emailHelper.AddTo(addr1, account.Name);

                foreach (string addr in AppConfig.EmailAddressResp.Split(','))
                {
                    if (addr != "")
                    {
                        emailHelper.AddCC(addr, "");
                    }
                }

                emailHelper.IsSendEmail = true;
                bool success = emailHelper.SendBySmtp();

                if (!success)
                {
                    logger.Error("ToSend - 信件寄發失敗，Email內容:" + emailHelper.Body.Substring(0, emailHelper.Body.Length / 3));
                }

                result = true;
            }
            catch (Exception ex)
            {
                logger.Error("信件寄發錯誤：" + ex.Message);
                logger.Error(ex.StackTrace);

                return false;
            }

            return result;
        }
    }
}
