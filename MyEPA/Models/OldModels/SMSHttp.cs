using MyEPA.Enums;
using MyEPA.Extensions;
using MyEPA.Helper;
using MyEPA.Models.BaseModels;
using MyEPA.Services;
using NLog;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MyEPA.Models
{
    public class SMSHttp
    {
        private static string _SendSMSUrl = "http://api.every8d.com/API21/HTTP/sendSMS.ashx";
        private static string _CreditUrl = "http://api.every8d.com/API21/HTTP/getCredit.ashx";
        static string _Id = "86158943";
        static string _Pwd = "funepa725";
        public static Dictionary<string, DeliveryStatusResultModel> GetDeliveryStatus(string batchId)
        {
            if(string.IsNullOrWhiteSpace(batchId))
            {
                return new Dictionary<string, DeliveryStatusResultModel> { };
            }
            RestClient client = new RestClient("https://oms.every8d.com/API21/HTTP/");
            RestRequest request = new RestRequest("getDeliveryStatus.ashx", Method.GET);
            request.AddParameter("UID", _Id);
            request.AddParameter("PWD", _Pwd);
            //發送批次代碼。於簡訊發送後取得。
            request.AddParameter("BID", batchId);
            //分⾴。可傳入空字串。若查詢筆數超過 1000 筆時，欲查詢第 1001~2000筆資料時，PageNo 傳入”2”。2001~3000 筆時，PageNo 傳入”3”。依此類推。
            request.AddParameter("PNO", "");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


            IRestResponse response = client.Execute(request);

            Dictionary<string, DeliveryStatusResultModel> result = new Dictionary<string, DeliveryStatusResultModel>();

            if (response.StatusCode != HttpStatusCode.OK)
            {
                NLogService.Logger.DebugT(response);
                return result;
            }
            var content = response.Content.Replace("\r", string.Empty).Split('\n');
            //result[0]筆數
            if (content[0].Replace("\t", string.Empty).Split(',')[0] != "1")
            {
                NLogService.Logger.DebugT(response);
                return result;
            }
            for (int i = 1; i < content.Length -1; i++)
            {
                try
                {
                    var values = content[i].Split('\t');
                    DeliveryStatusResultModel model = new DeliveryStatusResultModel
                    {
                        Name = values[0],
                        Mobile = values[1].Replace("+886", "0"),
                        SendTime = values[2].TryToDateTime(),
                        Cost = values[3].TryToDecimal(),
                        Status = values[4].TryToEnum<SendTextLogDetailStatusEnum>().GetValueOrDefault(),
                    };
                    result.Add(model.Mobile, model);
                }
                catch (Exception ex)
                {
                    NLogService.Logger.DebugT(ex);
                }
                
            }
            return result;
        }
        public AdminResultModel<Every8DResultModel> SendSMSNow(string subject, string content, string mobile)
        {
            return SendSMS(subject, content, mobile, DateTimeHelper.GetCurrentTime());
        }
        public AdminResultModel<Every8DResultModel> SendSMS(string subject, string content, string mobile, DateTime sendTime)
        {
            return SendSMS(subject, content, sendTime, mobile);
        }
        public AdminResultModel<Every8DResultModel> SendSMSNow(string subject, string content, IEnumerable<string> mobiles)
        {
            return SendSMS(subject, content, DateTimeHelper.GetCurrentTime(), mobiles);
        }
        public AdminResultModel<Every8DResultModel> SendSMSNow(string subject, string content, params string[] mobiles)
        {
            return SendSMS(subject, content, DateTimeHelper.GetCurrentTime(), mobiles);
        }
        public AdminResultModel<Every8DResultModel> SendSMS(string subject, string content, DateTime sendTime, IEnumerable<string> mobiles)
        {
            if (mobiles.IsEmptyOrNull())
            {
                return new AdminResultModel<Every8DResultModel> 
                {
                    IsSuccess = false
                };
            }
            return SendSMS(subject, content, sendTime, mobiles.ToArray());
        }
        public AdminResultModel<Every8DResultModel> SendSMS(string subject, string content, DateTime sendTime, params string[] mobiles)
        {
            if(mobiles.IsEmptyOrNull())
            {
                return new AdminResultModel<Every8DResultModel>
                {
                    IsSuccess = false,
                    ErrorMessage = "無手機號碼"
                };
            }

            mobiles = mobiles.Where(e => RegexHelper.IsMobile(e)).ToArray();

            
            string mibile = string.Join(",", mobiles);
            string time = sendTime.ToString("yyyyMMddHHmmss");

            return SendSMS(_Id, _Pwd, subject, content, mibile, time);
        }
        /// <summary>
        /// 傳送簡訊
        /// </summary>
        /// <param name="userID">帳號</param>
        /// <param name="password">密碼</param>
        /// <param name="subject">簡訊主旨，主旨不會隨著簡訊內容發送出去。用以註記本次發送之用途。可傳入空字串。</param>
        /// <param name="content">簡訊發送內容</param>
        /// <param name="mobile">接收人之手機號碼。格式為: +886912345678或09123456789。多筆接收人時，請以半形逗點隔開( , )，如0912345678,0922333444。</param>
        /// <param name="sendTime">簡訊預定發送時間。-立即發送：請傳入空字串。-預約發送：請傳入預計發送時間，若傳送時間小於系統接單時間，將不予傳送。格式為YYYYMMDDhhmnss；例如:預約2009/01/31 15:30:00發送，則傳入20090131153000。若傳遞時間已逾現在之時間，將立即發送。</param>
        /// <returns>true:傳送成功；false:傳送失敗</returns>
        private AdminResultModel<Every8DResultModel> SendSMS(string userID, string password, string subject, string content, string mobile, string sendTime)
        {
            AdminResultModel<Every8DResultModel> result = new AdminResultModel<Every8DResultModel>
            {
                Result = new Every8DResultModel { }
            };
            try
            {
                if (!string.IsNullOrEmpty(sendTime))
                {
                    try
                    {
                        //檢查傳送時間格式是否正確
                        DateTime checkDt = DateTime.ParseExact(sendTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                        if (!sendTime.Equals(checkDt.ToString("yyyyMMddHHmmss")))
                        {
                            result.ErrorMessage = "傳送時間格式錯誤";
                            result.IsSuccess = false;
                            return result;
                        }
                    }
                    catch
                    {
                        result.ErrorMessage = "傳送時間格式錯誤";
                        result.IsSuccess = false;
                        return result;
                    }
                }
                StringBuilder postDataSb = new StringBuilder();
                postDataSb.Append("UID=").Append(userID);
                postDataSb.Append("&PWD=").Append(password);
                postDataSb.Append("&SB=").Append(subject);
                postDataSb.Append("&MSG=").Append(content);
                postDataSb.Append("&DEST=").Append(mobile);
                postDataSb.Append("&ST=").Append(sendTime);

                string resultString = this.HttpPost(_SendSMSUrl, postDataSb.ToString());
                
                if (!resultString.StartsWith("-"))
                {
                    string[] split = resultString.Split(',');
                    result.Result.Credit = Convert.ToDecimal(split[0]);
                    result.Result.Sended = Convert.ToDecimal(split[1]);
                    result.Result.Cost = Convert.ToDecimal(split[2]);
                    result.Result.Unsend = Convert.ToDecimal(split[3]);
                    result.Result.BatchId = split[4];
                    result.Result.IsAllSuccess = !(result.Result.Unsend > 0 && result.Result.Credit == 0);
                    result.IsSuccess = true;
                }
                else
                {
                    //傳送失敗
                    result.ErrorMessage = resultString;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage = "系統錯誤，" + ex.ToString();
            }
            return result;
        }

        /// <summary>
        /// 取得帳號餘額
        /// </summary>
        /// <returns>true:取得成功；false:取得失敗</returns>
        public AdminResultModel<Every8DResultModel> GetCredit(string userID, string password)
        {
            AdminResultModel<Every8DResultModel> result = new AdminResultModel<Every8DResultModel> { };
            try
            {
                StringBuilder postDataSb = new StringBuilder();
                postDataSb.Append("UID=").Append(userID);
                postDataSb.Append("&PWD=").Append(password);

                string resultString = this.HttpPost(_CreditUrl, postDataSb.ToString());
                if (!resultString.StartsWith("-"))
                {
                    result.Result.Credit = Convert.ToDecimal(resultString);
                    result.IsSuccess = true;
                }
                else
                {
                    result.ErrorMessage = resultString;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.ToString();
            }
            return result;
        }

        private string HttpPost(string url, string postData)
        {
            string result = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] bs = System.Text.Encoding.UTF8.GetBytes(postData);
                request.ContentLength = bs.Length;
                request.GetRequestStream().Write(bs, 0, bs.Length);
                //取得 WebResponse 的物件 然後把回傳的資料讀出
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream());
                result = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }

    }
}