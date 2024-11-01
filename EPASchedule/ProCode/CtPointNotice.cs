using MyEPA.Models;
using MyEPA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEPA.Repositories;
using MyEPA.Models.FilterParameter;

namespace EPASchedule
{
    internal class CtPointNotice
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                //1.設定災害期間資料
                DiasterRepository DiasterRepository = new DiasterRepository();
                var diasters = DiasterRepository.GetList();
                var date = DateTime.Parse(DateTime.Now.ToShortDateString());
                var ds  = diasters.Where(a => date >= a.StartTime && date <= a.EndTime).ToList();
                if (ds.Count == 0)
                {
                    //當天日期不存在災害期間內
                    logger.Info("當天日期不存在災害期間內，無需通知：" + DateFormat.ToDate4(date));
                    return true;
                }

                var diasterId = ds.FirstOrDefault().Id;

                //閥值設備
                LogDisinfectorService LogDisinfectorService = new LogDisinfectorService();

                //低於閥值設備
                LogDisinfectorFilterParameter filterOr = new LogDisinfectorFilterParameter()
                {
                    DiasterIds = new List<int>() { diasterId },
                    Ct = 1,
                };

                var ors = LogDisinfectorService.GetLogDisinfectorCurrentByFilter(filterOr);
                
                //閥值藥劑
                LogDisinfectantService DisinfectantRepository = new LogDisinfectantService();

                //低於閥值藥劑
                LogDisinfectantFilterParameter filterAnt = new LogDisinfectantFilterParameter()
                {
                    DiasterIds = new List<int>() { diasterId },
                    Ct = 1,
                };

                var ants = DisinfectantRepository.GetLogDisinfectantCurrentByFilter(filterAnt);

                var tmp1 = ors.Select(a => new LogDisinfectant
                {
                    Type = "設備",
                    City = a.City,
                    Town = a.Town,
                    ContactUnit = a.ContactUnit,
                    DrugName = a.DisinfectInstrument,
                    CtPoint = a.CtPoint,
                    CurAmount = a.CurAmount,
                }).ToList();
                var tmp2 = ants.Select(a => new LogDisinfectant
                {
                    Type = "藥劑",
                    City = a.City,
                    Town = a.Town,
                    ContactUnit = a.ContactUnit,
                    DrugName = a.DrugName,
                    CtPoint = a.CtPoint,
                    CurAmount = a.CurAmount,
                }).ToList();
                var tmp = tmp1.Concat(tmp2);

                //2.資料
                //待警示藥劑(母體)
                var datas = tmp.ToList();

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

                    //設備
                    string msg1 = "";                   
                    var v1s = infos.Where(a => a.Type == "設備");
                    if (v1s.Count() > 0)
                    {
                        msg1 = @"
<table border='1' Cellpadding='3' Cellspacing='3' width='70%'>
     <tr>
        <th width='10%'>項次</th>
        <th width='15%'>縣市</th>
        <th width='15%'>單位</th>
        <th width='20%'>消毒設備</th>
        <th width='10%'>閥值</th>
        <th width='10%'>現有數量</th>
    </tr>";

                        int index = 0;
                        foreach (var info in v1s)
                        {
                            index++;

                            msg1 = msg1 + string.Format(@"   
    <tr>
        <td align='center'>{0}</td>
        <td align='center'>{1}</td>
        <td align='center'>{2}</td>
        <td align='center'>{3}</td>
        <td align='center'>{4}</td>
        <td align='center'>{5}</td>
    </tr>

", index, info.City, info.ContactUnit,
    info.DrugName, info.CtPoint, info.CurAmount);
                        }

                        msg1 = msg1 + @"
</table>";
                    }

                    //藥劑
                    string msg2 = "";
                    var v2s = infos.Where(a => a.Type == "藥劑");
                    if (v2s.Count() > 0)
                    {
                        msg2 = @"
<table border='1' Cellpadding='3' Cellspacing='3' width='70%'>
     <tr>
        <th width='10%'>項次</th>
        <th width='15%'>縣市</th>
        <th width='15%'>單位</th>
        <th width='20%'>消毒藥劑</th>
        <th width='10%'>閥值</th>
        <th width='10%'>現有數量</th>
    </tr>";

                        int index = 0;
                        foreach (var info in v2s)
                        {
                            index++;

                            msg2 = msg2 + string.Format(@"   
    <tr>
        <td align='center'>{0}</td>
        <td align='center'>{1}</td>
        <td align='center'>{2}</td>
        <td align='center'>{3}</td>
        <td align='center'>{4}</td>
        <td align='center'>{5}</td>
    </tr>

", index, info.City, info.ContactUnit,
    info.DrugName, info.CtPoint, info.CurAmount);
                        }

                        msg2 = msg2 + @"
</table>";
                    }

                    string msg = msg1 + msg2;

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
                        string subject = "(環保局)資源預警通報機制 - 數量低於閥值";


                        string CityMsg = string.Join("<br/>", totals.Select(a => a.Msg));
                        string content = string.Format(@"
{0}，{1}您好：
<br/><br/>

貴局消毒藥劑數量低於預警閥值，<br/>
請儘快採購消毒藥劑以因應環境消毒需求。
<br/><br/>

{2}",
city.City,
account.Name,
CityMsg);

                        bool done = ToSend(subject, content, account);
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

                emailHelper.Subject = "資源預警通報機制 - 數量低於閥值到期通知";
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
