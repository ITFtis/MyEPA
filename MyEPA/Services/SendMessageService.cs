using MyEPA.Helper;
using MyEPA.Models;
using MyEPA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyEPA.Services
{
    public class SendMessageService
    {
        SendTextLogRepository SendTextLogRepository = new SendTextLogRepository();
        SendTextLogDetailRepository SendTextLogDetailRepository = new SendTextLogDetailRepository();
        public void SendMessageByMobiles(IEnumerable<string> mobiles, string subject, string message)
        {
            int sendTextLogId = SendTextLogRepository.CreateAndResultIdentity<int>(new SendTextLogModel
            {
                Message = message,
                Topic = subject,
                CreateDate = DateTimeHelper.GetCurrentTime()
            });

            List<bool> isSuccess = new List<bool>();

            SMSHttp sms = new SMSHttp();
            foreach (var phone in mobiles)
            {
                AdminResultModel<Every8DResultModel> sendResult =
                    sms.SendSMSNow(subject, message, phone);

                isSuccess.Add(sendResult.IsSuccess);

                SendTextLogDetailRepository.Create(new SendTextLogDetailModel
                {
                    IsSuccess = sendResult.IsSuccess,
                    PhoneNumber = phone,
                    ResultMessage = sendResult.ErrorMessage,
                    SendTextLogId = sendTextLogId,
                    SendTime = DateTimeHelper.GetCurrentTime(),
                    BatchId = sendResult.Result?.BatchId
                });
            }

            if (isSuccess.Any(e => e == false))
            {
                throw new Exception($"簡訊發送失敗:{isSuccess.Count(e => e == false)}封");
            }
        }
    }
}