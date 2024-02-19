using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Models;
using MyEPA.Repositories;
using System.Collections.Generic;

namespace MyEPA.Services
{

    public class SendTextLogService
    {
        SendTextLogRepository SendTextLogRepository = new SendTextLogRepository();
        SendTextLogDetailRepository SendTextLogDetailRepository = new SendTextLogDetailRepository();
        public List<SendTextLogModel> GetTextLogs()
        {
            return SendTextLogRepository.GetByTop(50);
        }

        public List<SendTextLogDetailModel> GetLogDetails(int sendTextLogId)
        {
            var result = SendTextLogDetailRepository.GetBySendTextLogId(sendTextLogId);
            bool isUpdate = false;
            foreach (var item in result)
            {
                switch (item.Status)
                {
                    case SendTextLogDetailStatusEnum.Wait:
                    case SendTextLogDetailStatusEnum.Reservation:
                        isUpdate = true;
                        //先實作一筆一筆檢查
                        Dictionary<string, DeliveryStatusResultModel> status =
                            SMSHttp.GetDeliveryStatus(item.BatchId);
                        item.Status = status.GetValue(item.PhoneNumber.Replace("+886","0"))?.Status ?? 0;
                        item.ResultMessage = item.Status.GetDescription();
                        switch (item.Status)
                        {
                            case SendTextLogDetailStatusEnum.Wait:
                            case SendTextLogDetailStatusEnum.Reservation:
                            case SendTextLogDetailStatusEnum.Success:
                                item.IsSuccess = true;
                                break;
                            default:
                                item.IsSuccess = false;
                                break;
                        }
                        break;
                }
            }
            if(isUpdate)
                SendTextLogDetailRepository.Update(result);
            return result;
        }
    }
}