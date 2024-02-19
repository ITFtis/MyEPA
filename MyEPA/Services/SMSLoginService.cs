using MyEPA.Helper;
using MyEPA.Models;

namespace MyEPA.Services
{
    public class SMSLoginService
    {
        public AdminResultModel SMSVerify(SMSVerifyLoginModel smsVerifyLogin)
        {
            AdminResultModel result = new AdminResultModel
            {
                IsSuccess = false
            };

            if (string.IsNullOrWhiteSpace(smsVerifyLogin.UserName))
            {
                result.ErrorMessage = "帳號有誤請回上一步重新輸入";
                return result;
            }

            if (string.IsNullOrWhiteSpace(smsVerifyLogin.Code))
            {
                result.ErrorMessage = "請輸入簡訊驗證碼";
                return result;
            }
            string verifyCode = GetVerifyCode(smsVerifyLogin.UserName);

            if (verifyCode == null)
            {
                result.ErrorMessage = "簡訊驗證碼已過期，請重新發送驗證碼";
                return result;
            }

            long times = GetSMSVerifyTimes(smsVerifyLogin, verifyCode);

            int maxInputSMSCount = SettingHelper.MaxInputSMSCount;

            if (times > maxInputSMSCount)
            {
                result.ErrorMessage = $"輸入錯誤達 {maxInputSMSCount} 次，請重新發送簡訊";
                return result;
            }

            if (smsVerifyLogin.Code != verifyCode)
            {
                result.ErrorMessage = $"簡訊驗證碼錯誤 {times}次，請重新輸入";
                return result;
            }

            RemoveVerifyCode(smsVerifyLogin.UserName);

            result.IsSuccess = true;

            return result;
        }
        /// <summary>
        /// 驗證碼驗證次數
        /// </summary>
        /// <param name="smsVerifyLogin"></param>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        private static long GetSMSVerifyTimes(SMSVerifyLoginModel smsVerifyLogin, string verifyCode)
        {
            var codeErrorKey = CacheKeyHelper.GetCodeErrorKey(smsVerifyLogin.UserName, verifyCode);

            long count = CacheHelper.Increment(codeErrorKey);
            return count;
        }
        private static void RemoveVerifyCode(string userName)
        {
            string key = CacheKeyHelper.GetSMSLogin(userName);

            CacheHelper.RemoveCacheByKey(key);
        }
        /// <summary>
        /// 取得驗證碼
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private static string GetVerifyCode(string userName)
        {
            string smsLoginKey = CacheKeyHelper.GetSMSLogin(userName);

            string verifyCode = CacheHelper.GetCacheValue<string>(smsLoginKey);
            return verifyCode;
        }
    }
}