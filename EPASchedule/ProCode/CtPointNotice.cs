﻿using MyEPA.Models;
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

                LogDisinfectantService DisinfectantRepository = new LogDisinfectantService();

                //低於閥值藥劑
                LogDisinfectantFilterParameter filterAnt = new LogDisinfectantFilterParameter()
                {
                    DiasterIds = new List<int>() { diasterId },
                    Ct = 1,
                };

                var ants = DisinfectantRepository.GetLogDisinfectantCurrentByFilter(filterAnt);

                //測試 xxxxxxxxxxxxxxx
                ants = ants.Where(a => a.City == "宜蘭縣").ToList();
                //xxxxxxxxxxxxxxx

                //City, Town, ContactUnit, DrugName
                var tmp = ants.Select(a => new
                {
                    City = a.City,
                    Town = a.Town,
                    ContactUnit = a.ContactUnit,
                    DrugName = a.DrugName,
                    CtPoint = a.CtPoint,
                    CurAmount = a.CurAmount,
                });

                //待警示藥劑(母體)
                var datas = tmp.ToList();

                //待警示藥劑單位
                var units = datas.Select(a => new { a.City, a.Town, a.ContactUnit }).Distinct().OrderBy(a => a.City);

                //鄉鎮帳號
                var accounts = new UsersService().GetAll().Where(a => a.MainContacter == "是").ToList();

                //通知
                foreach (var unit in units)
                {
                    var infos = datas.Where(a => a.City == unit.City && a.Town == unit.Town && a.ContactUnit == unit.ContactUnit);
                    var account = accounts.Where(a => a.City == unit.City && a.Town == unit.Town).FirstOrDefault();

                    //紀錄查無主要聯絡人資訊
                    if (account == null)
                    {
                        var msgs = infos.Select((a, index) => (index + 1).ToString() + "." + "藥劑(" + a.DrugName + ")");
                        string msg = string.Join("\r\n", msgs);

                        string errors = string.Format("\r***無法通知，因查無此單位主要聯絡人：{0}{1}***\r{2}",
                                                unit.City, unit.Town, unit.ContactUnit,
                                                msg);
                        logger.Error(errors);
                        continue;
                    }
                    else
                    {
                        //寄發Mail
                        //infos 資訊 + account 收件者帳號
                        var aaa = infos.ToList();

                        string msg = "";

                        msg = @"
                <table border='1' Cellpadding='3' Cellspacing='3' width='40%'>
                     <tr>
                        <th width='10%'>項次</th>
                        <th width='30%'>消毒藥劑</th>
                        <th width='20%'>閥值</th>
                        <th width='20%'>現有數量</th>                        
                    </tr>";

                        int index = 0;
                        foreach (var info in infos)
                        {
                            index++;

                            msg = msg + string.Format(@"   
                    <tr>
                        <td align='center'>{0}</td>
                        <td align='center'>{1}</td>
                        <td align='center'>{2}</td>
                        <td align='center'>{3}</td>
                    </tr>

                ", index, info.DrugName, info.CtPoint, info.CurAmount);
                        }

                        msg = msg + @"
                </table>";

                        string content = string.Format(@"

                {0}{1}，聯繫單位名稱({2})，{3}您好：
                <br/><br/>

                貴局消毒藥劑數量低於預警閥值，<br/>
                請儘快採購消毒藥劑以因應環境消毒需求。
                <br/><br/>

                {4}

                ",
unit.City,
unit.Town,
unit.ContactUnit,
account.Name,
msg);

                        bool done = ToSend(content, account);

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