using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEPA.Models;
using MyEPA.Services;

namespace EPASchedule
{
    internal class DisinfectantNotice
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //private EsdmsModelContextExt _dbContextEsdms = new EsdmsModelContextExt();
        //private MisModelContext _dbContextMis = new MisModelContext();

        public void Execute()
        {
            try
            {
                if (!Do())
                {
                    logger.Error("執行失敗");
                }
                else
                {
                    logger.Info("執行成功");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
        }

        private bool Do()
        {
            try
            {
                DisinfectantService DisinfectantService = new DisinfectantService();

                //所有藥劑
                var ants = DisinfectantService.GetAll();

                //City, Town, ContactUnit, DrugName
                var tmp = ants.Select(a => new
                {
                    City = a.City,
                    Town = a.Town,
                    ContactUnit = a.ContactUnit,
                    DrugName = a.DrugName,
                    ServiceLife = a.ServiceLife,
                    ServiceLifeDiffDay = DateFormat.ToDiffDays(a.ServiceLife, DateTime.Now),
                });

                //待警示藥劑(母體)
                var datas = tmp.Where(a => a.ServiceLifeDiffDay <= AppConfig.ValidDay).ToList();

                //待警示藥劑單位
                var units = datas.Select(a => new { a.City, a.Town }).Distinct().OrderBy(a => a.City);

                //鄉鎮帳號
                var accounts = new UsersService().GetAll().Where(a => a.MainContacter == "是").ToList();

                //通知
                foreach (var unit in units)
                {
                    var infos = datas.Where(a => a.City == unit.City && a.Town == unit.Town);
                    var account = accounts.Where(a => a.City == unit.City && a.Town == unit.Town).FirstOrDefault();
                    
                    //紀錄查無主要聯絡人資訊
                    if (account == null)
                    {                        
                        var msgs = infos.Select((a, index) => (index + 1).ToString() + "." + "藥劑(" + a.DrugName + ")：效期(" + DateFormat.ToDate14(a.ServiceLife) + "),天數(" + a.ServiceLifeDiffDay + ")");
                        string msg = string.Join("\r\n", msgs);

                        string errors = string.Format("\r***無法通知，因查無此單位主要聯絡人：{0}{1}***\r{2}", 
                                                unit.City, unit.Town, 
                                                msg);
                        logger.Error(errors);
                        continue;
                    }
                    else
                    {
                        //寄發Mail
                        //infos 資訊 + account 收件者帳號
                        var msgs = infos.Select((a, index) => (index + 1).ToString() + "." + a.DrugName + "到期日(" + DateFormat.ToDate14(a.ServiceLife) + ")，剩餘天數(" + a.ServiceLifeDiffDay + ")");
                        string msg = string.Join("\r\n", msgs);

                        string content = string.Format(@"

{0}清潔隊您好，
<br/><br/>

貴局尚有消毒藥劑使用期限即將到期，<br/>
請優先使用以下藥劑以避免逾期藥效失效。
<br/><br/>

{1}

",
unit.City, unit.Town,
msg
);

                        bool done = ToSend(msg, account);
                    }
                }

                return true;
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

        private bool ToSend(string content, UsersModel account)
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

                emailHelper.Subject = "資源預警通報機制 - 使用期限到期";
                emailHelper.Body = content;

                //收件者
                string addr1 = AppConfig.TestEmailAddress != "" ? AppConfig.TestEmailAddress : account.Email;
                //addr1 = "123";  //xxxxxxxxxxxxxxxxxxxxxxxxx
                emailHelper.AddTo(addr1, account.Name);

                foreach (string addr in AppConfig.EmailAddressResp.Split(','))
                {
                    if (addr != "")
                    {
                        emailHelper.AddTo(addr, "");
                    }
                }

                foreach (string addr in AppConfig.EmailAddressCC.Split(','))
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
                    logger.Error("ToSend - 信件寄發失敗:" + emailHelper.ToMails);
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
