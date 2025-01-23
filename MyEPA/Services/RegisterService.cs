using Microsoft.Win32;
using MyEPA.Models;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.Windows.Interop;

namespace MyEPA.Services
{
    public class RegisterService
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        RegistersRepository registersRepository = new RegistersRepository();
        UsersRepository userRepository = new UsersRepository();

        public AdminResultModel Register(RegistersModel model, List<string> humanType)
        {
            if (userRepository.GetByUserName(model.Id) != null)
            {
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "抱歉，與現有用戶同帳號，請換帳號名稱後申請"
                };
            }

            if (registersRepository.Get(model.Id) != null)
            {
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "抱歉，此帳號已經存在，待審核中！"
                };
            }

            model.HumanType = string.Join("、", humanType ?? new List<string>());

            try
            {

                if (!string.IsNullOrWhiteSpace(model.Duty))
                {
                    var foundDuty = new DutyRepository().GetByDutyName(model.Duty);
                    model.DutyId = foundDuty?.Id;
                }

                if (!string.IsNullOrWhiteSpace(model.City))
                {
                    var foundCity = new CityRepository().GetByCityName(model.City);
                    model.CityId = foundCity?.Id;
                }

                if (model.CityId.HasValue && !string.IsNullOrWhiteSpace(model.Town))
                {
                    var foundTown = new TownRepository().GetByTownName(model.CityId.Value, model.Town);
                    model.TownId = foundTown?.Id;

                    if(model.TownId == null)
                    {
                        return new AdminResultModel
                        {
                            IsSuccess = false,
                            ErrorMessage = "抱歉，此鄉鎮並不存在資料表，請通知系統管理員！" + model.Town
                        };
                    }
                }

                if ((model.CityId != null && model.TownId != null))
                {
                    UsersRepository UsersRepository = new UsersRepository();
                    if (model.MainContacter == "是" && UsersRepository.IsExistsByMainContacter((int)model.CityId, (int)model.TownId))
                    {

                        return new AdminResultModel
                        {
                            IsSuccess = false,
                            ErrorMessage = "抱歉，主要負責人已存在，不可重覆設定：" + model.City + model.Town
                        };
                    }
                }

                //信件通知負責人
                ToSend(model);

                registersRepository.Create(model);
            }
            catch (Exception ex)
            {
                return new AdminResultModel
                {
                    IsSuccess = false,
                    ErrorMessage = "註冊失敗"
                };
            }

            return new AdminResultModel
            {
                IsSuccess = true,
                ErrorMessage = "已登錄成功，請等待審核"
            };
        }

        private bool ToSend(RegistersModel reg)
        {
            bool result = false;

            try
            {
                string content = string.Format(@"
系統負責人您好，「環境災害管理資訊系統」新帳號申請人：
<br/><br/>

帳號：{0}<br/>
姓名：{1}<br/>
機關類別(角色)：{2}<br/>
機關名稱(縣市)：{3}<br/>
單位(鄉鎮區)名稱：{4}<br/>
來源IP：{5}

",
reg.Id,
reg.Name,
reg.Duty,
reg.City,
reg.Town,
reg.SourceIP
);

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

                emailHelper.Subject = "環境災害管理資訊系統，新帳號註冊通知";
                emailHelper.Body = content;
                foreach (string addr in AppConfig.EmailAddressResp.Split(','))
                {
                    if (addr != "")
                    {
                        emailHelper.AddTo(addr, "");
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