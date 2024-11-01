using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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

                ////////測試 xxxxxxxxxxxxxxx
                //////ants = ants.Where(a => a.City == "宜蘭縣").ToList();
                ////////xxxxxxxxxxxxxxx

                //City, Town, ContactUnit(聯繫單位名稱), DrugName
                var tmp = ants.Select(a => new
                {
                    City = a.City,
                    Town = a.Town,
                    ContactUnit = a.ContactUnit,
                    DrugName = a.DrugName,
                    Amount = a.Amount,
                    ServiceLife = a.ServiceLife,
                    ServiceLifeDiffDay = DateFormat.ToDiffDays(a.ServiceLife, DateTime.Now),
                });

                //1.資料
                //待警示藥劑(母體)
                var datas = tmp.Where(a => a.ServiceLifeDiffDay <= AppConfig.ValidDay).ToList();

                //待警示藥劑單位
                var units = datas.Select(a => new { a.City, a.Town, a.ContactUnit }).Distinct().OrderBy(a => a.City);

                //鄉鎮帳號
                var accounts = new UsersService().GetAll().Where(a => a.MainContacter == "是").ToList();

                //信件內容
                List<TotalUnitMsg> totalMsgs = new List<TotalUnitMsg>();
                CityService cityService = new CityService();

                foreach (var unit in units)
                {
                    TotalUnitMsg aaa = new TotalUnitMsg();
                    var infos = datas.Where(a => a.City == unit.City && a.Town == unit.Town && a.ContactUnit == unit.ContactUnit);

                    string msg = "";

                    msg = @"
<table border='1' Cellpadding='3' Cellspacing='3' width='70%'>
     <tr>
        <th width='10%'>項次</th>
        <th width='15%'>縣市</th>
        <th width='15%'>單位</th>
        <th width='20%'>消毒藥劑</th>
        <th width='10%'>數量</th>
        <th width='10%'>到期日</th>
        <th width='10%'>剩餘天數</th>
    </tr>";

                    int index = 0;
                    foreach (var info in infos)
                    {
                        index++;

                        //超過期限
                        string alertStyle = info.ServiceLifeDiffDay < 0 ? "style='color:red'" : "";

                        msg = msg + string.Format(@"   
    <tr>
        <td align='center'>{0}</td>
        <td align='center'>{1}</td>
        <td align='center'>{2}</td>
        <td align='center'>{3}</td>
        <td align='center'>{4}</td>
        <td align='center'>{5}</td>
        <td align='center' {7}>{6}</td>
    </tr>

", index, info.City, info.ContactUnit, 
info.DrugName, info.Amount, 
DateFormat.ToDate14(info.ServiceLife), info.ServiceLifeDiffDay, alertStyle);
                    }

                    msg = msg + @"
</table>";
                    
                    int citySort = 0;
                    var city = cityService.GetByCityName(unit.City);
                    if (city != null)
                    {
                        citySort = city.Sort;
                    }

                    totalMsgs.Add(new TotalUnitMsg { City = unit.City, Town = unit.ContactUnit, Msg = msg, CitySort = citySort });
                }

                //2.通知(totalMsgs)
                totalMsgs = totalMsgs.OrderBy(a => a.CitySort).ToList();
                //(1)清潔隊信件
                foreach (var v in totalMsgs)
                {
                    var account = accounts.Where(a => a.City == v.City && a.Town == v.Town).FirstOrDefault();

                    //紀錄查無主要聯絡人資訊
                    if (account == null)
                    {
                        //(\r換行)
                        string errors = string.Format("***(清潔隊)無法通知，無此單位主要聯絡人：{0}{1}***", v.City, v.Town);
                        logger.Error(errors);
                        continue;
                    }
                    else
                    {
                        //寄發Mail
                        //v 資訊 + account 收件者帳號
                        string subject = "(清潔隊)資源預警通報機制 - 使用期限到期";

                        string content = string.Format(@"
{0}{1}，{2}您好：
<br/><br/>

貴局尚有消毒藥劑使用期限即將到期，<br/>
請優先使用以下藥劑以避免逾期藥效失效。
<br/><br/>

{3}",
v.City,
v.Town,
account.Name,
v.Msg);

                        bool done = ToSend(subject, content, account);
                    }
                }

                //(2).環保局信件
                var citys = cityService.GetAll().Where(a => a.Type == 0);
                foreach (var city in citys)
                {
                    //有警示資料才要send
                    var totals = totalMsgs.Where(a => a.City == city.City).ToList();
                    if (totals.Count() == 0)
                        continue;

                    var account = accounts.Where(a => a.City == city.City).FirstOrDefault();
                    //紀錄查無主要聯絡人資訊
                    if (account == null)
                    {
                        string errors = string.Format("***(環保局)無法通知，無此單位主要聯絡人：{0}***", city.City);
                        logger.Error(errors);
                        continue;
                    }
                    else
                    {
                        //寄發Mail
                        //v 資訊 + account 收件者帳號
                        string subject = "(環保局)資源預警通報機制 - 使用期限到期";


                        string CityMsg = string.Join("<br/>", totals.Select(a => a.Msg));
                        string content = string.Format(@"
{0}，{1}您好：
<br/><br/>

以下清潔隊尚有消毒藥劑使用期限即將到期，<br/>
請優先使用以下藥劑以避免逾期藥效失效。
<br/><br/>

{2}",
city.City,
account.Name,
CityMsg);

                        bool done = ToSend(subject, content, account);
                    }
                }

                //(3).中央單位(環衛組與綜規組)信件
                foreach (string addr in AppConfig.EmailAddressGov.Split(','))
                {
                    //寄發Mail
                    //v 資訊 + account 收件者帳號
                    string subject = "(中央單位)資源預警通報機制 - 使用期限到期";

                    UsersModel account = new UsersModel()
                    {
                        Name = addr,
                        Email = addr
                    };

                    string GovMsg = string.Join("<br/>", totalMsgs.Select(a => a.Msg));
                    string content = string.Format(@"
中央單位，您好：
<br/><br/>

以下清潔隊尚有消毒藥劑使用期限即將到期，<br/>
請優先使用以下藥劑以避免逾期藥效失效。
<br/><br/>

{0}",
                    GovMsg);

                    bool done = ToSend(subject, content, account);
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

        private bool ToSend(string subject, string content, UsersModel account)
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
